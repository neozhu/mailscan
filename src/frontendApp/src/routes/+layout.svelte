<script lang="ts">
	import '../app.postcss';
	import { applyAction, enhance } from '$app/forms';
	import { invalidateAll,goto } from '$app/navigation';
	import { initializeStores, Modal, Toast } from '@skeletonlabs/skeleton';
	initializeStores();
	// Floating UI for Popups
	import { computePosition, autoUpdate, flip, shift, offset, arrow } from '@floating-ui/dom';
	import type { LayoutData } from './$types';
	import { storePopup, AppBar, getModalStore } from '@skeletonlabs/skeleton';
	import type { ModalSettings, ModalComponent, ModalStore } from '@skeletonlabs/skeleton';

	import { UserCircle, Menu, LogIn, Activity, Airplay } from 'svelte-lucide';
	export let data: LayoutData;
	$: user = data.user;

	storePopup.set({ computePosition, autoUpdate, flip, shift, offset, arrow });

	const modalStore = getModalStore();

	const handleLogout = () => {
		const modal: ModalSettings = {
			type: 'confirm',
			// Data
			title: 'Please Confirm',
			body: 'Are you sure want to log out?',
			// TRUE if confirm pressed, FALSE if cancel pressed
			response: async (r: boolean) => {
				if (r) {
					const { status } = await fetch('/logout', {
						method: 'POST',
						headers: {
							'Content-Type': 'application/x-www-form-urlencoded'
						}
					});
					 
					if (status==200) {
						await invalidateAll();
						goto('/login')
					}
				}
			}
		};
		modalStore.trigger(modal);
	};
</script>

<AppBar class="h-16  space-y-4 p-4 shadow-2xl">
	<svelte:fragment slot="lead">
		<Menu class="h-6 w-6 text-blue-500" />
	</svelte:fragment>

	Mail Scan

	<svelte:fragment slot="trail">
		{#if user}
			<button type="button" class="btn-icon bg-initial" on:click={handleLogout}>
				<UserCircle /></button
			>
		{:else}
			<a type="button" class="btn bg-initial m-0" href="/login">
				<LogIn />
			</a>
		{/if}
	</svelte:fragment>
</AppBar>
<main class="container h-full flex justify-center items-center max-w-7xl mx-auto p-0">
	<slot />
</main>
<Modal />
<Toast />
