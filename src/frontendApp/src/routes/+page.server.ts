import type { PageServerLoad, Actions } from './$types';
import { redirect } from '@sveltejs/kit';
import { NlpManager } from 'node-nlp';
import natural from 'natural';
import { HttpStatusCode } from '$lib/statusCodes';
import type { ScanHistory, NlpDocument, NlpEntity, Department, Person } from '$/lib/type';
 import {recognizeText,getLanguageInfo,tokenizer,saveScanHistory} from '$lib/helper';

export const load = (async ({ locals }) => {
	if (!locals.pb.authStore.isValid) throw redirect(HttpStatusCode.SEE_OTHER, '/login');

	const records: Department = await locals.pb.collection('department').getFullList({
		sort: '-created'
	});
	return {
		departments: records
	};
}) satisfies PageServerLoad;

/** @type {import('./$types').Actions} */
export const actions: Actions = {
	process: async ({ locals, request }) => {
		const start = performance.now();
		const acceptLanguageHeader = request.headers.get('accept-language');
		const { lang, language } = getLanguageInfo(acceptLanguageHeader);
		const data = await request.formData();
		const file = data.get('image') as File;

		if (file && file instanceof File) {
			const text = await recognizeText(file, lang);

			const manager = new NlpManager({ languages: [language], forceNER: true });
			manager.addNamedEntityText(
				'person',
				'personName',
				['en'],
				['Hendry Liguna', 'LICAN', 'Dr. Strange']
			);
			const doc: NlpDocument = await manager.process(language, text);
			const labels: string[] = ['person'];
			const entities: NlpEntity[] = doc.entities;
			const personEnt: NlpEntity[] = entities.filter((x) => labels.includes(x.entity));
			if (personEnt && personEnt.length > 0) {
				const personName = personEnt[0].sourceText;
				console.log('personName:' + personName);
				let person: Person;
				try {
					const fitler = `name="${personName}"`;
					person = (await locals.pb.collection('people').getFirstListItem(fitler, {
						expand: 'department'
					})) as Person;
					console.log(person);
					return { success: true, person };
				} catch (error) {
					console.log('not found person with name:' + personName);
					const tokens =await tokenizer(text,language);
					const end = performance.now();
					const executionTime = +((end - start) / 1000).toFixed(5); // 执行时间（毫秒）
					const record = await saveScanHistory(locals.pb, text, lang,tokens,executionTime);
					return { success: true, record };
				}
			} else {
				const tokens = await tokenizer(text,language);
				const end = performance.now();
				const executionTime = +((end - start) / 1000).toFixed(5); // 执行时间（毫秒）
				const record = await saveScanHistory(locals.pb, text, lang,tokens,executionTime);
				return { success: true, record };
			}
		}
		return { success: true };
	}
};


