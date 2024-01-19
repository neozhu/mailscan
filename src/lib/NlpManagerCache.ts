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

    async getOrCreateNlpManager(userId:string):NlpManager {
        const cacheKey = `nlp-manager-${userId}`;
        let manager = await this.memoryCache.get(cacheKey);
        
        if (!manager) {
            manager = this.createNlpManager(userId);
            await this.memoryCache.set(cacheKey, manager);
        }
        return manager;
    }
    async createNlpManager(userId:string):NlpManager {
        console.log('createNlpManager instance:',userId)
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
        return manager;
    }
    async refresh(userId:string){
        const cacheKey = `nlp-manager-${userId}`;
        const manager = this.createNlpManager(userId);
        await this.memoryCache.set(cacheKey, manager);
    }
}

const nlpManagerCache = new NlpManagerCache();
export default nlpManagerCache;