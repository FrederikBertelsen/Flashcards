import { json, type RequestHandler } from '@sveltejs/kit';
import DatabaseClient from '$lib/server/DatabaseClient';
import { checkUserAuthentication } from '$lib/server/authUtils';
import type { UpdateDeck } from '$lib/models/updateDeck';

export const PUT: RequestHandler = async ({ request, locals }) => {
	const notAuthorizedResponse = checkUserAuthentication(locals);
	if (notAuthorizedResponse) return notAuthorizedResponse;

	let updatedDeck = (await request.json()) as UpdateDeck;

	await DatabaseClient.UpdateDeck(updatedDeck, locals.session?.id);

	return new Response(null, { status: 204 }); // No Content
};
