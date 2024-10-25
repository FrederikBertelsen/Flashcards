<script lang="ts">
	import { page } from '$app/stores';
	import type { PageData } from './$types';
	import DeckTable from '$lib/components/mine/DeckTable.svelte';
	import AwaitPromise from '$lib/components/utils/AwaitPromise.svelte';
	import type { Deck } from '$lib/models/deck';

	export let data: PageData;
	let searchTerm = $page.params.searchterm;
	let deckList: Deck[];
</script>

<h1 class="pb-5">Search Results for '{searchTerm}'</h1>

<AwaitPromise promise={data.deckListPromise}  bind:value={deckList}>
	{#if deckList.length === 0}
		<p class="text-2xl">No decks found!</p>
	{:else}
		<DeckTable deckList={deckList} />
	{/if}
</AwaitPromise>