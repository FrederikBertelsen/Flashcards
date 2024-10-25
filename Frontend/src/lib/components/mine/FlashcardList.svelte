<script lang="ts">
	import FlashcardView from '$lib/components/mine/FlashcardView.svelte';
	import type { Flashcard } from '$lib/models/flashcard';
	import { Input } from '$lib/components/ui/input';
	import { Separator } from '$lib/components/ui/separator';

	export let flashcards: Flashcard[];
	export let editable: boolean
	let usedFlashcards = flashcards;
	$: searchQuery = '';

	$: {
		if (searchQuery.length < 3) {
			usedFlashcards = flashcards;
		} else {
			usedFlashcards = flashcards.filter((flashcard) => {
				return flashcard.front.toLowerCase().includes(searchQuery.toLowerCase()) ||
					flashcard.back.toLowerCase().includes(searchQuery.toLowerCase());
			});
		}
	}

</script>

<div class="deck-list">
	<div class="sm:flex sm:space-x-3 mb-2 mt-4">
		<h2 class="whitespace-nowrap">Flashcards ({flashcards.length})</h2>
		<div class="w-full max-w-md">
			<Input placeholder="Search in flashcards..." maxlength={64} bind:value={searchQuery} />
		</div>
	</div>

	<div class="flashcards space-y-2">
		{#each usedFlashcards as flashcard}
				<FlashcardView {flashcard} {editable} />
		{/each}
	</div>
</div>
