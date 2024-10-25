import { redirect, type RequestHandler } from '@sveltejs/kit';
import { generateCodeVerifier, generateState } from 'arctic';
import { googleOauth } from '$lib/server/auth';

import type { RequestEvent } from '@sveltejs/kit';
import { GOOGLE_OAUTH_CODE_VERIFIER_COOKIE_NAME, GOOGLE_OAUTH_STATE_COOKIE_NAME } from '$lib/server/authUtils';

export const GET: RequestHandler = async ({ cookies }) => {
	const state = generateState();
	const codeVerifier = generateCodeVerifier();

	const url = await googleOauth.createAuthorizationURL(state, codeVerifier, {
		scopes: ['profile']
	});

	cookies.set(GOOGLE_OAUTH_STATE_COOKIE_NAME, state, {
		path: '/',
		secure: import.meta.env.PROD,
		httpOnly: true,
		maxAge: 60 * 10, // 10 min
		sameSite: 'lax'
	});
	cookies.set(GOOGLE_OAUTH_CODE_VERIFIER_COOKIE_NAME, codeVerifier, {
		path: '/',
		secure: import.meta.env.PROD,
		httpOnly: true,
		maxAge: 60 * 10, // 10 min
		sameSite: 'lax'
	});

	redirect(302, url.toString());
};
