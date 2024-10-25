import { State } from 'ts-fsrs';

export interface Review {
	id: string;
	learningSessionId: string;
	flashcardId: string;
	clozeIndex: number;
	card: {
		due: Date;
		stability: number;
		difficulty: number;
		elapsed_days: number;
		scheduled_days: number;
		reps: number;
		lapses: number;
		state: State;
		last_review?: Date;
	};
}
