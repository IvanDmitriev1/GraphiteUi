using GraphiteUi.Components.Bases;
using GraphiteUi.Extensions;
using GraphiteUi.Styles;
using GraphiteUi.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GraphiteUi.Components;

public partial class UiPopup : UiComponentBase
{
    private ElementReference _rootReference;
    private string _rootClassValue = PopupStyles.RootClass;
    private string _triggerClassValue = PopupStyles.TriggerClass;
    private string _contentClassValue = PopupStyles.ContentClass;
    private string _backdropClassValue = PopupStyles.BackdropClass;
    private string _triggerAsValue = "button";
    private string _triggerModeValue = "toggle";

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Parameter] public RenderFragment? TriggerContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string TriggerAs { get; set; } = "button";

    [Parameter] public string TriggerMode { get; set; } = "toggle";

    [Parameter] public string? TriggerClass { get; set; }

    [Parameter] public string? ContentClass { get; set; }

    [Parameter] public string? BackdropClass { get; set; }

    [Parameter] public IReadOnlyDictionary<string, object>? TriggerAttributes { get; set; }

    [Parameter] public IReadOnlyDictionary<string, object>? ContentAttributes { get; set; }

    [Parameter] public IReadOnlyDictionary<string, object>? BackdropAttributes { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool CloseOnOutsideClick { get; set; } = true;

    [Parameter] public bool CloseOnEscape { get; set; } = true;

    [Parameter] public bool ShowBackdrop { get; set; } = true;

    private bool IsButtonTrigger => string.Equals(_triggerAsValue, "button", StringComparison.OrdinalIgnoreCase);

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        As = parameters.GetValueOrDefault(nameof(As), "div");
        Class = parameters.GetValueOrDefault<string?>(nameof(Class));
        AdditionalAttributes = parameters.GetValueOrDefault<IReadOnlyDictionary<string, object>?>(nameof(AdditionalAttributes));

        TriggerContent = parameters.GetValueOrDefault<RenderFragment?>(nameof(TriggerContent));
        ChildContent = parameters.GetValueOrDefault<RenderFragment?>(nameof(ChildContent)); TriggerAs = parameters.GetValueOrDefault(nameof(TriggerAs), "button");

        TriggerMode = parameters.GetValueOrDefault(nameof(TriggerMode), "toggle");

        TriggerClass = parameters.GetValueOrDefault<string?>(nameof(TriggerClass));
        ContentClass = parameters.GetValueOrDefault<string?>(nameof(ContentClass));
        BackdropClass = parameters.GetValueOrDefault<string?>(nameof(BackdropClass));
        TriggerAttributes = parameters.GetValueOrDefault<IReadOnlyDictionary<string, object>?>(nameof(TriggerAttributes));

        ContentAttributes = parameters.GetValueOrDefault<IReadOnlyDictionary<string, object>?>(nameof(ContentAttributes));
        BackdropAttributes = parameters.GetValueOrDefault<IReadOnlyDictionary<string, object>?>(nameof(BackdropAttributes));

        Disabled = parameters.TryGetValue(nameof(Disabled), out bool disabled) && disabled;
        CloseOnOutsideClick = !parameters.TryGetValue(nameof(CloseOnOutsideClick), out bool closeOnOutsideClick) || closeOnOutsideClick;
        CloseOnEscape = !parameters.TryGetValue(nameof(CloseOnEscape), out bool closeOnEscape) || closeOnEscape;
        ShowBackdrop = !parameters.TryGetValue(nameof(ShowBackdrop), out bool showBackdrop) || showBackdrop;

        _triggerAsValue = string.IsNullOrWhiteSpace(TriggerAs) ? "button" : TriggerAs;
        _triggerModeValue = string.IsNullOrWhiteSpace(TriggerMode) ? "toggle" : TriggerMode;

        _rootClassValue = RootClassMergeCache.GetOrAdd(TwMerge, PopupStyles.RootClass, Class);
        _triggerClassValue = RootClassMergeCache.GetOrAdd(TwMerge, PopupStyles.TriggerClass, TriggerClass);
        _contentClassValue = RootClassMergeCache.GetOrAdd(TwMerge, PopupStyles.ContentClass, ContentClass);
        _backdropClassValue = RootClassMergeCache.GetOrAdd(TwMerge, PopupStyles.BackdropClass, BackdropClass);

        await base.SetParametersAsync(ParameterView.Empty);
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
