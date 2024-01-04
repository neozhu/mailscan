// See https://kit.svelte.dev/docs/types#app
// for information about these interfaces


declare namespace App {

	interface Locals {
		pb: import('pocketbase').default
		user: import('pocketbase').default['authStore']['model']
	  }
	// interface PageData {}
	// interface Error {}
	// interface Platform {}
}
