const constraints = (window.constraints = {
    audio: false,
    video: {
        // 指定分辨率
        width: 640,
        height: 480,
        // 指定刷新率
        frameRate: 30,
    },

});
export async function getCameraFeed(videoElement, dotnet) {
    console.log("initializing camera");
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
