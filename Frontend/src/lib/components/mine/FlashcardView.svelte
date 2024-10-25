<script lang="ts">
	import { type Flashcard, FlashType } from '$lib/models/flashcard';
	import ResizingTextArea from '$lib/components/utils/ResizingTextArea.svelte';
	import { Button } from '$lib/components/ui/button';
	import { Label } from '$lib/components/ui/label';
	import { Separator } from '$lib/components/ui/separator';
	import { ApiClient } from '$lib/utils/ApiClient';
	import { Save, Space, SquarePen } from 'lucide-svelte';
	import { error } from '@sveltejs/kit';

	const saveChanges = () => {
		ApiClient.UpdateFlashcard(flashcard)
			// .then((updatedFlashcard: Flashcard) => {
			// 	flashcard = updatedFlashcard;
			// })
			.catch((e) => {
				throw e;
			});

		editMode = false;
	};
	const editButtonClicked = () => {
		if (editMode) saveChanges();
		else editMode = true;
	};

	export let flashcard: Flashcard;
	export let editable: boolean = true;

	let editMode: boolean = false;
</script>

<div class="relative rounded-lg border-2 p-4" class:pr-12={editable}>
	{#if editable}
		<Button variant="ghost" on:click={editButtonClicked} class="absolute right-0 top-0 px-3 py-1">
			{#if editMode}
				<Save />
			{:else}
				<SquarePen />
			{/if}
		</Button>
	{/if}

	{#if flashcard.flashType === FlashType.NORMAL}
		<div class="lg:flex">
			<div class="w-full">
				{#if editMode}
					<ResizingTextArea placeHolder="Enter front text here..." bind:value={flashcard.front} />
				{:else}
					<pre class="whitespace-pre-wrap">{flashcard.front}</pre>
				{/if}
			</div>

			<Separator orientation="vertical" class="mx-4 w-[2px] " />
			<Separator orientation="horizontal" class="my-4 h-[2px] lg:hidden" />

			<div class="w-full">
				{#if editMode}
					<ResizingTextArea placeHolder="Enter back text here..." bind:value={flashcard.back} />
				{:else}
					<pre class="whitespace-pre-wrap">{flashcard.back}</pre>
				{/if}
			</div>
		</div>
	{:else if flashcard.flashType === FlashType.CLOZE}
		{#if editMode}
			<ResizingTextArea placeHolder="Enter cloze text here..." bind:value={flashcard.back} />
		{:else}
			<pre class="whitespace-pre-wrap">{flashcard.front}</pre>
		{/if}
	{/if}
</div>
