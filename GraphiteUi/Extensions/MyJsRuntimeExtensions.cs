using Microsoft.JSInterop;

namespace GraphiteUi.Extensions;

internal static class MyJsRuntimeExtensions
{
    public static ValueTask<IJSObjectReference> LoadModule(this IJSRuntime jsRuntime, string fileName) =>
        jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/GraphiteUi/{fileName}");
}