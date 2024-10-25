<script lang="ts">
	import { Button } from "$lib/components/ui/button/index";
	// DO NOT REMOVE
	import * as Card from "$lib/components/ui/card/index";
	// DO NOT REMOVE
	import { ROUTES } from '$lib/Routes';
</script>
<Card.Root class="mx-auto max-w-sm">
	<Card.Header>
		<Card.Title class="text-2xl">Login</Card.Title>
		<Card.Description>You can only log in with a Google account</Card.Description>
	</Card.Header>
	<Card.Content>
		<div class="grid gap-4">
			<Button href="{ROUTES.GOOGLE_OAUTH}" class="w-full">Login with Google</Button>
		</div>
	</Card.Content>
</Card.Root>