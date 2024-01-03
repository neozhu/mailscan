import type { PageServerLoad, Actions } from './$types';
import Tesseract  from 'tesseract.js';
import { NlpManager } from 'node-nlp';


 

export const load = (async () => {
	return {};
}) satisfies PageServerLoad;

/** @type {import('./$types').Actions} */
export const actions: Actions = {
	process: async ({ request }) => {
		const data = await request.formData();
		const file = data.get('image') as File;

		if (file && file instanceof File) {
			console.log(file);
			const buffer = await file.arrayBuffer();
			const worker = await Tesseract.createWorker('eng');
			const { data: { text } } = await worker.recognize(buffer,{rotateAuto: true});
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
				['voith','Voith','VOITH']
			  );
			  const doc = await manager.process('en', text);
			  const entities:String[]=['person','place','phonenumber','org'];
			  const docentities = doc.entities.filter(x=>entities.includes(x.entity))
			  console.log(docentities);
			  console.log('==========================')
			  
		}
		return { success: true };
	}
};
