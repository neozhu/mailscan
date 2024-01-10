<script lang="ts">
	import type { SvelteComponent } from 'svelte';
	import { departments } from '$lib/stores/department';
	import type { Department } from '$lib/type';
	import { X, XCircle, Plus } from 'svelte-lucide';
	import { enhance, applyAction } from '$app/forms';
	export let parent: SvelteComponent;
	let records: Department[] = $departments ?? [];
	let item: Department = { id: '', name: '', address: '' };
	let form: HTMLFormElement;
	let namerequired: string = '';
	let descrequired: string = '';
	let saving:boolean=false;
	async function handleRemoveItem(item: Department) {
		if (confirm(`Are you sure you want to delete ${item.name}?`) == true) {
			var formData = new FormData();
			formData.append('id', item.id);
			const response = await fetch('?/deleteClassification', {
				method: 'POST',
				body: formData
			});
			if (response.ok) {
				records = records.filter((i) => i.id != item.id);
				departments.set(records);
			}
		}
	}
	function handleCreateItem() {
		if (validation()) {
			saving=true;
			form.requestSubmit();
		}
	}
	function validation(): boolean {
		if (item.name) {
			namerequired = '';
		} else {
			namerequired = 'name required.';
		}
		if (item.address) {
			descrequired = '';
		} else {
			descrequired = 'description requried';
		}
		if (item.name && item.address) {
			return true;
		} else {
			return false;
		}
	}
	function handleCreatedResult(dep: Department) {
		records.unshift(dep);
		records=records;
		departments.set(records);
		item.name='';
		item.address='';
		
	}
</script>

<div class="w-modal-wide t-10 my-20">
	<div class="card variant-glass-surface relative">
		<header class="card-header flex between relative p-4">
			<h3 class="h3 text-left">Setting Classification</h3>
			<button
				class="btn-icon hover:variant-soft-primary absolute top-0 right-0"
				on:click={parent.onClose}><XCircle /></button
			>
		</header>

		<section class="p-4">
			<h6 class="h6 my-1.5" data-toc-ignore="">Create a classification:</h6>

			<form
				method="post"
				action="?/createClassification"
				use:enhance={() => {
					return async ({ result }) => {
						saving=false
						if (result.type === 'success') {
							if(result.data && result.data.data){
								handleCreatedResult(result.data.data);
							}
							await applyAction(result);
						}
					};
				}}
				bind:this={form}
			>
				<div class="flex">
					<div class="m-0.5">
						<input
							type="text"
							name="name"
							placeholder="name"
							class="input variant-form-material"
							required
							bind:value={item.name}
						/>
						<div class="text-error-500">{namerequired}</div>
					</div>
					<div class="m-0.5">
						<input
							name="address"
							type="text"
							placeholder="description"
							required
							class="input variant-form-material"
							bind:value={item.address}
						/>
						<div class="text-error-500">{descrequired}</div>
					</div>
					<button
						type="button"
						disabled={saving}
						class="btn-icon hover:variant-soft-primary m-1 h-10 w-10"
						on:click={handleCreateItem}
					>
						<Plus /></button
					>
				</div>
			</form>

			<h6 class="h6 my-1.5" data-toc-ignore="">Classification:</h6>
			<div class="overflow-y-auto focus:overscroll-contain overscroll-auto max-h-56">
				{#each records as item}
					<button
						class="chip variant-soft hover:variant-filled mr-1 mb-1"
						on:click={() => handleRemoveItem(item)}
					>
						<span>{item.name} | {item.address}</span>
						<span><X size="16" /></span>
					</button>
				{/each}
			</div>
		</section>
	</div>
</div>
