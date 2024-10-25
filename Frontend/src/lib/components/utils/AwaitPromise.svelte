<script lang="ts">
	import { onMount } from 'svelte';
	import { Skeleton } from '$lib/components/ui/skeleton';

	type T = $$Generic;
	export let promise: Promise<T>;
	export let value: T;
	export let error: Error | null = null;
	export let loading: boolean = true;

	onMount(async () => {
		try {
			value = await promise;
		} catch (err) {
			error = err as Error;
		} finally {
			loading = false;
		}
	});
</script>

{#if loading}
	<div class="flex items-center space-x-4">
		<Skeleton class="h-12 w-12 rounded-full" />
		<div class="space-y-2">
			<Skeleton class="h-4 w-[250px]" />
			<Skeleton class="h-4 w-[200px]" />
		</div>
	</div>
{:else if error}
	<p class="text-2xl text-red-600">{error.message}</p>
{:else}
	<slot />
{/if}