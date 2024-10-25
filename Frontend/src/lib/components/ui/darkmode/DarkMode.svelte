<script lang="ts">
	import { Sun, Moon } from "lucide-svelte";

	import { toggleMode } from "mode-watcher";
	import { Button } from "$lib/components/ui/button";
</script>

<Button on:click={toggleMode} variant="ghost" class="hover:bg-accent/0 hover:text-foreground/60" size="icon">
	<Sun
		class="absolute h-5 w-5 rotate-0 scale-100 transition-all dark:-rotate-90 dark:scale-0"
	/>
	<Moon
		class="absolute h-5 w-5 rotate-90 scale-0 transition-all dark:rotate-0 dark:scale-100"
	/>
	<span class="sr-only">Toggle theme</span>
</Button>