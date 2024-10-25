<script>
	import { createEventDispatcher, onMount } from 'svelte';

	export let value = '';
	export let required = true;
	export let maxlength;

	const dispatch = createEventDispatcher();
	let editing = false;
	let original = '';

	onMount(() => {
		original = value;
	});

	function edit() {
		editing = true;
	}

	function save() {
		if (value !== original) {
			dispatch('submit', value);
		}
		original = value;
		editing = false;
	}

	function keydown(event) {
		if (event.key === 'Escape') {
			event.preventDefault();
			value = original;
			editing = false;
		} else if (event.key === 'Enter') {
			event.preventDefault();
			save();
		}
	}

	function focus(element) {
		element.focus();
	}
</script>

{#if editing}
	<input
		class="w-full"
		bind:value
		on:blur={save}
		on:keydown={keydown}
		{required}
		{maxlength}
		use:focus
	/>
{:else}
	<div role="button" tabindex="0" on:click={edit} on:keydown={(e) => e.key === 'Enter' && edit()}>
		{value}
	</div>
<!--	<div >Hello</div>-->

{/if}

<style>
	input {
		border: none;
		background: none;
		font-size: inherit;
		color: inherit;
		font-weight: inherit;
		text-align: inherit;
		box-shadow: none;
	}

	div[role='button'] {
		outline: none;
		cursor: pointer;
	}

	div[role='button']:focus {
		outline: 2px solid #000;
	}
</style>
