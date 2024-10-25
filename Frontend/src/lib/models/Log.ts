import { Rating, State } from 'ts-fsrs';

export interface Log {
	userID: string;
	learningSessionId: string;
	reviewId: string;
	log: {
		rating: Rating;
		state: State;
		due: Date;
		stability: number;
		difficulty: number;
		elapsed_days: number;
		last_elapsed_days: number;
		scheduled_days: number;
		review: Date;
	};
}
