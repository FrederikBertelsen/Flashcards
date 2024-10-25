export function generateClozeVariants(input: string): string[] {
	const regex = /(\{([1-9]\d*):([^}]*)})/g;
	// const regex = /(\{\{(c[1-9]\d*)::([^}]*)}})/g;
	const matches = [...input.matchAll(regex)];

	// Group cloze deletions by their numbers
	const groups: { [key: string]: string[] } = {};
	matches.forEach(match => {
		const [fullMatch, , number, text] = match;
		if (!groups[number])
			groups[number] = [];
		groups[number].push(fullMatch);
	});

	// Generate variants of the input string by replacing each group with [___]
	const variants: string[] = [];

	for (const number in groups) {
		let variant = input;
		groups[number].forEach(cloze => {
			variant = variant.replace(cloze, '[___]');
		});

		// Remove the cloze syntax from the other cloze deletions groups
		variant = variant.replace(/\{[1-9]\d*:([^}]*)}/g, '$1');
		// variant = variant.replace(/\{\{c[1-9]\d*::([^}]*)}}/g, '$1');

		variants.push(variant);
	}

	return variants;
}