import { json, type RequestHandler } from '@sveltejs/kit';
import DatabaseClient from '$lib/server/DatabaseClient';
import { checkUserAuthentication } from '$lib/server/authUtils';

export const PUT: RequestHandler = async ({ request, params, locals }) => {
	const notAuthorizedResponse = checkUserAuthentication(locals);
	if (notAuthorizedResponse) return notAuthorizedResponse;

	let collaboratorIds = (await request.json()) as string[];
	let deckId = params.deckid;

	if (!deckId) return new Response('Deck ID is required', { status: 400 });

	await DatabaseClient.UpdateCollaborators(deckId, collaboratorIds, locals.session?.id);

	return new Response(null, { status: 204 }); // No Content
};
