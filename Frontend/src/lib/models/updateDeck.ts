import type { User } from '$lib/models/User';

export interface UpdateDeck {
	id: string;
	name: string;
	isPublic: boolean;
}
