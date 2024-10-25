import type { PageServerLoad } from './$types';
import { handleAuthorizationRedirect } from '$lib/server/authUtils';
import { Database } from 'lucide-svelte';
import DatabaseClient from '$lib/server/DatabaseClient';

export const load: PageServerLoad = async (event) => {
	await handleAuthorizationRedirect(event);

	let user = await DatabaseClient.GetUserById(event.params.userid);

	if (!user) {
		return {
			status: 404,
			error: {
				message: 'User not found!'
			}
		};
	}

	let isCurrentUser = event.locals.user?.id === user?.id;

	return {
		user,
		isCurrentUser
	};
};
