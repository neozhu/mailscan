<script lang="ts">
	import { onMount, onDestroy } from 'svelte';
	import type { SvelteComponent } from 'svelte';
	import { XCircle, ArrowRight, MailSearch, Trash2 } from 'svelte-lucide';
	import {
		getModalStore,
		getToastStore,
		type ToastSettings,
		type ModalStore,
		type ModalSettings,
		type ToastStore
	} from '@skeletonlabs/skeleton';
	import type { NlpEntity } from '$lib/type';

	const modalStore: ModalStore = getModalStore();
	const toastStore: ToastStore = getToastStore();
	export let parent: SvelteComponent;
	let records: NlpEntity[] = $modalStore[0].meta.entities;
	async function removeEntityHandler(person: NlpEntity) {
		if (intervalId) {
			clearInterval(intervalId);
		}
		if (confirm('Are you sure you want to delete?') == true) {
			await removeKeywords(person);
		}
	}
	async function removeKeywords(person: NlpEntity) {
		const formData = new FormData();
		formData.append('entity', person.entity);
		formData.append('option', person.option);
		formData.append('sourceText', person.sourceText);
		const response = await fetch('?/removeKeywords', {
			method: 'POST',
			body: formData
		});
		if (response.ok) {
			records = records.filter((x) => x.sourceText != person.sourceText);
		} else {
			console.error('Error removing keywords');
		}
	}
	let count = 4;
	let intervalId: ReturnType<typeof setInterval> | null = null;
	onMount(() => {
		intervalId = setInterval(() => {
			if (count > 0) {
				count -= 1;
			} else if (intervalId) {
				clearInterval(intervalId);
				parent.onClose();
			}
		}, 1000);
	});
	onDestroy(() => {
		if (intervalId) {
			clearInterval(intervalId);
		}
	});
</script>

<div class="w-modal-wide t-10 my-20">
	<div class="card variant-glass-surface relative">
		<header class="card-header flex justify-end relative p-4">
			<button
				class="btn-icon hover:variant-soft-primary absolute top-0 right-0"
				on:click={parent.onClose}><XCircle /></button
			>
		</header>

		<section class="p-4">
			<h6 class="h6 my-1.5" data-toc-ignore="">Below is the recognized information:</h6>
			<article class="gap-1 overflow-y-auto focus:overscroll-contain overscroll-auto max-h-40">
				<dl class="list-dl">
					{#each records as person}
						<div>
							<span class="p-4"><MailSearch /></span>
							<span class="flex-auto">
								<dt class="font-bold">{person.option}</dt>
								<dd class="text-sm opacity-50">keywords: {person.sourceText}</dd>
							</span>
							<button
								type="button"
								on:click={() => {
									removeEntityHandler(person);
								}}
								class="btn-icon btn-sm hover:variant-soft-primary variant-soft-surface"
								><Trash2 /></button
							>
						</div>
					{/each}
				</dl>
			</article>
		</section>

		<footer class="card-footer text-end">
			<button
				type="button"
				on:click={parent.onClose}
				class="btn bg-gradient-to-br variant-gradient-tertiary-secondary"
			>
				<span><ArrowRight /></span>
				<span>Next</span>
				<span class="badge-icon variant-filled">{count}</span>
			</button>
		</footer>
	</div>
</div>
