import type { PageServerLoad } from './$types';
import DatabaseClient from '$lib/server/DatabaseClient';
import { handleAuthorizationRedirect } from '$lib/server/authUtils';
import { error } from '@sveltejs/kit';
import { Database } from 'lucide-svelte';
import type { Collaborator } from '$lib/models/Collaborator';

export const load: PageServerLoad = async (event) => {
	await handleAuthorizationRedirect(event);

	const deckID = event.params.deckid;
	const sessionId = event.locals.session?.id;

	let [deck, flashcards, collaborators] = await Promise.all([DatabaseClient.GetDeckById(deckID, sessionId), DatabaseClient.GetFlashcardsByDeckId(deckID, sessionId), DatabaseClient.GetCollaborators(deckID, sessionId)]);
	if (!deck) {
		error(404, {
			message: 'Deck not found!'
		});
	}
	if (!flashcards) {
		error(404, {
			message: 'Flashcards not found!'
		});
	}
	if (!collaborators) {
		error(404, {
			message: 'Collaborators not found!'
		});
	}

	const isCreator = event.locals.user?.id === deck.creator.id;
	const isEditor = isCreator || collaborators.some((collaborator) => collaborator.id === event.locals.user?.id);

	return { deck, flashcards, collaborators, isCreator, isEditor };
};
