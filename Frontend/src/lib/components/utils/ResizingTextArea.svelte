<script lang="ts">
	import { onMount } from 'svelte';
	import { Textarea } from '$lib/components/ui/textarea';

	export const ResizeTextareaHeight = () => {
		// delay by 1 ms
		// setTimeout(() => {
			let textarea = divParent.querySelector('textarea');
			if (!textarea) {
				console.error('Textarea not found');
				return;
			}

			textarea.style.height = 'auto';
			textarea.style.height = textarea.scrollHeight + 'px';
		// }, 100);
	};

	onMount(() => {
		ResizeTextareaHeight();
	});

	let divParent: HTMLDivElement;

	export let value = '';
	export let id = '';
	export let placeHolder = '';

</script>

<div bind:this={divParent}>
	<Textarea
		{id}
		class="resize-none overflow-hidden bg-muted/50 text-[1em]"
		rows={1}
		placeholder={placeHolder}
		on:input={ResizeTextareaHeight}
		on:change={ResizeTextareaHeight}
		bind:value
		maxlength={512}
	/>
</div>
