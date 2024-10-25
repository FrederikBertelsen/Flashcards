import { redirect, type ServerLoadEvent } from '@sveltejs/kit';
import type { Cookies } from '@sveltejs/kit';
import type { Lucia, Session, User } from 'lucia';
import DatabaseClient from '$lib/server/DatabaseClient';
import { lucia } from '$lib/server/auth';
import { ROUTES } from '$lib/Routes';

export const GOOGLE_OAUTH_STATE_COOKIE_NAME = 'googleOathState';
export const GOOGLE_OAUTH_CODE_VERIFIER_COOKIE_NAME = 'googleOauthCodeVerifier';

export const NO_AUTH_ROUTES = [ROUTES.LOGIN, ROUTES.GOOGLE_OAUTH];
export const ONLY_AUTH_ROUTES = [ROUTES.CREATE_DECK, ROUTES.LOGOUT];
export const ONLY_ADMIN_ROUTES = [ROUTES.ADMIN];

export const handleAuthorizationRedirect = async (event: ServerLoadEvent) => {
	await processAuthorizationRedirect(event.url.pathname, event.locals.user, event.locals.session);
};

export const processAuthorizationRedirect = async (pathname: string, user: User | null, session: Session | null) => {
	const isAuthenticated = user != null && session != null;

	if (ONLY_AUTH_ROUTES.includes(pathname) && !isAuthenticated) {
		return redirect(302, ROUTES.LOGIN);
	}

	if (NO_AUTH_ROUTES.includes(pathname) && isAuthenticated) {
		return redirect(302, ROUTES.ROOT);
	}

	if (ONLY_ADMIN_ROUTES.includes(pathname) && !(await verifyAdminPrivileges(user, session))) {
		return redirect(302, ROUTES.ROOT);
	}
};

export async function verifyAdminPrivileges(user: User | null, session: Session | null): Promise<boolean> {
	if (!user || !session) {
		return false;
	}

	return await DatabaseClient.IsUserAdmin(user.id, user.googleId, session.id);
}

export function checkUserAuthentication(locals: App.Locals): Response | undefined {
	if (!locals.user || !locals.session) {
		return new Response(JSON.stringify({ error: 'Unauthorized' }), { status: 401 });
	}
}

export const createAndSetSession = async (lucia: Lucia, userId: string, cookies: Cookies) => {
	const session = await lucia.createSession(userId, {});
	const sessionCookie = lucia.createSessionCookie(session.id);

	cookies.set(sessionCookie.name, sessionCookie.value, {
		path: '.',
		...sessionCookie.attributes
	});
};

export const deleteSessionCookie = async (lucia: Lucia, cookies: Cookies) => {
	const sessionCookie = lucia.createBlankSessionCookie();

	cookies.set(sessionCookie.name, sessionCookie.value, {
		path: '.',
		...sessionCookie.attributes
	});
};
