<script lang="ts">
	import { type Flashcard, FlashType } from '$lib/models/flashcard';
	import { Button } from '$lib/components/ui/button';
	import { SeparatorHorizontal } from 'lucide-svelte';
	import { Separator } from '$lib/components/ui/separator';
	import { Textarea } from '$lib/components/ui/textarea';
	import ResizingTextArea from '$lib/components/utils/ResizingTextArea.svelte';
	import { Label } from '$lib/components/ui/label';

	export let flashcards: Flashcard[];
	export let randomized: boolean = false;

	if (randomized) flashcards = flashcards.sort(() => Math.random() - 0.5);

	let resizingTextArea: ResizingTextArea;
	let resizingTextAreaCloze: ResizingTextArea;
	$: i = 0;
	$: currentFlashcard = flashcards[i];
	$: showingFront = true;
	$: showingNormal = currentFlashcard.flashType === FlashType.NORMAL;
	$: currentFace = showingFront ? currentFlashcard.front : currentFlashcard.back;
	$: currentFaceName = showingFront ? 'front' : 'back';
	$: currentFaceNameCap = showingFront ? 'Front' : 'Back';

	const nextFlashcard = () => {
		i = Math.min(flashcards.length - 1, i + 1);
		showingFront = true;
		// resizeTextAreas();
	};
	const previousFlashcard = () => {
		i = Math.max(0, i - 1);
		showingFront = true;
		// resizeTextAreas();
	};
	const flip = () => {
		showingFront = !showingFront;
		// resizeTextAreas();
	};
	// const resizeTextAreas = () => {
	// 	setTimeout(() => {
	// 		if (resizingTextArea) resizingTextArea.ResizeTextareaHeight();
	// 		if (resizingTextAreaCloze) resizingTextAreaCloze.ResizeTextareaHeight();
	// 	}, 1);
	// };
</script>

<div class="flex flex-col justify-between border-2">
	<div class="flex items-center justify-between p-4">
		<Button size="lg" class="w-full max-w-xs text-xl" on:click={previousFlashcard}>Previous</Button>
		<p class="text-2xl">{i + 1}/{flashcards.length}</p>
		<Button size="lg" class="w-full max-w-xs text-xl" on:click={nextFlashcard}>Next</Button>
	</div>

	<Separator orientation="horizontal" class="my-0 h-[2px]" />

	<Button class="m-4 text-xl" on:click={flip} disabled={!showingNormal}>Flip</Button>

	<Separator orientation="horizontal" class="my-0 h-[2px]" />

	{#if currentFlashcard}
		<div class="space-y-10 p-4">
			<div>
				{#if showingNormal}
					<Label for={currentFaceName}>{currentFaceNameCap}</Label>

					<Separator orientation="horizontal" class="my-0 h-[2px] mb-4" />

					<!--					<ResizingTextArea bind:this={resizingTextArea} id={currentFaceName} placeHolder="Enter {currentFaceName} text here..." bind:value={currentFace}></ResizingTextArea>-->
					<pre class="whitespace-pre-wrap">{currentFace}</pre>

				{:else}
					<Label for="cloze">Cloze</Label>
<!--					<ResizingTextArea bind:this={resizingTextAreaCloze} id="cloze" placeHolder="Enter cloze text here..." bind:value={currentFlashcard.front}></ResizingTextArea>-->
					<pre class="whitespace-pre-wrap">{currentFace}</pre>
				{/if}
			</div>
		</div>
	{/if}
</div>
