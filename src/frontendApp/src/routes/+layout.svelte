<script lang="ts">
	import '../app.postcss';
	import { initializeStores,Modal } from '@skeletonlabs/skeleton';
	initializeStores();
	// Floating UI for Popups
	import { computePosition, autoUpdate, flip, shift, offset, arrow } from '@floating-ui/dom';
	import { storePopup, AppBar } from '@skeletonlabs/skeleton';
	import {Icon, Bars3, Bell, MagnifyingGlass } from 'svelte-hero-icons';



	storePopup.set({ computePosition, autoUpdate, flip, shift, offset, arrow });
</script>

<AppBar class="h-16  space-y-4 p-4 shadow-2xl">
	<svelte:fragment slot="lead">
		<Icon src="{Bars3}" class="h-6 w-6 text-blue-500" />
	</svelte:fragment>

	Mail Scan

	<svelte:fragment slot="trail">
		<Icon src="{Bell}"  class="h-6 w-6 text-blue-500" />
		<Icon src="{MagnifyingGlass}"  class="h-6 w-6 text-blue-500" />
	</svelte:fragment>
</AppBar>
<main class="container h-full flex justify-center items-center max-w-7xl mx-auto p-0">
	<slot />
</main>
<Modal />
