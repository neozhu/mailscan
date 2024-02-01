import nlpManagerCache from '$lib/NlpManagerCache';
import type { NlpDocument, NlpEntity } from '$lib/type';
import type { NlpManager } from 'node-nlp-typescript';


class NlpProcessor {
	public async initial(userId:string): Promise<boolean> {
		console.log('NlpProcessor.initial()',userId);

		try {
		   await nlpManagerCache.getOrCreateNlpManager(userId);
			// Subscribing to all changes ('*') on the 'entities' collection in PocketBase.
			// delte -> removeNamedEntityText
			// create -> addNamedEntityText
			// pb.collection('entities').subscribe('*', function (e) {
			// 	if(e.action=='delete'){
			// 		const del = e.record as unknown as Entity;
			// 		nlp.removeNamedEntityText(del.name,del.option,[del.lang],del.keywords.split(',').map(x=>x.trim()));
			// 	}else if(e.action=='create'){
			// 		const created = e.record as unknown as Entity;
			// 		nlp.addNamedEntityText(created.name,created.option,[created.lang],created.keywords.split(',').map(x=>x.trim()));
			// 	}else if(e.action=='update'){
			// 		const updated = e.record as unknown as Entity;
			// 		nlp.removeNamedEntityText(updated.name,updated.option,[updated.lang],updated.keywords.split(',').map(x=>x.trim()));
			// 		nlp.addNamedEntityText(updated.name,updated.option,[updated.lang],updated.keywords.split(',').map(x=>x.trim()));
			// 	}
				
			// }, { /* other options like expand, custom headers, etc. */ });
		} catch (error) {
			console.log('NlpProcessor.initial', error);
		}
		return true;
	}

	public async process(userId:string,text: string, language: string = 'en'): Promise<NlpEntity[]> {
		const nlp =  await nlpManagerCache.getOrCreateNlpManager(userId);
		const doc: NlpDocument = await nlp.process(language, text);
		//console.log('doc', doc)
		const labels = nlpManagerCache.Labels.get(userId)??[];
		const entities: NlpEntity[] = doc.entities;
		const personEntities: NlpEntity[] = entities.filter((x) => labels.includes(x.entity));
		//console.log('process result:', personEntities)
		const unique = new Set();
		const filteredResults = personEntities.filter((record) => {
			const key = `${record.entity}_${record.option}`;
			if (unique.has(key)) {
				return false;
			}
			unique.add(key);
			return true;
		});
		return filteredResults;
	}

	public async refresh(userId:string): Promise<void> {
		try {
		  await	nlpManagerCache.refresh(userId);
		} catch (error) {
			console.log('refresh:', error);
		}
	}
	 
}

const nlpProcessor = new NlpProcessor();
export default nlpProcessor;
