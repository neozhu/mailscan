import type { PageServerLoad, Actions } from './$types';
import Tesseract from 'tesseract.js';
import { NlpManager } from 'node-nlp';
import natural from 'natural';
import nlp from 'compromise';



export const load = (async () => {
	return {};
}) satisfies PageServerLoad;

/** @type {import('./$types').Actions} */
export const actions: Actions = {
	process: async ({ request }) => {
		const acceptLanguageHeader = request.headers.get('accept-language');
		if(acceptLanguageHeader){
			const preferredLanguage = acceptLanguageHeader.split(',')[0].split(';')[0];
			console.log(preferredLanguage);
		}
		const data = await request.formData();
		const file = data.get('image') as File;

		if (file && file instanceof File) {
			console.log(file);
			const buffer = await file.arrayBuffer();
			const worker = await Tesseract.createWorker('eng');
			const { data: { text } } = await worker.recognize(buffer, { rotateAuto: true });
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
			manager.addNamedEntityText(
				'place',
				'placeName',
				['en'],
				['Karawang 41361, West Java']
			);
			manager.addNamedEntityText(
				'org',
				'orgName',
				['en'],
				['voith', 'Voith', 'VOITH', "AMD"]
			);
			manager.addNamedEntityText(
				'product',
				'productName',
				['en'],
				['GRAPHICS CARD', 'TUF']
			);
			const doc = await manager.process('en', text);
			const entities: String[] = ['person', 'place', 'phonenumber', 'org','product'];
			const docentities = doc.entities.filter((x:any) => entities.includes(x.entity))
			console.log(docentities);
			console.log('==========================')
			const tokenizer = new natural.WordTokenizer();
			let tokens = tokenizer.tokenize(text);
			console.log(tokens);
			console.log('==========================')
		}
		return { success: true };
	}
};
