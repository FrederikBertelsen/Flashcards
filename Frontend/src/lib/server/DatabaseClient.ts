import { type Flashcard, FlashType } from '$lib/models/flashcard';
import type { Deck } from '$lib/models/deck';
import { Delete, Get, HandleCastError, Post, Put } from '$lib/utils/FetchUtils';
import type { User } from '$lib/models/User';
import type { DatabaseSession, Session } from 'lucia';
import type { UpdateDeck } from '$lib/models/updateDeck';
import type { Collaborator } from '$lib/models/Collaborator';
import type { Review } from '$lib/models/Review';
import type { LearningSession } from '$lib/models/LearningSession';
import { type Card, type ReviewLog, TypeConvert } from 'ts-fsrs';

const domain = 'http://localhost:5213';

class DatabaseClient {
	// Deck
	static async CreateDeck(name: string, sessionId: string | undefined): Promise<Deck> {
		const url = domain + '/api/decks';
		const json = await Post(url, { name }, sessionId);

		json.creator = json.creator as User;

		return HandleCastError<Deck>(json, 'Deck');
	}

	static async ImportQuizletDeck(quizletDeckId: string, sessionId: string | undefined): Promise<Deck> {
		const url = domain + `/api/import/quizlet/${quizletDeckId}`;
		const json = await Post(url, {}, sessionId);

		json.creator = json.creator as User;

		return HandleCastError<Deck>(json, 'Deck');
	}

	static async GetDecks(): Promise<Deck[]> {
		const url = domain + '/api/decks';
		const json = await Get(url);

		return HandleCastError<Deck[]>(json, 'Deck[]');
	}

	static async GetDeckById(deckId: string, sessionId: string | undefined): Promise<Deck | null> {
		const url = domain + `/api/decks/${deckId}`;
		const json = await Get(url, sessionId);
		if (!json) return null;

		return HandleCastError<Deck>(json, 'Deck');
	}

	static async GetDecksByUserId(userId: string, sessionId: string | undefined): Promise<Deck[]> {
		const url = domain + `/api/users/${userId}/decks`;
		const json = await Get(url, sessionId);

		return HandleCastError<Deck[]>(json, 'Deck[]');
	}

	static async GetDecksBySearchTerm(searchTerm: string): Promise<Deck[]> {
		const url = domain + `/api/decks/search/${searchTerm}`;
		const json = await Get(url);

		return HandleCastError<Deck[]>(json, 'Deck[]');
	}

	static async UpdateDeck(updateDeck: UpdateDeck, sessionId: string | undefined): Promise<void> {
		const url = domain + `/api/decks/${updateDeck.id}`;
		await Put(url, updateDeck, sessionId);
	}

	static async DeleteDeck(deckId: string, sessionId: string | undefined): Promise<void> {
		const url = domain + `/api/decks/${deckId}`;
		await Delete(url, sessionId);
	}

	// Flashcard
	static async CreateFlashcard(deckId: string, flashType: FlashType, front: string, back: string, sessionId: string | undefined): Promise<Flashcard> {
		const url = domain + '/api/flashcards';
		const json = await Post(url, { deckId, flashType, front, back }, sessionId);

		return HandleCastError<Flashcard>(json, 'Flashcard');
	}

	static async GetFlashcardsByDeckId(deckId: string, sessionId: string | undefined): Promise<Flashcard[] | null> {
		const url = domain + `/api/decks/${deckId}/Flashcards`;
		const json = await Get(url, sessionId);
		if (!json) return null;

		return HandleCastError<Flashcard[]>(json, 'Flashcard[]');
	}

	static async UpdateFlashcard(flashcard: Flashcard, sessionId: string | undefined): Promise<void> {
		const url = domain + `/api/flashcards/${flashcard.id}`;
		await Put(url, { id: flashcard.id, front: flashcard.front, back: flashcard.back, flashType: flashcard.flashType }, sessionId);
	}

	static async DeleteFlashcard(flashcardId: string, sessionId: string | undefined): Promise<void> {
		const url = domain + `/api/flashcards/${flashcardId}`;
		await Delete(url, sessionId);
	}

	// User
	static async CreateUser(name: string, googleId: string, pictureUrl: string): Promise<User> {
		const url = domain + '/api/users';
		const json = await Post(url, { name, googleId, pictureUrl });

		return HandleCastError<User>(json, 'User');
	}

	static async GetUserById(userId: string): Promise<User | null> {
		const url = domain + `/api/users/${userId}`;
		const json = await Get(url);
		if (!json) return null;

		return HandleCastError<User>(json, 'User');
	}

	static async GetUserByGoogleId(googleId: string): Promise<User | null> {
		const url = domain + `/api/users/google/${googleId}`;
		const json = await Get(url);
		if (!json) return null;

		return HandleCastError<User>(json, 'User');
	}

	static async IsUserAdmin(userId: string, googleId: string, sessionId: string | undefined): Promise<boolean> {
		const url = domain + `/api/users/admin`;
		const json = await Post(url, { userId, googleId }, sessionId);

		return HandleCastError<boolean>(json, 'boolean');
	}

	static async UpdateUser(userId: string, newUsername: string, sessionId: string | undefined): Promise<void> {
		const url = domain + `/api/users/${userId}`;
		await Put(url, { newUsername }, sessionId);
	}

	static async DeleteUser(userId: string, sessionId: string | undefined): Promise<void> {
		const url = domain + `/api/users/${userId}`;
		await Delete(url, sessionId);
	}

	// Collaborator
	static async GetCollaborators(deckId: string, sessionId: string | undefined): Promise<Collaborator[]> {
		const url = domain + `/api/decks/${deckId}/collaborators`;
		const json = await Get(url, sessionId);

		return HandleCastError<Collaborator[]>(json, 'Collaborator[]');
	}

	static async UpdateCollaborators(deckId: string, collaboratorIds: string[], sessionId: string | undefined): Promise<void> {
		const url = domain + `/api/decks/${deckId}/collaborators`;
		await Put(url, collaboratorIds, sessionId);
	}

	// Session
	static async CreateSession(session: DatabaseSession): Promise<Session> {
		const url = domain + '/api/sessions';
		const json = await Post(url, { id: session.id, userId: session.userId, expiresAt: session.expiresAt });

		return HandleCastError<Session>(json, 'Session');
	}

	static async GetSessionById(sessionId: string): Promise<Session | null> {
		const url = domain + `/api/sessions/${sessionId}`;
		const json = await Get(url);
		if (!json) return null;

		json.expiresAt = new Date(json.expiresAt);

		return HandleCastError<Session>(json, 'Session');
	}

	static async GetUserSessions(userId: string): Promise<Session[]> {
		const url = domain + `/api/sessions/user/${userId}`;
		const json = await Get(url);

		return HandleCastError<Session[]>(json, 'Session[]');
	}

	static async updateSessionExpiration(sessionId: string, newExpiresAt: Date): Promise<void> {
		const url = domain + `/api/sessions/${sessionId}`;
		await Put(url, { newExpiresAt });
	}

	static async DeleteSession(sessionId: string): Promise<void> {
		const url = domain + `/api/sessions/${sessionId}`;
		await Delete(url);
	}

	static async DeleteUserSessions(userId: string): Promise<void> {
		const url = domain + `/api/sessions/user/${userId}`;
		await Delete(url);
	}

	static async DeleteExpiredSessions(): Promise<void> {
		const url = domain + '/api/sessions/expired';
		await Delete(url);
	}

	// Learning Session
	static async CreateLearningSession(name: string, deckId: string, userId: string, reviews: Review[], sessionId: string | undefined): Promise<LearningSession> {
		const url = domain + '/api/learning-sessions';
		const json = await Post(url, { name, deckId, userId, reviews }, sessionId);

		return HandleCastError<LearningSession>(json, 'LearningSession');
	}

	static async GetLearningSessionsByUserId(userId: string, sessionId: string | undefined): Promise<LearningSession[]> {
		const url = domain + `/api/users/${userId}/learning-sessions`;
		const json = await Get(url, sessionId);

		return HandleCastError<LearningSession[]>(json, 'LearningSession[]');
	}

	static async GetDueReviewsByLearningSessionId(learningSessionId: string, sessionId: string | undefined): Promise<Review[]> {
		const url = domain + `/api/learning-sessions/${learningSessionId}/due-reviews`;
		const json = await Get(url, sessionId);

		json.forEach((review: any) => {
			review.card = TypeConvert.card(review.card);
			if (review.card.due) review.card.due = new Date(review.card.due);
			if (review.card.last_review) review.card.last_review = new Date(review.card.last_review);
		});

		return HandleCastError<Review[]>(json, 'Review[]');
	}

	static async CreateReview(learningSessionId: string, flashcardId: string, clozeIndex: number, card: Card, sessionId: string | undefined): Promise<void> {
		const url = domain + '/api/reviews';
		await Post(url, { learningSessionId, flashcardId, clozeIndex, card }, sessionId);
	}

	static async UpdateReview(reviewId: string, card: Card, sessionId: string | undefined): Promise<void> {
		const url = domain + `/api/reviews/${reviewId}`;
		await Put(url, { card }, sessionId);
	}

	static async CreateReviewLog(userId: string, learningSessionId: string, reviewId: string, log: ReviewLog, sessionId: string | undefined): Promise<void> {
		const url = domain + '/api/review-logs';
		await Post(url, { userId, learningSessionId, reviewId, log }, sessionId);
	}
}

export default DatabaseClient;
