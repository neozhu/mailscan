import PocketBase from 'pocketbase';
import { NlpManager } from 'node-nlp';
import { languages } from './helper';

import type { Person, NlpDocument, NlpEntity, Department } from '$lib/type'

export const nlp = new NlpManager({ languages: languages, forceNER: true });
class NlpProcessor {
    private initialized = false;
    public async initial(pb: PocketBase): Promise<boolean> {
        if (this.initialized) {
            return false;
        }
        try {
            const records: Person[] = await pb.collection('people').getFullList({
                sort: '-created',
                expand: 'department'
            });
            const groupedByDepartment: Record<string, Person[]> = records.reduce((acc, person) => {
                const dept: string = person.department ?? '';
                if (!acc[dept]) {
                    acc[dept] = [];
                }
                acc[dept].push(person);
                return acc;
            }, {});
            Object.keys(groupedByDepartment).forEach(department => {
                const category = `${groupedByDepartment[department][0].expand.department.name} | ${groupedByDepartment[department][0].expand.department.address}`;
                //const category = `${groupedByDepartment[department][0].expand.department.name}`;
                let keywords: string[] = [];
                let languages: Set<string> = new Set<string>();

                groupedByDepartment[department].forEach(person => {
                    keywords.push(person.name);
                    languages.add(person.lang == '' ? 'en' : person.lang);
                });
                let label: string = 'person';
                //console.log('addNamedEntityText', label, category, [...languages], keywords)
                nlp.addNamedEntityText(label, category, [...languages], keywords);
            });
            this.initialized=true;
        } catch (error) {
            console.log('NlpProcessor.initial', error)
        }
        return true;
    }

    public async process(text: string, language: string = 'en'): Promise<NlpEntity[]> {
        const doc: NlpDocument = await nlp.process(language, text);
        //console.log('doc', doc)
        const labels: string[] = ['person'];
        const entities: NlpEntity[] = doc.entities;
        const personEntities: NlpEntity[] = entities.filter((x) => labels.includes(x.entity));
        //console.log('personEntities', personEntities)
        return personEntities;
    }

    public async addNamedEntityText(db:PocketBase,label: string, departmentId: string, languages: string[] = ['en'], keywords: string[]) {
        let category='';
        try{
            const record:Department = await db.collection('department').getOne(departmentId);
            category = `${record.name} | ${record.address}`;
            nlp.addNamedEntityText(label, category, languages, keywords);
            //console.log('addNamedEntityText',label, category, languages, keywords)
        }catch(error){
            console.log('addNamedEntityText:',error);
        }
    }
    public async removeNamedEntityText(db:PocketBase,label: string, departmentId: string, languages: string[] = ['en'], keywords: string[]) {
        let category='';
        try{
            const record:Department = await db.collection('department').getOne(departmentId);
            category = `${record.name} | ${record.address}`;
            nlp.removeNamedEntityText(label, category, languages, keywords);
            //console.log('removeNamedEntityText',label, category, languages, keywords)
        }catch(error){
            console.log('addNamedEntityText:',error);
        }
    }
}

const nlpProcessor = new NlpProcessor();
export default nlpProcessor;