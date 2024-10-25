import type { RequestHandler } from '@sveltejs/kit';
import { json } from '@sveltejs/kit';
import DatabaseClient from '$lib/server/DatabaseClient';
import { checkUserAuthentication } from '$lib/server/authUtils';

export const POST: RequestHandler = async ({ request, locals }) => {
	const notAuthorizedResponse = checkUserAuthentication(locals);
	if (notAuthorizedResponse) return notAuthorizedResponse;

	const { quizletDeckId } = await request.json();
	const deck = await DatabaseClient.ImportQuizletDeck(quizletDeckId, locals.session?.id);

	return json({ deck });
};
