<script lang="ts">
	import type { PageData  } from './$types';
	import { departments } from '$lib/stores/department';
	import Camera from '$lib/Components/Camera.svelte';
	export let data:PageData;
	departments.set(data.departments)
</script>

<Camera />


 
