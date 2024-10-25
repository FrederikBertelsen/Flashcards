<script lang="ts">
	import type { PageData } from './$types';
	import DeckTable from '$lib/components/mine/DeckTable.svelte';
	import AwaitPromise from '$lib/components/utils/AwaitPromise.svelte';
	import type { Deck } from '$lib/models/deck';
	import { Button } from '$lib/components/ui/button';
	import { ROUTES } from '$lib/Routes';

	export let data: PageData;
	let deckList: Deck[] = data.deckList;
</script>

<div class="flex items-center">
	<h1>All Decks</h1>
	<Button href={ROUTES.CREATE_DECK} class="ml-auto">
		New Deck
	</Button>
</div>

<!--<AwaitPromise promise={data.deckList} bind:value={deckList}>-->
	<DeckTable deckList={deckList} />
<!--</AwaitPromise>-->