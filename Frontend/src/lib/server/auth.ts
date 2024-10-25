import { Lucia } from 'lucia';
import { dev } from '$app/environment';
import DatabaseAdapter from '$lib/server/DatabaseAdapter';
import { GOOGLE_CLIENT_ID, GOOGLE_CLIENT_SECRET, GOOGLE_REDIRECT_URL } from '$env/static/private';
import { Google } from 'arctic';

export const googleOauth = new Google(GOOGLE_CLIENT_ID, GOOGLE_CLIENT_SECRET, GOOGLE_REDIRECT_URL);

const adapter = new DatabaseAdapter(); // your adapter

export const lucia = new Lucia(adapter, {
	sessionCookie: {
		attributes: {
			secure: !dev
		}
	},
	getUserAttributes: (attributes) => {
		return {
			// attributes has the type of DatabaseUserAttributes
			googleId: attributes.googleId,
			username: attributes.username,
			pictureUrl: attributes.pictureUrl
		};
	}
});

declare module 'lucia' {
	interface Register {
		Lucia: typeof lucia;
		DatabaseUserAttributes: DatabaseUserAttributes;
	}
}

interface DatabaseUserAttributes {
	googleId: string;
	username: string;
	pictureUrl: string;
}
