import { redirect,fail } from '@sveltejs/kit';
import type { Actions} from './$types';
import type { PageServerLoad } from './$types';
import { HttpStatusCode } from '$lib/statusCodes';

export const load = (async ({ locals }) => {
	// if user is logged in, send them back to home
	if (locals.pb.authStore.isValid) throw redirect(HttpStatusCode.SEE_OTHER, '/');
	//  return { user: structuredClone(locals.pb.authStore.model) };
}) satisfies PageServerLoad;

interface FormResError {
	code: FormFieldKey | 'unknown';
	message: string;
}
enum FormFieldKey {
	Email = 'email',
}
const makeErrObj = (message = '', code: FormFieldKey | 'unknown' = 'unknown'): FormResError => ({
	code,
	message
});
export const actions: Actions = {
	default: async ({ locals, request }) => {
		const data = Object.fromEntries(await request.formData()) as {
			email: string;
		};
		// validate emailOrUsername
		if (!data.email || data.email == '')
			return fail(HttpStatusCode.BAD_REQUEST, {
				email:data.email,
				error: makeErrObj('Empty email address', FormFieldKey.Email)
			});
		
		try {
			await locals.pb.collection('users').requestPasswordReset(data.email);
		} catch (e) {
			console.error(e);
			return fail(HttpStatusCode.BAD_REQUEST, {
				email:data.email,
				error: makeErrObj('Incorrect credentials!', 'unknown')
			});
		}

		throw redirect(303, '/login');
	}
};
