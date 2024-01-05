import type { PageServerLoad, Actions } from './$types';
import { redirect } from '@sveltejs/kit';
import Tesseract from 'tesseract.js';
import { NlpManager } from 'node-nlp';
import natural from 'natural';
import { HttpStatusCode } from '$lib/statusCodes';


interface NlpEntity {
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
}
export const load = (async ({locals}) => {
	if (!locals.pb.authStore.isValid) throw redirect(HttpStatusCode.SEE_OTHER, '/login');
}) satisfies PageServerLoad;

/** @type {import('./$types').Actions} */
export const actions: Actions = {
	process: async ({locals, request }) => {
		const start = performance.now();
		const acceptLanguageHeader = request.headers.get('accept-language');
		if(acceptLanguageHeader){
			const preferredLanguage = acceptLanguageHeader.split(',')[0].split(';')[0];
			console.log(preferredLanguage);
		}
		const data = await request.formData();
		const file = data.get('image') as File;

		if (file && file instanceof File) {
			//console.log(file);
			const buffer = await file.arrayBuffer();
			const worker = await Tesseract.createWorker('eng');
			const { data: { text } } = await worker.recognize(buffer);
			await worker.terminate();
			console.log(text);
			// const { data: { text } } = await Tesseract.recognize(buffer, 'eng', {
			// 	rotateAuto: true,
			// });
			// console.log(text);

			console.log('==========================');

			const manager = new NlpManager({ languages: ['en'], forceNER: true });
			manager.addNamedEntityText(
				'person',
				'personName',
				['en'],
				['Hendry Liguna', 'Bruce Wayne', 'Dr. Strange']
			);
			const doc = await manager.process('en', text);
			const labels: string[] = ['person'];
			const entities:NlpEntity[] = doc.entities;
			const person = entities.filter((x) => labels.includes(x.entity));
			if(person && person.length>0){
				console.log(person);
			}else{
				const end = performance.now();
				const executionTime = ((end - start)/1000).toFixed(5); // 执行时间（毫秒）
				const scanHistory = {
					"original_text": text,
					"elapsed_time": executionTime,
					"trained": false,
					"department": null
				};
				const record = await locals.pb.collection('scanHistories').create(scanHistory);
				console.log('create scanhistory',record);
				return {success:true,record};
				
			}
		
			console.log('==========================')
			const tokenizer = new natural.WordPunctTokenizer();
			const tokens = tokenizer.tokenize(text);
			console.log(tokens);
			console.log('==========================')
		}
		return { success: true };
	}
};
