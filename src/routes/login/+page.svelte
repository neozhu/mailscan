<script lang="ts">
    import type { ActionData  } from './$types';
	import {  REGISTER_PATH,FORGOT_PASSWORD_PATH } from '$lib/constant';
	import { invalidateAll } from '$app/navigation';
	import { applyAction, enhance } from '$app/forms'
 	import { pb } from '$lib/pocketbase'
	import { getToastStore } from '@skeletonlabs/skeleton';
	import type { ToastSettings, ToastStore } from '@skeletonlabs/skeleton';
	const toastStore: ToastStore = getToastStore();

	export let form: ActionData;
	$: error = form?.error;
	$: formatError = (key: string) => (error?.code == key ? error.message : '');
	function showToast() {
		const t: ToastSettings = {
			message: 'Login successful! Welcome back.',
			timeout: 4000,
            background: 'variant-glass-success',
			hoverable: true
		};
		toastStore.trigger(t);
	}
</script>
<div class="py-20 p-0 mx-auto max-w-md">
<header class="text-center py-4">
	<div class="text-center mb-2 text-3xl font-bold">Welcome back!</div>
	<p class="unstyled text-sm md:text-base opacity-50">
		Don't have an account yet? <a href="{REGISTER_PATH}">Create account</a>
	</p>
</header>
<div class="card variant-glass-surface p-6 space-y-6 shadow-xl text-lef">
	<form class="space-y-4"  method="POST" use:enhance={() => {
		return async ({ result }) => {
		  pb.authStore.loadFromCookie(document.cookie)
		  await applyAction(result)
		  showToast()
		}
	  }} >
		<label class="label">
			<span>Email</span>
			<input type="email" name="email" placeholder="your-email@example.com" class="input" />
			<small class="text-error-500">{formatError('emailOrUsername')}</small>
		</label>
		<label class="label">
			<span>Password</span>
			<input type="password" name="password" placeholder="Your password" class="input" />
			<small class="text-error-500">{formatError('password')}</small>
		</label>

		{#if error?.code == 'unknown'}<p class="text-error-500">{error.message}</p>{/if}

        <div class="flex justify-between flex-wrap flex-col">
            <p class="text-sm unstyled py-2 text-slate-500">
                Forgot password? <a href="{FORGOT_PASSWORD_PATH}">Reset Password</a>
            </p>
            <button class="btn variant-filled-primary">Login</button>
        </div>
	</form>
	
</div>
</div>