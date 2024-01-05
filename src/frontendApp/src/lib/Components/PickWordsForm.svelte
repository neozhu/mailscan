<script lang="ts">
	import { Check, Save } from 'svelte-lucide';
	import { departments } from '$lib/pocketbase';
	import type { Department,ScanHistory } from '$lib/type';

	export let record:ScanHistory
	console.log(record)
	let selected_dep:string = '';
	let selected_lang:string =record&&record.lang? record.lang:'eng';
	let selected_words:string[]=[];

	$: words = record && record.words 
        ? parseWords(record.words) 
        : [];

    function parseWords(jsonString: string): string[] {
        try {
            return JSON.parse(jsonString) as string[];
        } catch (e) {
            return [];
        }
    }
	function addWord(word:string):void{
		if(selected_words.length===2){
			selected_words.shift();
		}
		selected_words.push(word);
	}
	
	function toggleWordsHandler(word: string): void {
		addWord(word);
	}

	function toggleDepartmentHandler(dep: Department):void {
		selected_dep = dep.id;
	}
	function toggleLanguageHandler(lang:string):void{
		selected_lang = lang
	}
	let langMap:Record<string,string>={
		eng:'English',
		deu:'Deutsch',
		chi_sim:'中文'
	}
</script>

<div class="t-10 my-20">
	<div class="card variant-glass-surface p-4">
		<h6 class="h6 my-1.5" data-toc-ignore="">
			Please select the words from the list below that are names of people.
		</h6>
		<article class="gap-1 overflow-y-auto focus:overscroll-contain overscroll-auto max-h-28">
			<div class="flex gap-1 flex-wrap">
				{#each words as word}
					<button
						class="chip {selected_words.includes(word)? 'bg-gradient-to-br variant-gradient-primary-secondar' : 'variant-soft'}"
						on:click={() => {
							toggleWordsHandler(word);
						}}
						on:keypress
					>
						{#if selected_words.includes(word)}<Check size="16">(icon)</Check>{/if}
						<span class="capitalize">{word}</span>
					</button>
				{/each}
			</div>

			 
		</article>

		<h6 class="h6 my-1.5 mt-5" data-toc-ignore="">
			Having selected the person's name, please now choose the department they belong to.
		</h6>
		<div class="flex gap-1 mb-6">
			{#if $departments}
				{#each $departments as d}
					<button
						class="chip {selected_dep == d.id ? 'bg-gradient-to-br variant-gradient-secondary-primary' : 'variant-soft'}"
						on:click={() => {
							toggleDepartmentHandler(d);
						}}
						on:keypress
					>
						{#if selected_dep == d.id}<Check size="16"></Check>{/if}
						<span class="capitalize">{d.name} | {d.address}</span>
					</button>
				{/each}
			{/if}
		</div>
		<h6 class="h6 my-1.5 mt-5" data-toc-ignore="">Please choose the language.</h6>
		<div class="flex gap-1 mb-6">
			{#each Object.keys(langMap) as lang}
				<button
					class="chip {selected_lang === lang ? 'bg-gradient-to-br variant-gradient-tertiary-secondary' : 'variant-soft'}"
					on:click={() => {
						toggleLanguageHandler(lang);
					}}
					on:keypress
				>
					{#if selected_lang === lang} <Check size="16"></Check> {/if}
					<span>{langMap[lang]}</span>
				</button>
			{/each}
		</div>

		<footer class="card-footer text-end">
			<button type="button" class="btn variant-filled-tertiary ">
				<span><Save /></span>
				<span>Save</span>
			</button>
		</footer>
	</div>
</div>
