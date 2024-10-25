import type { PageServerLoad } from './$types';
import { handleAuthorizationRedirect } from '$lib/server/authUtils';

export const load: PageServerLoad = async (event) => {
	await handleAuthorizationRedirect(event);

	if (event.locals.user) {
		return { user: event.locals.user };
	}
};
