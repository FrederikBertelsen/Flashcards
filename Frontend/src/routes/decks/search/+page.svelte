<script lang="ts">
	import { goto } from '$app/navigation';
	import { Input } from '$lib/components/ui/input';
	import { Button } from '$lib/components/ui/button';
	import { ROUTES } from '$lib/Routes';

	const search = () => {
		searchTerm = searchTerm.trim();
		if (searchTerm === '') return;

		const searchUrl = ROUTES.DECK_SEARCH(searchTerm);
		window.location.href = searchUrl;
		searchTerm = '';
	};

	const handleKeyDown = (event: KeyboardEvent) => {
		if (event.key === 'Enter') {
			search();
		}
	};

	let searchTerm: string = '';
</script>

<h1>Search</h1>
<form class="py-10">
	<div class="flex space-x-5">
		<Input
			type="text"
			id="search-term"
			placeholder="Search..."
			required
			bind:value={searchTerm}
			on:keypress={handleKeyDown}
		/>
		<Button type="submit" on:click={search}>Search</Button>
	</div>
</form>