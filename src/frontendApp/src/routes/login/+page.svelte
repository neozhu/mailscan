<script lang="ts">
    import type { PageData } from './$types';
    
    export let data: PageData;
</script>
<div class="py-10 p-0 mx-auto max-w-md">
<header class="text-center py-4">
	<div class="text-center mb-2 text-3xl font-bold">Welcome back!</div>
	<p class="unstyled text-sm md:text-base opacity-50">
		Don't have an account yet? <a href="/register">Create account</a>
	</p>
</header>
<div class="card p-6 space-y-6 shadow-xl text-lef">
	<form class="space-y-4">
		<label class="label">
			<span>Email</span>
			<input type="email" placeholder="your-email@example.com" class="input" />
		</label>
		<label class="label">
			<span>Password</span>
			<input type="password" placeholder="Your password" class="input" />
		</label>
        <div class="flex justify-between flex-wrap flex-col">
            <p class="text-sm unstyled py-2 text-slate-500">
                Forgot password? <a href="/resetpassword">Reset Password</a>
            </p>
            <button class="btn variant-filled-primary">Login</button>
        </div>
	</form>
	
</div>
</div>