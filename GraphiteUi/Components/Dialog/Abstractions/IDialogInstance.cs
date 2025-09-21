using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public interface IDialogInstance
{
    DialogId Id { get; }
    DialogOptions Options { get; }
    RenderFragment Render { get; }
    ElementReference DialogElementRef { get; }

    Task WaitAsync(CancellationToken ct);
    void Close();
}