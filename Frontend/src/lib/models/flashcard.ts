export interface Flashcard {
	id: string;
	deckId: string;
	flashType: number;
	front: string;
	back: string;
}

export enum FlashType {
	NORMAL = 0,
	CLOZE = 1
}
