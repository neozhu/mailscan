import Tesseract from "tesseract.js";
import PocketBase, { type RecordModel } from 'pocketbase';
import natural from 'natural';
import type { NlpEntity, Person, ScanHistory } from "./type";


// place files you want to import through the `$lib` alias in this folder.

export type LanguageInfo = {
    lang: string;
    language: string;
};

export const langMap: Record<string, string> = {
    'en-US': 'eng',
    'de-DE': 'deu',
    'zh-CN': 'chi_sim',
};

export const languageMap: Record<string, string> = {
    'en-US': 'en',
    'de-DE': 'de',
    'zh-CN': 'zh',
};
export function getLanguageInfo(acceptLanguageHeader: string | null): LanguageInfo {
    let lang = 'eng';
    let language = 'en';
    if (acceptLanguageHeader) {
        const preferredLanguage = acceptLanguageHeader.split(',')[0].split(';')[0];
        if (preferredLanguage in langMap) {
            lang = langMap[preferredLanguage];
        }
        if(preferredLanguage in languageMap){
            language = languageMap[preferredLanguage];
        }
    }
    return { lang, language };
}


export async function recognizeText(file: File, lang: string): Promise<string> {
    const langs: Tesseract.Lang[] = [lang as unknown as Tesseract.Lang];
	const buffer = await file.arrayBuffer();
	const worker = await Tesseract.createWorker(langs);
	const {
		data: { text }
	} = await worker.recognize(buffer);
	await worker.terminate();
	// const { data: { text } } = await Tesseract.recognize(buffer, 'eng', {
	// 	rotateAuto: true,
	// });
	// console.log(text);
	return text;
}

export async function tokenizer (text:string,lang:string):Promise<string[]>{
    let tokens:string[];
    if(lang=='ch'){
        const tokenizer = new natural.WordTokenizer();
        tokens = tokenizer.tokenize(text) as string[];
       
    }else if(lang=='de'){
        const tokenizer = new natural.AggressiveTokenizerDe();
        tokens = tokenizer.tokenize(text)as string[];
       
    }else{
        const tokenizer = new natural.WordTokenizer();
        tokens = tokenizer.tokenize(text)as string[];
    }
    //console.log(tokens);
    return tokens.filter(word => !/^\d+$/.test(word));
}

export async function saveScanHistory(pb:PocketBase, text:string,lang:string, words:string[],executionTime:number):Promise<RecordModel>{
    const history: ScanHistory = {
        original_text: text,
        words: JSON.stringify(words),
        elapsed_time: executionTime,
        trained: false,
        lang: lang,

    };
    const record = await pb.collection('scanHistories').create(history);
    console.log('saveScanHistory:', record);
    return record;
}
    
