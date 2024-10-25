export class ROUTES {
	static readonly ROOT = '/';

	static readonly DECKS = '/decks';
	static readonly CREATE_DECK = '/decks/create';
	static DECK_SEARCH(searchTerm: string): string {
		return `/decks/search/${searchTerm}`;
	}
	static USER(userId: string): string {
		return `/users/${userId}`;
	}
	static USERS_DECKS(userId: string): string {
		return `${ROUTES.USER(userId)}/decks`;
	}

	static readonly LOGIN = '/login';
	static readonly LOGOUT = '/logout';
	static readonly GOOGLE_OAUTH = '/login/google';

	static readonly ADMIN = '/admin';

	static readonly NOT_FOUND = '/404';
}
