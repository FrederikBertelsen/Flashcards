import { type Handle } from '@sveltejs/kit';
import { lucia } from '$lib/server/auth';
import { deleteSessionCookie, handleAuthorizationRedirect, processAuthorizationRedirect } from '$lib/server/authUtils';

export const handle: Handle = async ({ event, resolve }) => {
	// Retrieve the session ID from the browser's cookies
	const sessionId = event.cookies.get(lucia.sessionCookieName);

	// If there's no session ID, set both user and session to null and resolve the request
	if (!sessionId) {
		event.locals.user = null;
		event.locals.session = null;
		return resolve(event);
	}

	// Attempt to validate the session using the retrieved session ID
	const { session, user } = await lucia.validateSession(sessionId);

	// If the session is newly created (due to session expiration extension), generate a new session cookie
	if (session?.fresh) {
		const sessionCookie = lucia.createSessionCookie(session.id);

		// Set the new session cookie in the browser
		event.cookies.set(sessionCookie.name, sessionCookie.value, {
			path: '.',
			...sessionCookie.attributes
		});
	}

	// If the session is invalid, generate a blank session cookie to remove the existing session cookie from the browser
	if (!session) {
		await deleteSessionCookie(lucia, event.cookies);
	}

	// // If a user is logged in and attempts to access the login or register page, redirect them to the dashboard
	// if (session && NO_AUTH_ROUTES.includes(event.url.pathname)) {
	// 	throw redirect(303, ROOT_ROUTE);
	// }
	//
	// if (!session && ONLY_AUTH_ROUTES.includes(event.url.pathname)) {
	// 	throw redirect(303, LOGIN_ROUTE);
	// }
	await processAuthorizationRedirect(event.url.pathname, user, session);

	// Persist the user and session information in the event locals for use within endpoint handlers and page components
	event.locals.user = user;
	event.locals.session = session;

	return resolve(event);
};

// import { lucia } from '$lib/server/auth';
// import { type Handle, redirect } from '@sveltejs/kit';
// import { LOGIN_ROUTE, NO_AUTH_ROUTES, ONLY_AUTH_ROUTES, PROFILE_ROUTE, ROOT_ROUTE } from '$lib/server/authUtils';
//
// export const handle: Handle = async ({ event, resolve }) => {
// 	const sessionId = event.cookies.get(lucia.sessionCookieName);
// 	if (!sessionId) {
// 		event.locals.user = null;
// 		event.locals.session = null;
// 		return resolve(event);
// 	}
//
// 	const { session, user } = await lucia.validateSession(sessionId);
// 	if (session && session.fresh) {
// 		const sessionCookie = lucia.createSessionCookie(session.id);
// 		// sveltekit types deviates from the de-facto standard
// 		// you can use 'as any' too
// 		event.cookies.set(sessionCookie.name, sessionCookie.value, {
// 			path: '.',
// 			...sessionCookie.attributes
// 		});
// 	} else if (!session) {
// 		const sessionCookie = lucia.createBlankSessionCookie();
// 		event.cookies.set(sessionCookie.name, sessionCookie.value, {
// 			path: '.',
// 			...sessionCookie.attributes
// 		});
// 	}
//
// 	if (session && NO_AUTH_ROUTES.includes(event.url.pathname)) {
// 		throw redirect(303, ROOT_ROUTE);
// 	}
// 	if (!session && ONLY_AUTH_ROUTES.includes(event.url.pathname)) {
// 		throw redirect(303, LOGIN_ROUTE);
// 	}
//
// 	event.locals.user = user;
// 	event.locals.session = session;
// 	return resolve(event);
// };
