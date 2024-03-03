import adapter from '@sveltejs/adapter-node';
import { vitePreprocess } from '@sveltejs/vite-plugin-svelte';
import path from 'path'
/** @type {import('@sveltejs/kit').Config} */
const config = {
	extensions: ['.svelte'],
	// Consult https://kit.svelte.dev/docs/integrations#preprocessors
	// for more information about preprocessors
	preprocess: [vitePreprocess()],
	server: {
		maxRequestBodySize: '5mb',  // 增加到 5MB
	  },
	kit: {
		// adapter-auto only supports some environments, see https://kit.svelte.dev/docs/adapter-auto for a list.
		// If your environment is not supported or you settled on a specific environment, switch out the adapter.
		// See https://kit.svelte.dev/docs/adapters for more information about adapters.
		adapter: adapter(),
		target: '#svelte',
		vite:{
			resolve:{
				alias:{
					'$components': path.resolve('./src/lib/Components'),
					'$stores': path.resolve('./src/lib/stores')
				}
			}
		}
	}
};
export default config;