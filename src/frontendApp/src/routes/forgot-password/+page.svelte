<script lang="ts">
	import { applyAction, enhance } from '$app/forms';
    import type { ActionData  } from './$types';
    

	export let form: ActionData;
	$: error = form?.error;
	$: formatError = (key: string) => (error?.code == key ? error.message : '');
</script>
<div class="py-10 p-0 mx-auto max-w-md">
<header class="text-center py-4">
	<div class="text-center mb-2 text-3xl font-bold">Forgot your password?</div>
	<p class="unstyled text-sm md:text-base opacity-50">Enter your email to get a reset link</p>
</header>

<div class="card p-6 space-y-6 text-left shadow-xl">
	<form class="space-y-4"  method="POST" use:enhance={() => {
		return async ({ result }) => {
		  await applyAction(result)
		}
	  }} >
	<label class="label">
		<span>Email</span>
		<input type="email" name="email" placeholder="your-email@example.com" class="input" />
		<small class="text-error-500">{formatError('email')}</small>
	</label>
	<button type="submit" class="btn variant-filled-primary w-full">Reset Password</button>
	</form>
</div>
</div>