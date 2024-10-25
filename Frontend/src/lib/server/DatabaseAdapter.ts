import DatabaseClient from './DatabaseClient';
import type { Adapter, DatabaseSession, DatabaseUser, UserId } from 'lucia';
import { error } from '@sveltejs/kit';
import { Mapper } from '$lib/utils/Mapper';

class DatabaseAdapter implements Adapter {
	async deleteExpiredSessions(): Promise<void> {
		await DatabaseClient.DeleteExpiredSessions();
	}

	async deleteSession(sessionId: string): Promise<void> {
		await DatabaseClient.DeleteSession(sessionId);
	}

	async deleteUserSessions(userId: UserId): Promise<void> {
		await DatabaseClient.DeleteUserSessions(userId);
	}

	async getSessionAndUser(sessionId: string): Promise<[DatabaseSession | null, DatabaseUser | null]> {
		const session = await DatabaseClient.GetSessionById(sessionId);
		if (!session) return [null, null];

		const user = await DatabaseClient.GetUserById(session.userId);
		if (!user) error(404, "Session's User not found");

		const databaseSession: DatabaseSession = Mapper.toDatabaseSession(session);
		const databaseUser: DatabaseUser = Mapper.toDatabaseUser(user);

		return [databaseSession, databaseUser];
	}

	async getUserSessions(userId: UserId): Promise<DatabaseSession[]> {
		const sessions = await DatabaseClient.GetUserSessions(userId);
		return sessions.map((session) => Mapper.toDatabaseSession(session));
	}

	async setSession(session: DatabaseSession): Promise<void> {
		await DatabaseClient.CreateSession(session);
	}

	async updateSessionExpiration(sessionId: string, expiresAt: Date): Promise<void> {
		await DatabaseClient.updateSessionExpiration(sessionId, expiresAt);
	}
}

export default DatabaseAdapter;
