<script lang="ts">
	import '../styles/global.css';
	import Navbar from '$lib/components/mine/Navbar.svelte';
	import { ModeWatcher } from 'mode-watcher';
	import '@fontsource-variable/inter';
	import type { PageData } from './$types';
	import type { User } from '$lib/models/User';

	export let data: PageData;
	let user = data.user as User | null;


	import { onMount } from "svelte";
	import { Progress } from "$lib/components/ui/progress/index.js";

	// let value = 13;
	// onMount(() => {
	// 	const timer = setTimeout(() => (value = 66), 500);
	// 	return () => clearTimeout(timer);
	// });
</script>

<svelte:head>
	<title>FlashyFlashcards</title>
</svelte:head>

<ModeWatcher />
<div class="flex h-screen w-screen">
<!--	<Progress {value} max={100} class="w-[60%]" />-->
	<Navbar {user}>
		<div class="w-full flex flex-col items-center">
			<main class="w-full max-w-[1536px] px-4 md:px-8 lg:px-16">
				<slot />
			</main>
		</div>
	</Navbar>
</div>
