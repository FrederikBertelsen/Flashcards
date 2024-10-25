<script lang="ts">
	import { Input } from '$lib/components/ui/input';
	import { Button } from '$lib/components/ui/button';
	import { ApiClient } from '$lib/utils/ApiClient';
	import type { Deck } from '$lib/models/deck';
	import { goto } from '$app/navigation';
	import type { PageData } from './$types';
	import { error } from '@sveltejs/kit';

	export let data: PageData;

	let deckName: string = '';
	let quizletId: string = '';

	const postDeckAndRedirect = async () => {
		if (data.user) {
			ApiClient.CreateDeck(deckName)
				.then((newDeck: Deck) => {
					goto(`/decks/${newDeck.id}`);
				})
				.catch((e) => {
					console.error(e);
				});
		}
	};

	const ImportQuizletAndRedirect = async () => {
		if (data.user) {
			ApiClient.ImportQuizletDeck(quizletId)
				.then((newDeck: Deck) => {
					goto(`/decks/${newDeck.id}`);
				})
				.catch((e) => {
					console.error(e);
				});
		}
	};
</script>

<h1>Create new Deck</h1>

<div class="space-y-4 py-6">
	<form>
		<div class="flex items-center space-x-5">
			<Input type="text" id="deck-name" placeholder="Enter deck name here..." required bind:value={deckName} />
			<Button type="submit" on:click={postDeckAndRedirect}>Create Deck</Button>
		</div>
	</form>
	<form>
		<div class="flex items-center space-x-5">
			<Input type="text" id="quizlet-id" placeholder="Enter Quizlet ID here..." required bind:value={quizletId} />
			<Button type="submit" on:click={ImportQuizletAndRedirect}>Import from Quizlet</Button>
		</div>
	</form>
</div>
