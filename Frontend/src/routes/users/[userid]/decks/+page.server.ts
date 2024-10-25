import DatabaseClient from '$lib/server/DatabaseClient';
import { handleAuthorizationRedirect } from '$lib/server/authUtils';
import { error } from '@sveltejs/kit';
import type { PageServerLoad } from './$types';

export const load: PageServerLoad = async (event) => {
	await handleAuthorizationRedirect(event);

	const userId = event.params.userid;
	let user = await DatabaseClient.GetUserById(userId);

	if (!user) {
		error(404, {
			message: 'User not found!'
		});
	}

	const sessionId = event.locals.session?.id;
	let deckList = await DatabaseClient.GetDecksByUserId(userId, sessionId);

	const isCurrentUser = event.locals.user?.id === user.id;

	return { deckList, user, isCurrentUser };
};
