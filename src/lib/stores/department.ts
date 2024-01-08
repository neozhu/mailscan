import type { Department } from '$lib/type'
import { writable } from 'svelte/store'

export const departments = writable<Department[] | null>(null);