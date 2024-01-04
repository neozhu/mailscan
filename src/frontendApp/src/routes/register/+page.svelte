<script lang="ts">
    import type { ActionData } from './$types';
    import {  LOGIN_PATH } from '$lib/constant';
    import { applyAction, enhance } from '$app/forms';
	import { invalidateAll } from '$app/navigation';

    export let form: ActionData;
	$: error = form?.error;

	$: formatError = (key: string) => (error?.code == key ? error.message : '');
 
</script>
<div class="py-10 p-0 mx-auto max-w-md">
    <header class="text-center py-4">
        <div class="text-center mb-2 text-3xl font-bold">Create Your Account</div>
        <p class="unstyled text-sm md:text-base opacity-50">
            Already have an account? <a href="{LOGIN_PATH}">Login here</a>
        </p>
    </header>
    <div class="card p-6 space-y-6 shadow-xl text-lef">
        <form class="space-y-4" method="POST"
		action="?/register" use:enhance={() =>
			async ({ result }) => {
				invalidateAll();
				await applyAction(result);
			}}>
            <label class="label">
                <span>Email</span>
                <input type="email" name="email" placeholder="your-email@example.com" class="input" />
                <small class="text-error-500">{formatError('email')}</small>
            </label>
            <label class="label">
                <span>Password</span>
                <input type="password" name="password" placeholder="Create a password" class="input" />
                <small class="text-error-500">{formatError('password')}</small>
            </label>
            <label class="label">
                <span>Confirm Password</span>
                <input type="password" name="passwordConfirm" placeholder="Confirm your password" class="input" />
                <small class="text-error-500">{formatError('passwordConfirm')}</small>
            </label>

            {#if error?.code == 'unknown'}<p class="text-error-500">{error.message}</p>{/if}
            <!-- Additional fields can be added here if needed -->
            <div class="flex justify-between py-2 flex-wrap flex-col">
                <button class="btn variant-filled-primary">Register</button>
            </div>
        </form>
       
    </div>
    </div>