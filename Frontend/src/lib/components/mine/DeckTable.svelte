<script lang="ts">
	import * as Table from '$lib/components/ui/table';
	import { goto } from '$app/navigation';
	import type { Deck } from '$lib/models/deck';
	import { ROUTES } from '$lib/Routes';
	import { Button } from '$lib/components/ui/button';

	export let deckList: Deck[];
</script>

<Table.Root class="border-2">
	<Table.Header>
		<Table.Row class="bg-muted hover:bg-muted dark:bg-muted/50 dark:hover:bg-muted/50 ">
			<Table.Head>Deck Name</Table.Head>
			<Table.Head>Flashcards</Table.Head>
			<Table.Head class="text-right">ID</Table.Head>
			<Table.Head class="text-right">Creator</Table.Head>
		</Table.Row>
	</Table.Header>
	<Table.Body>
		{#each deckList as deck}
			<Table.Row class="cursor-pointer hover:bg-muted dark:hover:bg-muted/50" on:click={async () => await goto(`/decks/${deck.id}`)}>
				<Table.Cell class="font-medium">{deck.name}</Table.Cell>
				<Table.Cell>{deck.flashcardCount}</Table.Cell>
				<Table.Cell class="text-right">{deck.id}</Table.Cell>
				<Table.Cell class="text-right"><Button variant="link" href={ROUTES.USER(deck.creator.id)}><p class="text-sm">{deck.creator.name}</p></Button></Table.Cell>
			</Table.Row>
		{/each}
	</Table.Body>
</Table.Root>
