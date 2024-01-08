import type { PageServerLoad, Actions } from './$types';
import { redirect } from '@sveltejs/kit';
import { NlpManager } from 'node-nlp';
import { HttpStatusCode } from '$lib/statusCodes';
import { type ScanHistory, type NlpDocument, type NlpEntity, type Department, type Person } from '$lib/type';
import textProcessor from '$lib/helper';
import nlpProcessor from '$lib/NlpProcessor';



export const load = (async ({ locals }) => {
	if (!locals.pb.authStore.isValid) throw redirect(HttpStatusCode.SEE_OTHER, '/login');
	await nlpProcessor.initial(locals.pb);
	try {
		const records: Department[] = await locals.pb.collection('department').getFullList({
			sort: '-created'
		});
		return {
			departments: records
		};
	} catch (err) {
		console.log(err)
		return {
			departments: []
		};
	}

}) satisfies PageServerLoad;

/** @type {import('./$types').Actions} */
export const actions: Actions = {
	createClassification: async({ locals, request })=>{
		const data:Department = Object.fromEntries(await request.formData()) as unknown as {
			 id:string,
			 name:string,
			 address:string,
			 owner?:string
		};
		data.owner = locals.user?.id;
		const result = await locals.pb.collection('department').create(data);
		return {data:result}
	},
	deleteClassification:async({ locals, request })=>{
		const data = await request.formData();
		const id=data.get('id')?.toString();
		await locals.pb.collection('department').delete(id!);
	},
	removeKeywords: async ({ locals, request }) => {
		const data: NlpEntity = Object.fromEntries(await request.formData()) as unknown as {
			start: number;
			end: number;
			len: number;
			levenshtein: number;
			accuracy: number;
			entity: string;
			type: string;
			option: string;
			sourceText: string;
			utteranceText: string;
		};
		if (data.option.includes('|')) {
			const name = data.option.split('|')[0].trim();
			const address = data.option.split('|')[1].trim();
			const label='person';
			try{
				const record:Department =await locals.pb.collection('department').getFirstListItem(`name="${name}" && address="${address}"`);
				const result = await locals.pb.collection('people').getList(1,50,{filter:`name="${data.sourceText}" && department="${record.id}"`});
				result.items.forEach(async (item)=>{
					const p:Person=item as unknown as Person;
					await nlpProcessor.removeNamedEntityText(locals.pb,label,record.id,[p.lang??'en'],[p.name])
					await locals.pb.collection('people').delete(item.id!);
				});
			}catch(error){
				console.log('removeKeywords:',error);
			}
		}
	},
	addScanHistory: async ({ locals, request }) => {
		const data = Object.fromEntries(await request.formData()) as unknown as {
			id?: string,
			original_text: string,
			person_name?: string,
			elapsed_time: number,
			trained: boolean,
			department?: string,
			words: string,
			lang: string,
			owner: string
		};
		//console.log('department:', data.department);
		const person: Person = {
			name: data.person_name ?? '',
			email: '',
			phoneNumber: '',
			department: data.department,
			owner: locals.user?.id,
			lang: data.lang
		}
		data.owner = locals.user?.id;
		try {
			const exist: Person = await locals.pb.collection('people').getFirstListItem(`name="${data.person_name}"`);
			exist.department = data.department;
			const personerecord = await locals.pb.collection('people').update(exist.id!, exist);
		} catch {
			const personerecord = await locals.pb.collection('people').create(person);
		}
		await nlpProcessor.addNamedEntityText(locals.pb, 'person', data.department ?? '', [data.lang], [data.person_name ?? '']);
		const historyrecord = await locals.pb.collection('scanHistories').create(data);
		//console.log('addScanHistory:', historyrecord);
	},
	process: async ({ locals, request }) => {
		const start = performance.now();
		const data = await request.formData();
		const defaultLanguage = data.get('lang')?.toString() ?? 'en-US';
		const { lang, language } = textProcessor.getLanguageInfo(defaultLanguage);
		const file = data.get('image') as File;

		if (file && file instanceof File) {
			const text = await textProcessor.recognizeText(file, lang);
			const personEntities: NlpEntity[] = await nlpProcessor.process(text, language);

			if (personEntities && personEntities.length > 0) {
				const personName = personEntities[0].sourceText;
				//console.log('personName:',personName)
				return { success: true, personEntities };
			} else {
				const tokens = await textProcessor.tokenizer(text, language);
				const end = performance.now();
				const executionTime = +((end - start) / 1000).toFixed(5); // 执行时间（毫秒）
				const record: ScanHistory = {
					original_text: text,
					department: '',
					person_name: '',
					words: JSON.stringify(tokens),
					elapsed_time: executionTime,
					trained: false,
					lang: language,
					owner: locals.user?.id
				};
				return { success: true, words: tokens.length, record };
			}
		}
		return { success: true };
	}
};


