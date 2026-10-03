using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiPopup : UiComponentBase
{

    [Parameter] public RenderFragment? TriggerContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string TriggerAs { get; set; } = "button";

    [Parameter] public string TriggerMode { get; set; } = "toggle";

    [Parameter] public IReadOnlyDictionary<string, object>? TriggerAttributes { get; set; }

    [Parameter] public string? TriggerClass { get; set; }
    [Parameter] public string? ContentClass { get; set; }
    [Parameter] public string? BackdropClass { get; set; }
    [Parameter] public IReadOnlyDictionary<string, object>? ContentAttributes { get; set; }
    [Parameter] public IReadOnlyDictionary<string, object>? BackdropAttributes { get; set; }

    private string TriggerClassValue => Merge(PopupStyles.TriggerClass, TriggerClass);
    private string ContentClassValue => Merge(PopupStyles.ContentClass, ContentClass);
    private string BackdropClassValue => Merge(PopupStyles.BackdropClass, BackdropClass);

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool CloseOnOutsideClick { get; set; } = true;

    [Parameter] public bool CloseOnEscape { get; set; } = true;

    [Parameter] public bool ShowBackdrop { get; set; } = true;

    private bool IsButtonTrigger => string.Equals(TriggerAs, "button", StringComparison.OrdinalIgnoreCase);

    private string RootClassValue { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
        RootClassValue = MergeRootClass(PopupStyles.RootClass);
    }
}
