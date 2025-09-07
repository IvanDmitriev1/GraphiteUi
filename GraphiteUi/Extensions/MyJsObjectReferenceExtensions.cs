using Microsoft.JSInterop;

namespace GraphiteUi.Extensions;

internal static class MyJsObjectReferenceExtensions
{
    public static async ValueTask HandleDispose(this IJSObjectReference? reference)
    {
        if (reference is null)
            return;

        try
        {
            await reference.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
        }
    }
}