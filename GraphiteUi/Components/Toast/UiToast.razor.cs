using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiToast : UiComponentBase
{
    [Parameter, EditorRequired] public int Id { get; set; }
    [Parameter] public ThemeColor Color { get; set; } = ThemeColor.Primary;
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Message { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Dismissible { get; set; } = true;
    [Parameter] public bool Sticky { get; set; }
    [Parameter] public bool PauseOnHover { get; set; } = true;
    [Parameter] public int DurationMs { get; set; } = 6000;
    [Parameter] public bool Closing { get; set; }
    [Parameter] public EventCallback OnDismiss { get; set; }

    private string State => Closing ? "closing" : "open";

    private bool IsDangerTone => Color == ThemeColor.Danger;

    private bool HasMessage => !string.IsNullOrWhiteSpace(Message);

    private string AriaRole => IsDangerTone ? "alert" : "status";

    private string AriaLive => IsDangerTone ? "assertive" : "polite";

    private protected string RootClass => MergeRootClass(ToastStyles.ItemClass);

    private Task DismissAsync() => OnDismiss.InvokeAsync();
}
