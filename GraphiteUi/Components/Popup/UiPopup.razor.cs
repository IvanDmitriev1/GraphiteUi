using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiPopup : UiComponentBase
{
    private const string TriggerAsValue = "button";
    private const string TriggerModeValue = "toggle";

    [Parameter] public RenderFragment? TriggerContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string TriggerAs { get; set; } = "button";

    [Parameter] public string TriggerMode { get; set; } = "toggle";

    [Parameter] public IReadOnlyDictionary<string, object>? TriggerAttributes { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool CloseOnOutsideClick { get; set; } = true;

    [Parameter] public bool CloseOnEscape { get; set; } = true;

    [Parameter] public bool ShowBackdrop { get; set; } = true;

    private bool IsButtonTrigger => TriggerAsValue == "button";

    private string RootClassValue { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
        RootClassValue = MergeRootClass(PopupStyles.RootClass);
    }
}
