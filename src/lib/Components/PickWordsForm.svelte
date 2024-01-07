<script lang="ts">
	import type { SvelteComponent } from 'svelte';
	import { applyAction, enhance } from '$app/forms';
	import { invalidateAll } from '$app/navigation';
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
	let form: HTMLFormElement;
	let record: ScanHistory = $modalStore[0].meta.record;
	let selected_dep: string = '';
	let selected_lang: string = record && record.lang ? record.lang : 'eng';
	let selected_words: string[] = [];
	let personName: string = '';
	let words = record && record.words ? parseWords(record.words) : {};
	let personnameinvaildmessage: string = '';
	let departmentinvaildmessage: string = '';
	let saving: boolean;
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
			if (selected_words.length > 5) {
				const removed = selected_words.shift();
				if (removed) words[removed] = false;
			}
		} else {
			selected_words = selected_words.filter((x) => x != w);
		}
		if (selected_words) {
			record.person_name = selected_words.join(' ');
			personName = selected_words.join(' ');
		} else {
			record.person_name = '';
			personName = '';
		}
		vaildateForm();
	}
	function vaildateForm(): boolean {
		console.log(record.person_name);
		if (record.department == '') {
			departmentinvaildmessage = `Please allocate the departments accordingly`;
		} else {
			departmentinvaildmessage = '';
		}
		if (record.person_name == '') {
			personnameinvaildmessage = `Please pick out the person's name from the following words.`;
		} else {
			personnameinvaildmessage = '';
		}
		if (personnameinvaildmessage != '' || departmentinvaildmessage != '') {
			return false;
		} else {
			return true;
		}
	}
	function toggleDepartmentHandler(dep: Department): void {
		selected_dep = dep.id;
		record.department = dep.id;
		vaildateForm();
	}
	function toggleLanguageHandler(lang: string): void {
		selected_lang = lang;
		record.lang = lang;
	}
	async function saveScanHsitoryHandler() {
		if (vaildateForm()) {
			saving = true;
			console.log('submit');
			form.requestSubmit();
		}
	}
	async function handleSubmit(event: SubmitEvent & { currentTarget: HTMLFormElement }) {
		event.preventDefault();
	}
	function showToasts() {
		const toast: ToastSettings = {
			message: 'Save successfully, Please try it again.',
			timeout: 2000,
			autohide: true,
			background: 'variant-glass-success'
		};
		toastStore.trigger(toast);
	}
	let langMap: Record<string, string> = {
		en: 'English',
		de: 'Deutsch',
		zh: '中文'
	};
</script>

<div class="t-10 my-20">
	<div class="card variant-glass-surface relative">
		<header class="card-header flex justify-end relative p-4">
			<button
				class="btn-icon hover:variant-soft-primary absolute top-0 right-0"
				on:click={parent.onClose}><XCircle /></button
			>
		</header>
		<form
			bind:this={form}
			action="?/addScanHistory"
			method="POST"
			on:submit|preventDefault={handleSubmit}
			use:enhance={() => {
				return async ({ result }) => {
					console.log(result);
					if (result.type === 'success') {
						await applyAction(result);
						parent.onClose();
						showToasts();
					}
					saving = false;
				};
			}}
		>
			<input type="hidden" name="original_text" bind:value={record.original_text} />
			<input type="hidden" name="elapsed_time" bind:value={record.elapsed_time} />
			<input type="hidden" name="department" bind:value={record.department} />
			<input type="hidden" name="lang" bind:value={record.lang} />
			<input type="hidden" name="owner" bind:value={record.owner} />
			<input type="hidden" name="person_name" bind:value={record.person_name} />
			<input type="hidden" name="words" bind:value={record.words} />
			<input type="hidden" name="trained" bind:value={record.trained} />
			<section class="p-4">
				<h6 class="h6 my-1.5" data-toc-ignore="">
					<span class="text-error-500">*</span> Please select the words from the list below that are
					names of people.
				</h6>
				<article class="gap-1 overflow-y-auto focus:overscroll-contain overscroll-auto max-h-28">
					<div class="flex gap-1 flex-wrap">
						{#each Object.keys(words) as w}
							<button
								type="button"
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
					<small class="text-error-500">{personnameinvaildmessage}</small>
				</article>

				<h6 class="h6 my-1.5 mt-5" data-toc-ignore="">
					<span class="text-error-500">*</span> Please choose the department for {#if personName}
						<span class="chip variant-filled">{personName}</span>
					{/if}.
				</h6>
				<article class="gap-1 overflow-y-auto focus:overscroll-contain overscroll-auto max-h-28">
					{#if $departments}
						<div class="flex gap-1 flex-wrap">
							{#each $departments as d}
								<button
									type="button"
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
						</div>
						<small class="text-error-500">{departmentinvaildmessage}</small>
					{/if}
				</article>
				<h6 class="h6 my-1.5 mt-5" data-toc-ignore="">Please choose the language.</h6>
				<div class="flex gap-1 mb-6">
					{#each Object.keys(langMap) as lang}
						<button
							type="button"
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
			</section>
		</form>
		<footer class="card-footer text-end">
			<button
				type="button"
				disabled={saving}
				on:click={saveScanHsitoryHandler}
				class="btn bg-gradient-to-br variant-gradient-tertiary-secondary"
			>
				<span><Save /></span>
				<span>Save</span>
			</button>
		</footer>
	</div>
</div>
