import { OAuth2RequestError } from 'arctic';
import { generateId, generateIdFromEntropySize } from 'lucia';
import { googleOauth, lucia } from '$lib/server/auth';

import type { RequestEvent, RequestHandler } from '@sveltejs/kit';
import { createAndSetSession, GOOGLE_OAUTH_CODE_VERIFIER_COOKIE_NAME, GOOGLE_OAUTH_STATE_COOKIE_NAME } from '$lib/server/authUtils';
import DatabaseClient from '$lib/server/DatabaseClient';

export const GET: RequestHandler = async (event: RequestEvent) => {
	const code = event.url.searchParams.get('code');
	const state = event.url.searchParams.get('state');

	const storedState = event.cookies.get(GOOGLE_OAUTH_STATE_COOKIE_NAME);
	const storedCodeVerifier = event.cookies.get(GOOGLE_OAUTH_CODE_VERIFIER_COOKIE_NAME);

	if (!code || !state || !storedState || !storedCodeVerifier || state !== storedState) {
		return new Response('Invalid Oauth state or code verifier', {
			status: 400
		});
	}

	try {
		const tokens = await googleOauth.validateAuthorizationCode(code, storedCodeVerifier);

		const googleUserResponse = await fetch('https://openidconnect.googleapis.com/v1/userinfo', {
			headers: {
				Authorization: `Bearer ${tokens.accessToken}`
			}
		});

		const googleUser: GoogleUser = (await googleUserResponse.json()) as GoogleUser;

		const existingUser = await DatabaseClient.GetUserByGoogleId(googleUser.sub);

		if (existingUser) {
			const session = await lucia.createSession(existingUser.id, {});
			const sessionCookie = lucia.createSessionCookie(session.id);
			event.cookies.set(sessionCookie.name, sessionCookie.value, {
				path: '.',
				...sessionCookie.attributes
			});
		} else {
			const newUser = await DatabaseClient.CreateUser(googleUser.name, googleUser.sub, googleUser.picture);

			await createAndSetSession(lucia, newUser.id, event.cookies);
		}
		return new Response(null, {
			status: 302,
			headers: {
				Location: '/'
			}
		});
	} catch (e) {
		// the specific error message depends on the provider
		if (e instanceof OAuth2RequestError) {
			// invalid code
			return new Response(null, {
				status: 400
			});
		}
		return new Response(null, {
			status: 500
		});
	}
};

interface GoogleUser {
	sub: string;
	name: string;
	given_name: string;
	family_name: string;
	picture: string;
}
