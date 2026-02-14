using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using GraphiteUi.Utilities;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiMenu : UiComponentBase
{
    [Parameter] public RenderFragment? TriggerContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string TriggerAs { get; set; } = "button";
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

    private string RootClass => MergeRootClass(MenuStyles.RootClass);

    private string TriggerClassValue =>
        RootClassMergeCache.GetOrAdd(TwMerge, MenuStyles.TriggerClass, TriggerClass);

    private string ContentClassValue =>
        RootClassMergeCache.GetOrAdd(TwMerge, MenuStyles.ContentClass, ContentClass);

    private string BackdropClassValue =>
        RootClassMergeCache.GetOrAdd(TwMerge, PopupStyles.BackdropClass, BackdropClass);
}
