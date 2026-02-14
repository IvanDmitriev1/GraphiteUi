using GraphiteUi.Components.Bases;
using GraphiteUi.Extensions;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GraphiteUi.Components;

public partial class UiPopup : UiComponentBase
{
    private ElementReference _rootReference;
    private const string TriggerAsValue = "button";
    private const string TriggerModeValue = "toggle";

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Parameter] public RenderFragment? TriggerContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string TriggerAs { get; set; } = "button";

    [Parameter] public string TriggerMode { get; set; } = "toggle";

    [Parameter] public string? TriggerClass { get; set; }

    [Parameter] public IReadOnlyDictionary<string, object>? TriggerAttributes { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool CloseOnOutsideClick { get; set; } = true;

    [Parameter] public bool CloseOnEscape { get; set; } = true;

    [Parameter] public bool ShowBackdrop { get; set; } = true;

    private bool IsButtonTrigger => TriggerAsValue == "button";

    private string RootClassValue { get; set; } = string.Empty;
    private string TriggerClassValue { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
        RootClassValue = MergeRootClass(PopupStyles.RootClass);
        TriggerClassValue = Merge(PopupStyles.TriggerClass, TriggerClass);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return Task.CompletedTask;
        }

        ElementReference = _rootReference;
        return SetUpJsAsync();
    }

    private async Task SetUpJsAsync()
    {
        await using var module = await JsRuntime.LoadModule("js/uiPopup.js");
        await module.InvokeVoidAsync("refreshUiPopupBlazor", _rootReference);
    }
}
