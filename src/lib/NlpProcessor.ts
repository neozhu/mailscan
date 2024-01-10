import { NlpManager } from 'node-nlp';
import { languages } from './helper';
import { pb } from '$lib/pocketbase';
import type { NlpDocument, NlpEntity, Entity } from '$lib/type';

export const nlp = new NlpManager({
	languages: languages,
	forceNER: true,
	ner: { threshold: 0.8 }
});
class NlpProcessor {
	private initialized = false;
	private labels = new Set<string>();
	public async initial(): Promise<boolean> {
		if (this.initialized) {
			return false;
		}
		try {
			const entities: Entity[] = await pb.collection('entities').getFullList({
				sort: '-created',
				expand: 'department'
			});
			for (const en of entities) {
				const keywords = en.keywords
					.split(',')
					.map((s) => s.trim())
					.filter((x) => x.length > 0);
				nlp.addNamedEntityText(en.name, en.option, [en.lang], keywords);
				this.labels.add(en.name);
				//console.log('addNamedEntityText', en.name, en.option, [en.lang], keywords);
			}
			// Subscribing to all changes ('*') on the 'entities' collection in PocketBase.
			// delte -> removeNamedEntityText
			// create -> addNamedEntityText
			pb.collection('entities').subscribe('*', function (e) {
				if(e.action=='delete'){
					const del = e.record as unknown as Entity;
					nlp.removeNamedEntityText(del.name,del.option,[del.lang],del.keywords.split(',').map(x=>x.trim()));
				}else if(e.action=='create'){
					const created = e.record as unknown as Entity;
					nlp.addNamedEntityText(created.name,created.option,[created.lang],created.keywords.split(',').map(x=>x.trim()));
				}else if(e.action=='update'){
					const updated = e.record as unknown as Entity;
					nlp.removeNamedEntityText(updated.name,updated.option,[updated.lang],updated.keywords.split(',').map(x=>x.trim()));
					nlp.addNamedEntityText(updated.name,updated.option,[updated.lang],updated.keywords.split(',').map(x=>x.trim()));
				}
				
			}, { /* other options like expand, custom headers, etc. */ });
			this.initialized = true;
		} catch (error) {
			console.log('NlpProcessor.initial', error);
		}
		return true;
	}

	public async process(text: string, language: string = 'en'): Promise<NlpEntity[]> {
		const doc: NlpDocument = await nlp.process(language, text);
		//console.log('doc', doc)
		const labels: string[] = [...this.labels];
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

	public async addNamedEntityText(
		label: string,
		option: string,
		languages: string[] = ['en'],
		keywords: string[]
	) {
		try {
			nlp.addNamedEntityText(label, option, languages, keywords);
			//console.log('addNamedEntityText',label, category, languages, keywords)
		} catch (error) {
			console.log('addNamedEntityText:', error);
		}
	}
	public async removeNamedEntityText(
		label: string,
		option: string,
		languages: string[] = ['en'],
		keywords: string[]
	) {
		try {
			nlp.removeNamedEntityText(label, option, languages, keywords);
			//console.log('removeNamedEntityText',label, category, languages, keywords)
		} catch (error) {
			console.log('addNamedEntityText:', error);
		}
	}
}

const nlpProcessor = new NlpProcessor();
export default nlpProcessor;
