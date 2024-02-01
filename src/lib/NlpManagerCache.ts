import { NlpManager } from 'node-nlp';
import {createCache, memoryStore, type MemoryCache} from  'cache-manager';
import { languages } from './helper';
import { pb } from '$lib/pocketbase';
import type { Entity } from './type';
class NlpManagerCache {
    private memoryCache:MemoryCache;
    public Labels = new Map<string,string[]>();
    private readonly config={
        languages: languages,
        forceNER: true,
        ner: { threshold: 0.8 }
    };
    constructor() {
        this.memoryCache = createCache(memoryStore(), {
            max: 100,
            ttl: 30 * 1000 /*milliseconds*/,
          });
    }

    async getOrCreateNlpManager(userId:string):Promise<NlpManager> {
        const cacheKey = `nlp-manager-${userId}`;
        let manager = (await this.memoryCache.get(cacheKey))  as NlpManager;
        if (!manager) {
            manager = await this.createNlpManager(userId);
            await this.memoryCache.set(cacheKey, manager);
        }
        return manager;
    }
    async createNlpManager(userId:string):Promise<NlpManager> {
        const manager = new NlpManager(this.config);
        const entities: Entity[] = await pb.collection('entities').getFullList({
            sort: '-created',
            expand: 'department'
        });
        const names=new Set<string>;
        for (const en of entities) {
            const keywords = en.keywords
                .split(',')
                .map((s) => s.trim())
                .filter((x) => x.length > 0);
            manager.addNamedEntityText(en.name, en.option, [en.lang], keywords);
            names.add(en.name);
        }
        this.Labels.set(userId,[...names]);
        //console.log('createNlpManager instance:',userId)
        return manager;
    }
    async refresh(userId:string):Promise<void>{
        const cacheKey = `nlp-manager-${userId}`;
        const manager =await this.createNlpManager(userId);
        await this.memoryCache.set(cacheKey, manager);
    }
}

const nlpManagerCache = new NlpManagerCache();
export default nlpManagerCache;