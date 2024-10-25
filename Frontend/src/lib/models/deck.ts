import type { User } from '$lib/models/User';

export interface Deck {
	id: string;
	name: string;
	creator: {
		id: string;
		isAdmin: boolean;
		name: string;
		googleId: string;
		pictureUrl: string;
	};
	isPublic: boolean;
	flashcardCount: number;
}
