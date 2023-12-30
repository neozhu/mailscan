const constraints = (window.constraints = {
    audio: false,
    video: {
        // 指定分辨率
        width: 1280,
        height: 720,
        // 指定刷新率
        frameRate: 30,
    },

});
export async function getCameraFeed(videoElement, dotnet) {
    console.log("initializing camera", dotnet);
    try {
        let devices = await navigator.mediaDevices.enumerateDevices();
        let cameras = devices.filter(device => device.kind === 'videoinput');
        let cameraId;
        console.log("cameras: ", cameras.length);
        if (cameras.length > 1) {
            cameraId = cameras[cameras.length - 1].deviceId
        }
        let stream = await navigator.mediaDevices.getUserMedia({
            video: {
                facingMode: 'environment',
                deviceId: { exact: cameraId },
                width: 1280,
                height: 720,
            },
            audio: false
        });
        handleSuccess(stream, videoElement);
        dotnet.invokeMethodAsync("OnCameraStreaming");
    } catch (error) {
        handleError(error, dotnet);
    }
}
export async function vidOff(videoElement) {
    console.log('vidOff')
    window.vid.pause();
    window.vid.src = "";
    window.vid.srcObject = null;
    window.stream.getTracks()[0].stop();
}
function handleSuccess(stream, video) {
    const videoTracks = stream.getVideoTracks();
    console.log("Got stream with constraints:", constraints);
    console.log(`Using video device: ${videoTracks[0].label}`);
    window.stream = stream;
    window.vid = video;
    video.srcObject = stream;
    video.play();
}
function handleError(error, dotnet) {
    if (error.name === "ConstraintNotSatisfiedError") {
        const v = constraints.video;
        errorMsg(`resolution`, error, dotnet);
    } else if (error.name === "") {
        errorMsg("permissions", error, dotnet);
    }
    errorMsg(`getUserMedia error: ${error.name}`, error, dotnet);
}
function errorMsg(msg, error, dotnet) {
    if (typeof error !== "undefined") {
        console.error(error);
    }
    dotnet.invokeMethodAsync("OnCameraStreamingError", msg);
}

export async function capture(videoElement, dotnet) {
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
    context.drawImage(videoElement, left, top, cropWidth, cropHeight, 0, 0, canvasElement.width, canvasElement.height);

    // 获取图片数据
    const imageDataURL = canvasElement.toDataURL('image/png');

    // 这里可以将 imageDataURL 保存或发送到服务器
    //console.log(dotnet, imageDataURL)
    dotnet.invokeMethodAsync("OnCaptureCallback", imageDataURL);
}
