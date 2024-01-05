<script lang="ts">
 
 import { Check} from 'svelte-lucide';

	let flavors: Record<string, boolean> = {
		vanilla: true,
		chocolate: false,
		strawberry: false
	};
	function toggle(flavor: string): void {
		flavors[flavor] = !flavors[flavor];
	}
</script>
<div class="t-10 my-20">
<div class="card variant-glass-surface p-4">
	<h6 class="h6 my-1.5" data-toc-ignore="">
		Please select the words from the list below that are names of people.
	</h6>
	<div class="flex gap-1">
		{#each Object.keys(flavors) as f}
			<button
				class="chip {flavors[f] ? 'variant-filled' : 'variant-soft'}"
				on:click={() => {
					toggle(f);
				}}
				on:keypress
			>
				{#if flavors[f]}<Check size="16">(icon)</Check>{/if}
				<span class="capitalize">{f}</span>
			</button>
		{/each}
	</div>

    <h6 class="h6 my-1.5 mt-5" data-toc-ignore="">
		Having selected the person's name, please now choose the department they belong to.
	</h6>
    <div class="flex gap-1 mb-6">
		{#each Object.keys(flavors) as f}
			<button
				class="chip {flavors[f] ? 'variant-filled' : 'variant-soft'}"
				on:click={() => {
					toggle(f);
				}}
				on:keypress
			>
				{#if flavors[f]}<Check size="16">(icon)</Check>{/if}
				<span class="capitalize">{f}</span>
			</button>
		{/each}
	</div>
</div>
</div>