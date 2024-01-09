export interface Department{
    id:string,
    name:string,
    address:string,
    owner?:string
}
export interface Person{
    id?:string,
    name:string,
    email?:string,
    phoneNumber?:string,
    department?: string,
    owner?:string,
    lang?:string,
    expand?: {
        department: Department;
    };
}
export interface Entity{
    id?:string,
    name:string,
    lang:string,
    option:string,
    keywords:string,
    owner:string,
    collectionName:'entity'
}
export interface ScanHistory{
    id?:string,
    original_text:string,
    person_name?:string,
    elapsed_time:number,
    trained:boolean,
    department?:string,
    words:string,
    extracted_words:string,
    lang:string,
    owner?:string
}
export interface NlpDocument {
    entities: NlpEntity[];
}
export interface NlpEntity {
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
