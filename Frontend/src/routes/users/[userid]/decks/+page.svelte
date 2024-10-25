<script lang="ts">
	import type { PageData } from './$types';
	import DeckTable from '$lib/components/mine/DeckTable.svelte';
	import type { Deck } from '$lib/models/deck';
	import { Button } from '$lib/components/ui/button';
	import type { User } from '$lib/models/User';
	import { ROUTES } from '$lib/Routes';

	export let data: PageData;
	const user: User = data.user;
	const isCurrentUser = data.isCurrentUser;
	const deckList: Deck[] = data.deckList;
</script>

<div class="flex items-center">
	{#if isCurrentUser}
		<h1>Your Decks</h1>
		<Button href="{ROUTES.CREATE_DECK}" class="ml-auto">
			New Deck
		</Button>
	{:else}
		<h1>{user.name}'s Decks</h1>
	{/if}
</div>

<!--<AwaitPromise promise={data.deckList} bind:value={deckList}>-->
<DeckTable deckList={deckList} />
<!--</AwaitPromise>-->