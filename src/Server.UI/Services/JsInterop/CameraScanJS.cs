using CleanArchitecture.Blazor.Server.UI.Components.Camera;
using Microsoft.JSInterop;

namespace CleanArchitecture.Blazor.Server.UI.Services.JsInterop;

public partial class CameraScanJS
{
    private readonly IJSRuntime _jsRuntime;
  
    public CameraScanJS(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    public async Task<ValueTask> Init(ElementReference videoElment, DotNetObjectReference<CameraScan>? dotNetObject)
    {
        var jsmodule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/cameraScan.js");
        return jsmodule.InvokeVoidAsync("getCameraFeed", videoElment, dotNetObject);
    }
    public async Task<ValueTask> Capture(ElementReference videoElment,DotNetObjectReference<CameraScan>? dotNetObject)
    {
        var jsmodule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/cameraScan.js");
        return jsmodule.InvokeVoidAsync("capture", videoElment, dotNetObject);
    }
    public async Task<ValueTask> CameraOff()
    {
        var jsmodule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/cameraScan.js");
        return jsmodule.InvokeVoidAsync("vidOff");
    }
}
