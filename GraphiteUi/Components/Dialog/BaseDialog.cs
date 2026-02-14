using GraphiteUi.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GraphiteUi.Components;

public abstract class BaseDialog : ComponentBase
{
    [Inject] protected IJSRuntime JsRuntime { get; set; } = null!;
    [CascadingParameter] protected IDialogInstance DialogInstance { get; set; } = null!;

    protected void Close()
    {
        DialogInstance.Close();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            return OpenDialog();

        return Task.CompletedTask;
    }

    private async Task OpenDialog()
    {
        await using var module = await JsRuntime.LoadModule("js/dialogInterop.js");

        await module.InvokeVoidAsync(
            "openModalDialog",
            DialogInstance.DialogElementRef,
            DialogInstance.Options.DismissOnOverlayClick,
            DialogInstance.Options.CloseOnEscape);
    }
}

public abstract class BaseDialog<TResult> : BaseDialog
{
    protected new IDialogInstance<TResult> DialogInstance => (IDialogInstance<TResult>)base.DialogInstance;

    protected void SetResult(TResult result)
    {
        DialogInstance.SetResult(result);
    }
}
