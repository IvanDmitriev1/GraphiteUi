using GraphiteUi.Common;
using GraphiteUi.Extensions;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GraphiteUi.Components;

public abstract partial class UiInputFieldBase<TValue> : UiDebouncedInputBase<TValue>
{
    /// <summary>
    /// Gets or sets the label for the textbox.
    /// </summary>
    [Parameter] public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the placeholder for the textbox.
    /// </summary>
    [Parameter] public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the description for the textbox.
    /// </summary>
    [Parameter] public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the size of the input.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="Size.Medium"/>
    /// </remarks>
    [Parameter] public Size Size { get; set; } = Size.Medium;

    [Inject] protected IJSRuntime JsRuntime { get; set; } = null!;

    protected string TypeString { get; set; } = "text";

    private readonly IUiInputFieldStyles _styles = DefaultUiInputFieldStyles.Instance;
    private ElementReference _wrapperReference;

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return Task.CompletedTask;
        }

        return SetUpJs();
    }

    private async Task SetUpJs()
    {
        await using var module = await JsRuntime.LoadModule("js/uiInputField.js");
        await module.InvokeVoidAsync("refreshUiInputFieldsBlazor", _wrapperReference);
    }
}