<script lang="ts">
	import type { PageData } from './$types';
	import FlashcardList from '$lib/components/mine/FlashcardList.svelte';
	import FlashcardCreator from '$lib/components/mine/FlashcardCreator.svelte';
	import { ApiClient } from '$lib/utils/ApiClient';
	import { Button, buttonVariants } from '$lib/components/ui/button';
	import { Switch } from '$lib/components/ui/switch';
	import { Label } from '$lib/components/ui/label';
	import { Input } from '$lib/components/ui/input';
	import FlashcardReview from '$lib/components/mine/FlashcardReview.svelte';
	// DO NOT REMOVE
	import * as Dialog from '$lib/components/ui/dialog';
	import * as Tabs from '$lib/components/ui/tabs';
	import * as Card from '$lib/components/ui/card';
	import * as Avatar from '$lib/components/ui/avatar';
	import { ROUTES } from '$lib/Routes';
	// DO NOT REMOVE

	const handleFlashcardCreated = (event: CustomEvent) => {
		flashcards = [event.detail.flashcard, ...flashcards];
	};

	const saveChanges = () => {
		ApiClient.UpdateDeck(deck);
		let collaboratorIdList = collaboratorIdString.split(',').map((id) => id.trim());

		if (collaboratorIdList.length > 0) ApiClient.UpdateCollaborators(deck.id, collaboratorIdList);
		else alert(`No collaborators added: ${collaboratorIdList}`);
	};

	export let data: PageData;
	let deck = data.deck;
	let flashcards = data.flashcards;
	const isCreator = data.isCreator;
	const collaborators = data.collaborators;
	const isEditor = data.isEditor;

	let collaboratorIdString = collaborators.map((c) => c.id).join(', ');
</script>

<div class="deck space-y-10">
	<div class="flex items-center space-x-8">
		<h1>{deck.name}</h1>
		{#if isCreator}
			<Dialog.Root>
				<Dialog.Trigger class={buttonVariants({ variant: 'outline' })}>Edit Deck</Dialog.Trigger>
				<Dialog.Content class="sm:max-w-[425px]">
					<Dialog.Header>
						<Dialog.Title>Edit Deck</Dialog.Title>
						<Dialog.Description>Make changes to your deck here. Click save when you're done.</Dialog.Description>
					</Dialog.Header>
					<div class="space-y-2">
						<div>
							<Label for="is-public">Public</Label>
							<Switch id="is-public" bind:checked={deck.isPublic} />
						</div>

						<div>
							<Label for="name">Name</Label>
							<Input id="name" bind:value={deck.name} />
						</div>

						<div>
							<Label for="collaborators">Collaborators</Label>
							<Input id="collaborators" bind:value={collaboratorIdString} />
						</div>
					</div>
					<Dialog.Close on:click={saveChanges}>Save changes</Dialog.Close>
				</Dialog.Content>
			</Dialog.Root>
		{/if}
		<div class="flex flex-col items-center border">
			<div class="flex">
				<Avatar.Root>
					<Avatar.Image src={deck.creator.pictureUrl} alt={deck.creator.name} />
					<Avatar.Fallback>404</Avatar.Fallback>
				</Avatar.Root>
				<Button variant = "link" href={ROUTES.USER(deck.creator.id)}>{deck.creator.name}</Button>
			</div>
			<div class="flex space-x-1">
				<p>Collaborators:</p>
				{#each collaborators as collaborator}
					<Button variant = "link" href={ROUTES.USER(collaborator.id)}>{collaborator.name}</Button>
				{/each}
			</div>
		</div>
	</div>

	<Tabs.Root value="none">
		<Tabs.List>
			{#if isEditor}
				<Tabs.Trigger value="create">Create Flashcard</Tabs.Trigger>
			{/if}
			<Tabs.Trigger value="review">Review</Tabs.Trigger>
		</Tabs.List>
		{#if isEditor}
			<Tabs.Content value="create">
				<Card.Root>
					<!--				<Card.Header>-->
					<!--					<Card.Title class="text-3xl text-bold text-center">Create Flashcard</Card.Title>-->
					<!--				</Card.Header>-->
					<Card.Content class="pt-6">
						<FlashcardCreator {deck} on:flashcardCreated={handleFlashcardCreated} />
					</Card.Content>
					<Card.Footer>
						<!--					<Button>Save changes</Button>-->
					</Card.Footer>
				</Card.Root>
			</Tabs.Content>
		{/if}
		<Tabs.Content value="review">
			<Card.Root>
				<Card.Header>
					<!--					<Card.Title>Review Flashcard</Card.Title>-->
					<!--					<Card.Description>Create a new Flashcard for this Deck.</Card.Description>-->
				</Card.Header>
				<Card.Content class="space-y-2">
					{#if flashcards.length !== 0}
						<FlashcardReview {flashcards} />
					{/if}
				</Card.Content>
				<Card.Footer>
					<!--					<Button>Save changes</Button>-->
				</Card.Footer>
			</Card.Root>
		</Tabs.Content>
	</Tabs.Root>

	<FlashcardList {flashcards} editable={isEditor} />
</div>
