import { error } from '@sveltejs/kit';

async function MakeRequest(method: string, url: string, body: any, sessionId: string | undefined = undefined): Promise<any> {
	const headers: Record<string, string> = {
		'Content-Type': 'application/json'
	};
	if (sessionId) {
		headers['Authorization'] = `Bearer ${sessionId}`;
	}

	const options: RequestInit = {
		method,
		headers
	};
	if (method !== 'GET') options.body = JSON.stringify(body);

	// console.log(url, options);

	let res = await fetch(url, options);
	if (res.ok || res.status === 422) {
		const text = await res.text();
		return text ? JSON.parse(text) : {}; // Return empty object if response body is empty
	}
	let text = await res.text();

	// console.trace('Failed to fetch', res.status, text);
	if (res.status === 404) {
		return null;
	}

	// console.log('Failed to fetch', res.status, text);
	console.trace('Failed to fetch', res.status, text);

	error(res.status, text);
}

export function HandleCastError<T>(data: any, type: string): T {
	try {
		return data as T;
	} catch (e) {
		throw error(422, `Invalid ${type} format: ` + JSON.stringify(data));
	}
}

export async function Post(url: string, body: any, sessionId: string | undefined = undefined): Promise<any> {
	return MakeRequest('POST', url, body, sessionId);
}
export async function Get(url: string, sessionId: string | undefined = undefined): Promise<any> {
	return MakeRequest('GET', url, {}, sessionId);
}
export async function Put(url: string, body: any, sessionId: string | undefined = undefined): Promise<void> {
	return MakeRequest('PUT', url, body, sessionId);
}
export async function Delete(url: string, sessionId: string | undefined = undefined): Promise<void> {
	return MakeRequest('DELETE', url, {}, sessionId);
}
