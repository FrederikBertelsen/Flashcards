<script lang="ts">
	import type { PageData } from './$types';
	// DO NOT REMOVE
	import * as Avatar from '$lib/components/ui/avatar/index.js';
	// DO NOT REMOVE

	export let data: PageData;
	const user = data.user;
	const isCurrentUser = data.isCurrentUser;
</script>

{#if user}
	<div class="flex items-center space-x-4 py-4">
		<h1>{user.name}</h1>

		<Avatar.Root class="h-20 w-20">
			<Avatar.Image src={user.pictureUrl} alt={user.name} />
			<Avatar.Fallback>404</Avatar.Fallback>
		</Avatar.Root>
	</div>

	{#if isCurrentUser}
		<p>Your user ID is {data.user.id}.</p>
		<p>Your Google ID is {data.user.googleId}.</p>
		<p>Your PictureUrl is {data.user.pictureUrl}.</p>
	{/if}
{:else}
	<h1>User not found!</h1>
{/if}