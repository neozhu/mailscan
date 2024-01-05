<script lang="ts">
	import type { PageData  } from './$types';
	import { departments } from '$lib/pocketbase';
	import Camera from '$lib/components/Camera.svelte';
	import PickWordsForm from '$lib/Components/PickWordsForm.svelte'
	export let data:PageData;
	departments.set(data.departments)
</script>

<Camera />


 
