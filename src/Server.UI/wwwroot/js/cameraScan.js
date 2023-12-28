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
        let stream = await navigator.mediaDevices.getUserMedia(constraints);
        handleSuccess(stream, videoElement);
        dotnet.invokeMethodAsync("OnCameraStreaming");
    } catch (error) {
        handleError(error, dotnet);
    }
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
    canvasElement.width = videoElement.videoWidth;
    canvasElement.height = videoElement.videoHeight;
    context.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);
    // 获取图片数据
    const imageDataURL = canvasElement.toDataURL('image/png');
    // 这里可以将 imageDataURL 保存或发送到服务器
    //console.log(dotnet, imageDataURL)
    dotnet.invokeMethodAsync("OnCaptureCallback", imageDataURL);
}
