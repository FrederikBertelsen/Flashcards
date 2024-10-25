<script lang="ts">
	import { Label as LabelPrimitive } from "bits-ui";
	import { cn } from "$lib/utils.js";

	type $$Props = LabelPrimitive.Props;
	type $$Events = LabelPrimitive.Events;

	let className: $$Props["class"] = undefined;
	export { className as class };
</script>

<div class="mb-1">
	<LabelPrimitive.Root
		class={cn(
		"text-xl font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70",
		className
	)}
		{...$$restProps}
		on:mousedown
	>
		<slot />
	</LabelPrimitive.Root>
</div>

