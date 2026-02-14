using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Extensions;
using GraphiteUi.Styles;
using GraphiteUi.Utilities;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiToast : UiComponentBase
{
    [Parameter, EditorRequired] public int Id { get; set; }
    [Parameter, EditorRequired] public ToastOptions Options { get; set; } = ToastOptions.Default;
    [Parameter] public bool Sticky { get; set; }
    [Parameter] public EventCallback OnDismiss { get; set; }

    private ThemeColor Color => Options.Color;
    private string? Title => Options.Title;
    private int DurationMs => Math.Max(0, Options.DurationMs ?? 0);
    private string AriaLive => Color == ThemeColor.Danger ? "assertive" : "polite";
    private protected string RootClass =>
        RootClassMergeCache.GetOrAdd(TwMerge, ToastStyles.ItemClass, Options.Class);
}
