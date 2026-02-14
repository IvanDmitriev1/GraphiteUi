using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiAlert : UiComponentBase
{ 
    [Parameter] public ThemeColor Color { get; set; } = ThemeColor.Primary;
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? StartContent { get; set; }
    [Parameter] public bool Dismissible { get; set; }
    [Parameter] public bool ShowDismissButton { get; set; } = true;
    [Parameter] public bool Visible { get; set; } = true;
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter] public EventCallback OnDismiss { get; set; }

    private string ResolvedRole => Color == ThemeColor.Danger ? "alert" : "status";
    private protected string RootClass => MergeRootClass(AlertStyles.GetRootClasses(this));
    private protected string IndicatorClass => AlertStyles.GetIndicatorClass(this);

    public UiAlert()
    {
        As = "section";
    }

    private async Task DismissAsync()
    {
        if (!Visible)
            return;

        await VisibleChanged.InvokeAsync(false);
        await OnDismiss.InvokeAsync();
    }
}
