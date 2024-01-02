<script lang="ts">
    import type { PageData } from './$types';
    
    export let data: PageData;
</script>
<div class="py-10 p-0 mx-auto max-w-md">
<header class="text-center py-4">
	<div class="text-center mb-2 text-3xl font-bold">Forgot your password?</div>
	<p class="unstyled text-sm md:text-base opacity-50">Enter your email to get a reset link</p>
</header>

<div class="card p-6 space-y-6 text-left shadow-xl">
	<label class="label">
		<span>Email</span>
		<input type="email" placeholder="your-email@example.com" class="input" />
	</label>
	<button class="btn variant-filled-primary w-full">Reset Password</button>
</div>
</div>