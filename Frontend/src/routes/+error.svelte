<script lang="ts">

	import { page } from '$app/stores';
</script>


<div class="m-auto text-center">
	<h1 class="text-destructive">{$page.status}</h1>
	{#if $page.error}
		<p class="text-xl">{$page.error.message}</p>
	{:else}
		<h1>Unknown error?</h1>
	{/if}
</div>