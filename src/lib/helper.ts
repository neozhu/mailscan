import Tesseract from "tesseract.js";
import natural from 'natural';
import Segment from 'novel-segment'
export type LanguageInfo = {
    lang: string;
    language: string;
};

export const languages=['en','de','zh'];

class TextProcessor {
    private langMap: Record<string, string> = {
        'en-US': 'eng',
        'de-DE': 'deu',
        'zh-CN': 'chi_sim',
    };

    private languageMap: Record<string, string> = {
        'en-US': 'en',
        'de-DE': 'de',
        'zh-CN': 'zh',
    };

    public getLanguageInfo(defaultLanguage: string | null): LanguageInfo {
        let lang = 'eng';
        let language = 'en';
        if (defaultLanguage) {
            if (defaultLanguage in this.langMap) {
                lang = this.langMap[defaultLanguage];
            }
            if(defaultLanguage in this.languageMap){
                language = this.languageMap[defaultLanguage];
            }
        }
        return { lang, language };
    }

    public async recognizeText(file: File, lang: string): Promise<string> {
        const langs: Tesseract.Lang[] = [lang];
        const buffer = await file.arrayBuffer();
        const worker = await Tesseract.createWorker(langs);
        const {
            data: { text }
        } = await worker.recognize(buffer);
        await worker.terminate();
        console.log('recognizeText:',lang,text);
        if(lang=='chi_sim'){
            return text.replace(/\s+/g, '')
        }
        return text;
    }

    public async tokenizer(text: string, lang: string): Promise<string[]> {
        let tokens: string[];
         if(lang=='zh'){
            const  tokenizer =new  Segment();
            tokenizer.useDefault();
            const result = tokenizer.doSegment(text.replace(/\s+/g, ''));
            tokens =  result.map(item => item.w);
        } else if(lang=='de'){
            const tokenizer = new natural.AggressiveTokenizerDe();
            tokens = tokenizer.tokenize(text)as string[];
        } else {
            const tokenizer = new natural.WordTokenizer();
            tokens = tokenizer.tokenize(text)as string[];
        }
        return tokens.filter(word => !/^\d+$/.test(word));
    }
}
const textProcessor = new TextProcessor();
export default textProcessor;