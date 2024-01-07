import type { LayoutServerLoad } from './$types';
export const load = (async ({ locals,request }) => {
	let acceptLanguageHeader = request.headers.get('accept-language');
	if(acceptLanguageHeader==null){
		acceptLanguageHeader='en-US,en;';
	}
	return {
		acceptLanguageHeader:acceptLanguageHeader,
		user: locals.user
	};
}) satisfies LayoutServerLoad;