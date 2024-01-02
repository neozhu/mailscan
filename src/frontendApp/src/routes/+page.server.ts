import type { PageServerLoad, Actions } from './$types';
 
export const load = (async () => {
    return {};
}) satisfies PageServerLoad;

/** @type {import('./$types').Actions} */
export const actions:Actions  = {
	process: async ({ request  }) => {
		const data = await request.formData();
		console.log(data.get('image'));
		return { success: true };
	},
	 
};