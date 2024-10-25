<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import { type Flashcard, FlashType } from '$lib/models/flashcard';
	import type { Deck } from '$lib/models/deck';
	import ResizingTextArea from '$lib/components/utils/ResizingTextArea.svelte';
	import { Label } from '$lib/components/ui/label';
	import { Button } from '$lib/components/ui/button';

	// DO NOT REMOVE
	import * as Select from '$lib/components/ui/select';
	// DO NOT REMOVE

	import { ApiClient } from '$lib/utils/ApiClient';

	const dispatch = createEventDispatcher();

	const createFlashcard = async () => {
		if (front.trim().length === 0 || (flashType.value === FlashType.NORMAL && back.trim().length === 0)) {
			alert('Please fill in all fields to create the flashcard.');
			return;
		}

		const flashcard = await ApiClient.CreateFlashcard(deck.id, front, back, flashType.value);

		front = '';
		back = '';
		dispatch('flashcardCreated', { flashcard });
	};

	export let deck: Deck;

	let front: string = '';
	let back: string = '';

	$: flashType = { name: 'Normal', value: FlashType.NORMAL };
	let flashTypes = [
		{ name: 'Normal', value: FlashType.NORMAL },
		{ name: 'Cloze', value: FlashType.CLOZE }
	];
</script>

<!--<div class="text-center">-->
<!--	<h2>Create new Flashcard</h2>-->
<!--</div>-->

<form class="space-y-6" on:submit|preventDefault={createFlashcard}>
	<div>
		<Label>Flashcard type</Label>

		<Select.Root bind:selected={flashType}>
			<Select.Trigger class="w-[200px]">
				<Select.Value placeholder="Normal" />
			</Select.Trigger>
			<Select.Content>
				{#each flashTypes as { value, name }}
					<Select.Item {value}>{name}</Select.Item>
				{/each}
			</Select.Content>
		</Select.Root>
	</div>

	<div>
		<Label for="front">
			{#if flashType.value === FlashType.NORMAL}
				Front
			{:else}
				Cloze
			{/if}
		</Label>
		<ResizingTextArea id="front" placeHolder="Enter {flashType.value === FlashType.NORMAL ? 'front' : 'cloze'} text here..." bind:value={front} />
	</div>

	{#if flashType.value === FlashType.NORMAL}
		<div>
			<Label for="back">Back</Label>
			<ResizingTextArea id="back" placeHolder="Enter back text here..." bind:value={back} />
		</div>
	{/if}

	<Button class="w-full" type="submit">Create Flashcard</Button>
</form>
