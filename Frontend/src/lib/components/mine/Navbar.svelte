<script lang="ts">
	import { Input } from '$lib/components/ui/input';
	import { Search } from 'lucide-svelte';
	import DarkMode from '$lib/components/ui/darkmode/DarkMode.svelte';
	import Logo from '$lib/components/mine/Logo.svelte';
	import { ScrollArea } from '$lib/components/ui/scroll-area';
	import type { User } from '$lib/models/User';
	import { ROUTES } from '$lib/Routes';

	// DO NOT REMOVE
	import * as Avatar from '$lib/components/ui/avatar/index.js';
	import { Button } from '$lib/components/ui/button';
	// DO NOT REMOVE

	export let user: User | null;

	let searchQuery = '';

	const search = () => {
		searchQuery = searchQuery.trim();
		if (searchQuery === '') return;

		const searchUrl = ROUTES.DECK_SEARCH(searchQuery);
		window.location.href = searchUrl;
		searchQuery = '';
	};

	const handleKeyDown = (event: KeyboardEvent) => {
		if (event.key === 'Enter') search();
	};

	$: searchQuery = '';

	const navButtons = [
		['Home', ROUTES.ROOT],
		['Browse', ROUTES.DECKS],
	];

	if (user) {
		navButtons.push(['Your Decks', ROUTES.USERS_DECKS(user.id)]);
	}
</script>

<div class="flex h-full w-full">
	<header class="left-bar bg-muted/20 flex h-full flex-col border-r-2 px-6">
		<div class="flex items-center justify-center py-6">
			<a class="mr-2" href="/">
				<Logo />
			</a>
		</div>

		<nav class="flex flex-col space-y-4 py-8">
			{#each navButtons as [title, url]}
				<a class="menu-link" href={url}>{title}</a>
			{/each}

		</nav>
	</header>

	<ScrollArea class="w-full">
		<header class="top-bar bg-muted/20 flex space-x-4 border-b-2 p-4">
			<DarkMode />

			<div class="relative grid w-full items-center">
				<Search class="absolute left-2 z-10" />
				<Input id="search-navbar" class="ps-10" placeholder="Search..." maxlength={64} bind:value={searchQuery} on:keydown={handleKeyDown}></Input>
			</div>
			<div class="flex items-center space-x-2 whitespace-nowrap">
				{#if user}
					<a class="" href={ROUTES.USER(user.id)}>
						<Avatar.Root>
							<Avatar.Image src={user.pictureUrl} alt={user.name} />
							<Avatar.Fallback>404</Avatar.Fallback>
						</Avatar.Root>
					</a>

					<a class="menu-link" href={ROUTES.LOGOUT}>Log out</a>
				{:else}
					<a class="menu-link" href={ROUTES.LOGIN}>log in</a>
				{/if}
			</div>
		</header>
		<div class="content">
			<slot />
		</div>
	</ScrollArea>
</div>
