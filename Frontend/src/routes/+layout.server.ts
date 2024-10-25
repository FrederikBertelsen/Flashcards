import type { LayoutServerLoad } from './$types';

export const load: LayoutServerLoad = async (event) => {
	// await handleAuthorizationRedirect(event);

	return { user: event.locals.user };
};
