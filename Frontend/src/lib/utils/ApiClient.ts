import type { Deck } from '$lib/models/deck';
import { HandleCastError, Post, Put } from '$lib/utils/FetchUtils';
import { type Flashcard, FlashType } from '$lib/models/flashcard';
import { Mapper } from '$lib/utils/Mapper';

export class ApiClient {
	static async CreateDeck(name: string): Promise<Deck> {
		const endpoint = '/decks';
		const json = await Post(endpoint, { name });

		return HandleCastError<Deck>(json.deck, 'Deck');
	}

	static async ImportQuizletDeck(quizletDeckId: string): Promise<Deck> {
		const endpoint = '/decks/import/quizlet';
		const json = await Post(endpoint, { quizletDeckId });

		return HandleCastError<Deck>(json.deck, 'Deck');
	}

	static async CreateFlashcard(deckId: string, front: string, back: string, flashType: FlashType): Promise<Flashcard> {
		const endpoint = '/flashcards';
		const json = await Post(endpoint, { deckId, front, back, flashType });

		return HandleCastError<Flashcard>(json, 'Flashcard');
	}

	static async UpdateDeck(deck: Deck): Promise<void> {
		const endpoint = `/decks/${deck.id}`;

		await Put(endpoint, Mapper.ToUpdateDeck(deck));
	}

	static async UpdateCollaborators(deckId: string, collaboratorIds: string[]): Promise<void> {
		const endpoint = `/decks/${deckId}/collaborators`;
		await Put(endpoint, collaboratorIds);
	}

	static async UpdateFlashcard(flashcard: Flashcard): Promise<void> {
		const endpoint = `/flashcards/${flashcard.id}`;
		await Put(endpoint, flashcard);
	}
}
