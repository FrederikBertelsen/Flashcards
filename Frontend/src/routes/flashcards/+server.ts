import type { RequestHandler } from '@sveltejs/kit';
import { json } from '@sveltejs/kit';
import DatabaseClient from '$lib/server/DatabaseClient';
import type { Flashcard } from '$lib/models/flashcard';
import { checkUserAuthentication } from '$lib/server/authUtils';

export const POST: RequestHandler = async ({ request, locals }) => {
	const notAuthorizedResponse = checkUserAuthentication(locals);
	if (notAuthorizedResponse) return notAuthorizedResponse;

	let createdFlashcard = (await request.json()) as Flashcard;

	await DatabaseClient.CreateFlashcard(createdFlashcard.deckId, createdFlashcard.flashType, createdFlashcard.front, createdFlashcard.back, locals.session?.id);

	return new Response(null, { status: 204 }); // No Content
};
