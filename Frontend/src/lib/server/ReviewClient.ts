import type { LearningSession } from '$lib/models/LearningSession';
import DatabaseClient from '$lib/server/DatabaseClient';
import { createEmptyCard, default_enable_short_term, default_maximum_interval, default_request_retention, default_w, FSRS, fsrs, generatorParameters } from 'ts-fsrs';
import type { Review } from '$lib/models/Review';

const params = generatorParameters({
	// request_retention: default_request_retention,
	// maximum_interval: default_maximum_interval,
	// w: default_w,
	enable_fuzz: true // default_enable_fuzz,
	// enable_short_term: default_enable_short_term,
});

class ReviewClient {
	static GetFSRS(): FSRS {
		return fsrs(params);
	}
	static async CreateLearningSession(name: string, deckId: string, userId: string, sessionId: string | undefined): Promise<LearningSession> {
		const flashcards = await DatabaseClient.GetFlashcardsByDeckId(deckId, sessionId);
		if (!flashcards || flashcards.length === 0) throw new Error('Deck is empty');

		let reviews: Review[] = [];

		// const f = this.GetFSRS();
		flashcards.forEach((flashcard) => {
			const card = createEmptyCard(); // add shared date for all??

			const newReview: Review = {
				id: '',
				learningSessionId: '',
				flashcardId: flashcard.id,
				clozeIndex: -1,
				card: card
			};

			reviews.push(newReview);
		});

		return await DatabaseClient.CreateLearningSession(name, deckId, userId, reviews, sessionId);
	}
}

export default ReviewClient;
