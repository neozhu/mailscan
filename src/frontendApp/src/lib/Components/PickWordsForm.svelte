<script lang="ts">
	import type { SvelteComponent } from 'svelte';
	import { Check, Save, XCircle } from 'svelte-lucide';
	import { departments } from '$lib/pocketbase';
	import {
		getModalStore,
		getToastStore,
		type ToastSettings,
		type ModalStore,
		type ToastStore
	} from '@skeletonlabs/skeleton';
	import type { Department, ScanHistory } from '$lib/type';

	const modalStore: ModalStore = getModalStore();
	const toastStore: ToastStore = getToastStore();
	export let parent: SvelteComponent;
	let record: ScanHistory = $modalStore[0].meta.record;
	let selected_dep: string = '';
	let selected_lang: string = record && record.lang ? record.lang : 'eng';
	let selected_words: string[] = [];
	let personName: string = '';
	let words = record && record.words ? parseWords(record.words) : {};

	function parseWords(jsonString: string): Record<string, boolean> {
		try {
			const words = JSON.parse(jsonString) as string[];
			return words.reduce(
				(acc, word) => {
					acc[word] = false;
					return acc;
				},
				{} as Record<string, boolean>
			);
		} catch (e) {
			console.log(e);
			return {};
		}
	}
	function toggleWordsHandler(w: string): void {
		words[w] = !words[w];
		if (words[w]) {
			selected_words.push(w);
			if (selected_words.length > 2) {
				const removed = selected_words.shift();
				if (removed) words[removed] = false;
			}
		} else {
			selected_words = selected_words.filter((x) => x != w);
		}
		if (selected_words) {
			personName = selected_words.join(' ');
		} else {
			personName = '';
		}
	}

	function toggleDepartmentHandler(dep: Department): void {
		selected_dep = dep.id;
	}
	function toggleLanguageHandler(lang: string): void {
		selected_lang = lang;
	}
	function objectToFormData(record: any): FormData {
		const formData = new FormData();
		for (const key in record) {
			if (record.hasOwnProperty(key)) {
				console.log(key, record[key])
				formData.append(key, record[key]);
			}
		}
		return formData;
	}
	async function saveScanHsitoryHandler() {
		try {
			if (personName == '') {
				const toast: ToastSettings = {
					message: 'no name has been selected.',
					timeout: 2000,
					autohide: true,
					background: 'variant-glass-error'
				};
				toastStore.trigger(toast);
				return;
			}
			if (selected_dep == '') {
				const toast: ToastSettings = {
					message: 'no deparment has been selected.',
					timeout: 2000,
					autohide: true,
					background: 'variant-glass-error'
				};
				toastStore.trigger(toast);
				return;
			}
			record.person_name = personName;
			record.department = selected_dep;
		   const formData = objectToFormData(record);
			const response = await fetch('?/addScanHistory', {
				method: 'POST',
	     		body: formData
			});
			if (!response.ok) {
				throw new Error('Network response was not ok');
			}
			const result = await response.json();
			console.log(result);
			parent.onClose();
		} catch (error) {
			console.error('Error:', error);
		}
	}
	let langMap: Record<string, string> = {
		eng: 'English',
		deu: 'Deutsch',
		chi_sim: '中文'
	};
</script>

<div class="t-10 my-20">
	<div class="card variant-glass-surface p-4">
		<header class="card-header flex justify-end static">
			<button
				class="btn-icon hover:variant-soft-primary absolute top-0 right-0"
				on:click={parent.onClose}><XCircle /></button
			>
		</header>
		<h6 class="h6 my-1.5" data-toc-ignore="">
			Please select the words from the list below that are names of people.
		</h6>
		<article class="gap-1 overflow-y-auto focus:overscroll-contain overscroll-auto max-h-28">
			<div class="flex gap-1 flex-wrap">
				{#each Object.keys(words) as w}
					<button
						class="chip {words[w]
							? 'bg-gradient-to-br variant-gradient-secondary-primary'
							: 'variant-soft'}"
						on:click={() => {
							toggleWordsHandler(w);
						}}
						on:keypress
					>
						{#if words[w]}<Check size="16"></Check>{/if}
						<span class="capitalize">{w}</span>
					</button>
				{/each}
			</div>
		</article>

		<h6 class="h6 my-1.5 mt-5" data-toc-ignore="">
			Please choose the department for {#if personName}
				<span class="chip variant-filled">{personName}</span>
			{/if}.
		</h6>
		<div class="flex gap-1 mb-6">
			{#if $departments}
				{#each $departments as d}
					<button
						class="chip {selected_dep == d.id
							? 'bg-gradient-to-br variant-gradient-secondary-primary'
							: 'variant-soft'}"
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
					class="chip {selected_lang === lang
						? 'bg-gradient-to-br variant-gradient-tertiary-secondary'
						: 'variant-soft'}"
					on:click={() => {
						toggleLanguageHandler(lang);
					}}
					on:keypress
				>
					{#if selected_lang === lang}
						<Check size="16"></Check>
					{/if}
					<span>{langMap[lang]}</span>
				</button>
			{/each}
		</div>

		<footer class="card-footer text-end">
			<button
				type="button"
				on:click={saveScanHsitoryHandler}
				class="btn bg-gradient-to-br variant-gradient-tertiary-secondary"
			>
				<span><Save /></span>
				<span>Save</span>
			</button>
		</footer>
	</div>
</div>
