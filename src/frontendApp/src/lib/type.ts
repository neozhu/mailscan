export interface Department{
     id:string,
    name:string,
    address:string
}
export interface Person{
    id:string,
    name:string,
    email?:string,
    phoneNumber?:string,
    department?: string,
    expand: {
        department: Department;
    };
}
export interface ScanHistory{
    id?:string,
    original_text:string,
    person_name?:string,
    elapsed_time:number,
    trained:boolean,
    department?:string,
    words:string,
    lang:string
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