import type { UserId } from 'lucia';

export interface User {
	id: UserId;
	isAdmin: boolean;
	name: string;
	googleId: string;
	pictureUrl: string;
}
