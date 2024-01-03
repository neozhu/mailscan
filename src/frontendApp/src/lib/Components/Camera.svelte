<script lang="ts">
	import { onMount } from 'svelte';
	import { enhance, applyAction } from '$app/forms';
	import { getModalStore, SlideToggle, getToastStore } from '@skeletonlabs/skeleton';
	import type {
		ToastSettings,
		ModalSettings,
		ModalComponent,
		ModalStore,
		ToastStore
	} from '@skeletonlabs/skeleton';
	import { Icon, Camera, ArrowPathRoundedSquare, VideoCameraSlash } from 'svelte-hero-icons';
	import { Blob } from 'buffer';

	const modalStore: ModalStore = getModalStore();
	const toastStore: ToastStore = getToastStore();
	let videoElement: HTMLVideoElement;
	let fileElement: HTMLInputElement;
	let form: HTMLFormElement;
	let stream: MediaStream;
	let started: boolean = false;
	let useFrontCamera: boolean = false;
	let multipleCamerasAvailable: boolean = false;
	let processing: boolean = false;
	let screenshotDataURL: string;
	onMount(async () => {
		const devices = await navigator.mediaDevices.enumerateDevices();
		const videoInputs = devices.filter((device) => device.kind === 'videoinput');
		multipleCamerasAvailable = videoInputs.length > 1;
	});

	async function startCamera() {
		try {
			stream = await navigator.mediaDevices.getUserMedia({
				video: {
					width: { ideal: 1920 }, // 期望的宽度
					height: { ideal: 1080 }, // 期望的高度
					facingMode: useFrontCamera ? 'user' : 'environment'
				},
				audio: false
			});
			videoElement.srcObject = stream;
			started = true;
			const toast: ToastSettings = {
				message: 'start working',
				timeout: 2000,
				autohide: true,
				background: 'variant-filled-success',
			};
			toastStore.trigger(toast);
		} catch (err) {
			console.error('Error accessing the camera:', err);
		}
	}
	async function toggleCamera() {
		useFrontCamera = !useFrontCamera;
		if (started) {
			stopCamera();
			await startCamera();
		}
	}

	function stopCamera() {
		if (stream) {
			let tracks = stream.getTracks();
			tracks.forEach((track) => track.stop());
			videoElement.srcObject = null;
			started = false;

			const toast: ToastSettings = {
				message: 'closed camera',
				timeout: 2000,
				autohide: true,
				background: 'variant-filled-surface',
			};
			toastStore.trigger(toast);
		}
	}

	function captureCamera(): Promise<Blob> {
		return new Promise((resolve, reject) => {
			const canvasElement = document.createElement('canvas');
			const context = canvasElement.getContext('2d');
			// 计算缩放比例
			const scaleX = videoElement.clientWidth / videoElement.videoWidth;
			const scaleY = videoElement.clientHeight / videoElement.videoHeight;
			const scale = Math.max(scaleX, scaleY);

			// 计算裁剪区域
			const cropWidth = videoElement.clientWidth / scale;
			const cropHeight = videoElement.clientHeight / scale;
			const left = (videoElement.videoWidth - cropWidth) / 2;
			const top = (videoElement.videoHeight - cropHeight) / 2;

			// 设置canvas尺寸与video元素一致
			canvasElement.width = videoElement.clientWidth;
			canvasElement.height = videoElement.clientHeight;

			// 从video中截取与显示尺寸相匹配的部分
			context?.drawImage(
				videoElement,
				left,
				top,
				cropWidth,
				cropHeight,
				0,
				0,
				canvasElement.width,
				canvasElement.height
			);
			screenshotDataURL = canvasElement.toDataURL('image/png');
			// 获取图片数据
			canvasElement.toBlob((blob) => {
				if (blob && blob instanceof Blob) {
					resolve(blob);
				} else {
					reject(new Error('Failed to extract blob from canvas'));
				}
			}, 'image/png');
		});
	}
	function showResult() {
		processing = false
		const modal: ModalSettings = {
			type: 'alert',
			// Data
			title: 'capture camera image',
			image: screenshotDataURL
		};
		modalStore.trigger(modal);
 
	}
	async function handleCaptureClick() {
		if (!started) return;
		processing = true;
		const imageBlob = await captureCamera();
		const file = new File([imageBlob] as BlobPart[], 'image.png', { type: 'image/png' });
		const dataTransfer = new DataTransfer();
		dataTransfer.items.add(file);
		fileElement.files = dataTransfer.files;
		form.requestSubmit();
	}
	async function handleSubmit(event: SubmitEvent & { currentTarget: HTMLFormElement }) {
		event.preventDefault();
	}
</script>

<header
	class="relative w-screen flex items-center justify-center h-screen overflow-hidden"
	class:blur={processing}
	class:md:blur-l={processing}
>
	{#if started == false}
		<div class="relative z-30 p-5 text-2xl bg-opacity-50">
			<button
				type="button"
				class="btn btn-xl variant-filled-surface shadow-lg"
				on:click={startCamera}>Start</button
			>
		</div>
	{:else}
		<div class="fixed flex justify-end w-full top-10 z-20 py-10 gap-4 px-10">
			<SlideToggle
				name="slider-label"
				active="bg-primary-500"
				size="sm"
				checked={started}
				on:change={stopCamera}
			></SlideToggle>
		</div>
	{/if}
	<video
		bind:this={videoElement}
		autoplay
		class="absolute object-cover z-10 w-auto w-screen h-screen max-w-none"
	>
		<track kind="captions" srclang="en" label="English" />
	</video>
	<div class="fixed flex justify-center w-full bottom-0 z-10 py-10 gap-4">
		<form
			bind:this={form}
			enctype="multipart/form-data"
			use:enhance={({ formElement, formData, action, cancel }) => {
				return async ({ result }) => {
					if (result.type === 'success') {
						console.log(result);
						await applyAction(result);
						showResult();
					}
				};
			}}
			method="post"
			action="?/process"
			on:submit|preventDefault={handleSubmit}
		>
			<input type="file" style="display: none;" name="image" bind:this={fileElement} />
			<button
				on:click={handleCaptureClick}
				disabled={!started}
				type="button"
				class="btn-icon space-x-3 bg-gradient-to-br variant-gradient-tertiary-primary btn-icon-xl variant-filled shadow-lg"
				><Icon src={Camera} size="32"></Icon></button
			>
		</form>
		{#if multipleCamerasAvailable}
			<button
				type="button"
				class="btn-icon space-x-3 bg-gradient-to-br variant-gradient-tertiary-primary btn-icon-xl variant-filled"
				on:click={toggleCamera}><Icon src={ArrowPathRoundedSquare} size="32"></Icon></button
			>
		{/if}
	</div>
</header>
