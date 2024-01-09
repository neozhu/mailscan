import eventsource from 'eventsource';
global.EventSource = eventsource;
import type { PageServerLoad, Actions } from './$types';
import { redirect } from '@sveltejs/kit';
import { NlpManager } from 'node-nlp';
import { HttpStatusCode } from '$lib/statusCodes';
import {
	type ScanHistory,
	type NlpDocument,
	type NlpEntity,
	type Department,
	type Person,
	type Entity
} from '$lib/type';
import textProcessor from '$lib/helper';
import nlpProcessor from '$lib/NlpProcessor';

export const load = (async ({ locals }) => {
	if (!locals.pb.authStore.isValid) throw redirect(HttpStatusCode.SEE_OTHER, '/login');
	await nlpProcessor.initial();
	try {
		const records: Department[] = await locals.pb.collection('department').getFullList({
			sort: '-created'
		});
		return {
			departments: records
		};
	} catch (err) {
		console.log(err);
		return {
			departments: []
		};
	}
}) satisfies PageServerLoad;

/** @type {import('./$types').Actions} */
export const actions: Actions = {
	createClassification: async ({ locals, request }) => {
		const data: Department = Object.fromEntries(await request.formData()) as unknown as {
			id: string;
			name: string;
			address: string;
			owner?: string;
		};
		data.owner = locals.user?.id;
		const result = await locals.pb.collection('department').create(data);
		return { data: result };
	},
	deleteClassification: async ({ locals, request }) => {
		const data = await request.formData();
		const id = data.get('id')?.toString();
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
			lang: string;
		};
		await nlpProcessor.removeNamedEntityText(
			data.entity,
			data.option,
			[data.lang],
			[data.sourceText]
		);
		try {
			const {items} = await locals.pb
				.collection('entities')
				.getList(1, 50, {
					filter: `name="${data.entity}" && option="${data.option}" && lang="${data.lang}"`
				});
			for (const item of items) {
				const en = item as unknown as Entity;
				if (en.keywords.length > 0) {
					const keywords = en.keywords.split(',').filter((x) => x != data.sourceText);
					en.keywords = keywords.join(',');
					if (en.keywords.length > 0) {
						await locals.pb.collection('entities').update(en.id!, en);
					} else {
						await locals.pb.collection('entities').delete(en.id!);
					}
				}
			}
		} catch (error) {
			console.log('removeKeywords:', error);
		}
		//console.log('removeNamedEntityText:', data.entity,data.option,[data.lang],[data.sourceText]);
		// if (data.option.includes('|')) {
		// 	const name = data.option.split('|')[0].trim();
		// 	const address = data.option.split('|')[1].trim();
		// 	const label='person';
		// 	try{
		// 		const record:Department =await locals.pb.collection('department').getFirstListItem(`name="${name}" && address="${address}"`);
		// 		const result = await locals.pb.collection('people').getList(1,50,{filter:`name="${data.sourceText}" && department="${record.id}"`});
		// 		result.items.forEach(async (item)=>{
		// 			const p:Person=item as unknown as Person;
		// 			await nlpProcessor.removeNamedEntityText(locals.pb,label,record.id,[p.lang??'en'],[p.name])
		// 			await locals.pb.collection('people').delete(item.id!);
		// 		});
		// 	}catch(error){
		// 		console.log('removeKeywords:',error);
		// 	}
		// }
	},
	addScanHistory: async ({ locals, request }) => {
		const data = Object.fromEntries(await request.formData()) as unknown as {
			id?: string;
			original_text: string;
			person_name?: string;
			elapsed_time: number;
			trained: boolean;
			department?: string;
			words: string;
			extracted_words: string;
			lang: string;
			owner: string;
		};
		//console.log('department:', data.department);
		let option = data.person_name ?? '';
		if (data.lang == 'zh' && data.person_name) {
			option = data.person_name.replace(/\s+/g, '');
		}

		data.owner = locals.user?.id;
		let label: string = data.department ?? '';
		try {
			const department: Department = await locals.pb
				.collection('department')
				.getOne(data.department!);
			label = `${department.name} | ${department.address}`;
			const newEntity: Entity = {
				id: '',
				name: label,
				option: option,
				keywords: data.extracted_words,
				owner: locals.user?.id,
				lang: data.lang
			};
			await locals.pb.collection('entities').create(newEntity);
			await locals.pb.collection('scanHistories').create(data);
		} catch (error) {
			console.log(error);
		}
		await nlpProcessor.addNamedEntityText(label, option, [data.lang], [data.person_name ?? '']);
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
					extracted_words: '',
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
