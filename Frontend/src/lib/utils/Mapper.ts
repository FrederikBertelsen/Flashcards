import type { DatabaseSession, DatabaseUser, Session, UserId } from 'lucia';
import type { User } from '$lib/models/User';
import type { Deck } from '$lib/models/deck';
import type { UpdateDeck } from '$lib/models/updateDeck';

export class Mapper {
	static toDatabaseSession(session: Session): DatabaseSession {
		return {
			userId: session.userId as UserId,
			expiresAt: session.expiresAt,
			id: session.id,
			attributes: {}
		};
	}

	static ToUpdateDeck(deck: Deck): UpdateDeck {
		return {
			id: deck.id,
			name: deck.name,
			isPublic: deck.isPublic
		};
	}

	static toDatabaseUser(user: User): DatabaseUser {
		return {
			id: user.id as UserId,
			attributes: { googleId: user.googleId, username: user.name, pictureUrl: user.pictureUrl } as DatabaseUser['attributes']
		};
	}

	// static fromDatabaseSession(databaseSession: DatabaseSession): Session {
	// 	return {
	// 		userId: databaseSession.userId,
	// 		expiresAt: databaseSession.expiresAt,
	// 		fresh: true,
	// 		id: databaseSession.id
	// 	};
	// }
	//
	// static fromDatabaseUser(databaseUser: DatabaseUser): User {
	// 	return {
	// 		id: databaseUser.id,
	// 		name: databaseUser.attributes.username,
	// 		googleId: databaseUser.attributes.googleId,
	// 		pictureUrl: databaseUser.attributes.pictureUrl
	// 	};
	// }
}
