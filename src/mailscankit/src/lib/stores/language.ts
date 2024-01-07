import { writable } from 'svelte/store'

export const defaultLanguage = writable<string>('en-US');
