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
			lang:data.lang
		}
		data.owner = locals.user?.id;
		try{
			const exist:Person=await locals.pb.collection('people').getFirstListItem(`name="${data.person_name}"`) ;
			exist.department = data.department;
			const personerecord = await locals.pb.collection('people').update(exist.id!,exist);
		}catch{
			const personerecord = await locals.pb.collection('people').create(person);
		}
	    await nlpProcessor.addNamedEntityText( locals.pb,'person',data.department??'',[data.lang],[data.person_name??'']);
		const historyrecord = await locals.pb.collection('scanHistories').create(data);
		//console.log('addScanHistory:', historyrecord);
	},
	process: async ({ locals, request }) => {
		const start = performance.now();
		const data = await request.formData();
		const defaultLanguage = data.get('lang')?.toString()??'en-US';
		const { lang, language } = textProcessor.getLanguageInfo(defaultLanguage);
		const file = data.get('image') as File;

		if (file && file instanceof File) {
			const text = await textProcessor.recognizeText(file, lang);
		    const personEntities: NlpEntity[] =await nlpProcessor.process(text,language);

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
					owner:locals.user?.id
				};
				return { success: true,words:tokens.length, record };
			}
		}
		return { success: true };
	}
};


