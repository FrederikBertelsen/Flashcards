import DatabaseClient from '$lib/server/DatabaseClient';
import type { PageServerLoad } from './$types';
import { handleAuthorizationRedirect } from '$lib/server/authUtils';

export const load: PageServerLoad = async (event) => {
	await handleAuthorizationRedirect(event);

	const deckList = await DatabaseClient.GetDecks();
	return { deckList };
};
