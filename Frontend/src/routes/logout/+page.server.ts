import { fail, redirect } from '@sveltejs/kit';
import { lucia } from '$lib/server/auth';

import type { Actions, PageServerLoad } from './$types';
import { handleAuthorizationRedirect } from '$lib/server/authUtils';
import { ROUTES } from '$lib/Routes';
import { invalidateAll } from '$app/navigation';

export const load: PageServerLoad = async (event) => {
	await handleAuthorizationRedirect(event);

	if (event.locals.session) {
		await lucia.invalidateSession(event.locals.session.id);
		const sessionCookie = lucia.createBlankSessionCookie();
		event.cookies.set(sessionCookie.name, sessionCookie.value, {
			path: '.',
			...sessionCookie.attributes
		});
	}

	redirect(302, ROUTES.ROOT);
};
