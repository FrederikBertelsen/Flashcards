import DatabaseClient from '$lib/server/DatabaseClient';
import type { PageServerLoad } from './$types';
import { handleAuthorizationRedirect } from '$lib/server/authUtils';
import { invalidate } from '$app/navigation';

export const load: PageServerLoad = async (event) => {
	await handleAuthorizationRedirect(event);

	// await invalidate(event.url);

	const deckListPromise = DatabaseClient.GetDecksBySearchTerm(event.params.searchterm);
	return { deckListPromise };
};
