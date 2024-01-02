<script lang="ts">
    import type { PageData } from './$types';
    
    export let data: PageData;
</script>
<div class="py-10 p-0 mx-auto max-w-md">
    <header class="text-center py-4">
        <div class="text-center mb-2 text-3xl font-bold">Create Your Account</div>
        <p class="unstyled text-sm md:text-base opacity-50">
            Already have an account? <a href="/login">Login here</a>
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
                <input type="password" placeholder="Create a password" class="input" />
            </label>
            <label class="label">
                <span>Confirm Password</span>
                <input type="password" placeholder="Confirm your password" class="input" />
            </label>
            <!-- Additional fields can be added here if needed -->
            <div class="flex justify-between py-2 flex-wrap flex-col">
                <button class="btn variant-filled-primary">Register</button>
            </div>
        </form>
       
    </div>
    </div>