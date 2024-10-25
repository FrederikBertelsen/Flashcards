import type { RequestHandler } from '@sveltejs/kit';
import DatabaseClient from '$lib/server/DatabaseClient';
import type { Flashcard } from '$lib/models/flashcard';
import { checkUserAuthentication } from '$lib/server/authUtils';

export const PUT: RequestHandler = async ({ request, locals }) => {
	const notAuthorizedResponse = checkUserAuthentication(locals);
	if (notAuthorizedResponse) return notAuthorizedResponse;

	let updatedFlashcard = (await request.json()) as Flashcard;

	await DatabaseClient.UpdateFlashcard(updatedFlashcard, locals.session?.id);

	return new Response(null, { status: 204 }); // No Content
};
