<script lang="ts">
	import '../app.postcss';
	import { invalidateAll,goto } from '$app/navigation';
	import { initializeStores, Modal, popup, Toast } from '@skeletonlabs/skeleton';
	initializeStores();
	// Floating UI for Popups
	import { computePosition, autoUpdate, flip, shift, offset, arrow } from '@floating-ui/dom';
	import type { LayoutData } from './$types';
	import { storePopup, AppBar, getModalStore } from '@skeletonlabs/skeleton';
	import type { ModalSettings, ModalComponent, ModalStore } from '@skeletonlabs/skeleton';

	import { UserCircle, Menu, LogIn, Github, Languages   } from 'svelte-lucide';
	import {supportLanguages} from '$lib/constant'
	import{defaultLanguage} from '$lib/stores/language'
	import PickWordsForm from '$lib/Components/PickWordsForm.svelte';
	import ResultForm from '$lib/Components/ResultForm.svelte';

	export let data: LayoutData;
	$: user = data.user;
    defaultLanguage.update((v)=>v = data.acceptLanguageHeader.split(',')[0].split(';')[0])
	storePopup.set({ computePosition, autoUpdate, flip, shift, offset, arrow });
	const modalStore:ModalStore = getModalStore();
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

	const modalComponentRegistry: Record<string, ModalComponent> = {
		pickWordsForm: { ref: PickWordsForm },
		resultForm: { ref: ResultForm },
	};
</script>

<AppBar background="variant-glass-surface" class="h-20 fixed  top-0 w-full space-y-4 p-4 shadow-2xl z-50">
	<svelte:fragment slot="lead">
		<Menu class="h-6 w-6 text-blue-500" />
	</svelte:fragment>

	<h4 class="h4">
		<span class="bg-gradient-to-br from-blue-500 to-cyan-300 bg-clip-text text-transparent box-decoration-clone">Mail Scan Kit</span>
	</h4>

	<svelte:fragment slot="trail">
		<a class="btn-icon hover:variant-soft-primary" href="https://github.com/neozhu/mailscan" target="_blank" rel="noreferrer">
			<Github/>
		</a>

		<!-- Set Language -->
		<div>
			<!-- trigger -->
			<button class="btn hover:variant-soft-primary" use:popup={{ event: 'click', target: 'sponsor' }}>
				<span><Languages size="24"></Languages></span>
				<span class="hidden md:inline-block">{supportLanguages[$defaultLanguage]}</span>
			</button>
			<!-- popup -->
			<div class="card variant-glass-surfac p-4  shadow-xl" data-popup="sponsor">
				<div class="space-y-4">
					<nav class="list-nav ">
						<ul>
							{#each Object.keys(supportLanguages) as lang}
							<li>
								<button type="button" on:click={()=>defaultLanguage.update((n)=>n=lang)}>
									<span>{supportLanguages[lang]}</span>
								</button>
							</li>
							{/each}
						</ul>
					</nav>
					 
				</div>
			</div>
		</div>

		{#if user}
			<button type="button" class="btn-icon hover:variant-soft-primary" on:click={handleLogout}>
				<UserCircle /></button
			>
		{:else}
			<a type="button" class="btn-icon hover:variant-soft-primary" href="/login">
				<LogIn />
			</a>
		{/if}
	</svelte:fragment>
</AppBar>
<main class="container h-full flex justify-center items-center max-w-7xl mx-auto p-0">
	<slot />
</main>
<Modal background="variant-glass-secondary" components={modalComponentRegistry} zIndex="z-[888]"/>
<Toast position="t" background="variant-glass-secondary" zIndex="z-[999]"/>
