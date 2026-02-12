using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiAlert : UiComponentBase
{
    private bool _isVisible = true;
    private bool _hasVisibleParameter;
    private bool _lastVisibleParameter = true;

    [Parameter] public ThemeColor Color { get; set; } = ThemeColor.Primary;
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? StartContent { get; set; }
    [Parameter] public bool Dismissible { get; set; }
    [Parameter] public bool ShowDismissButton { get; set; } = true;
    [Parameter] public bool AutoHideOnDismiss { get; set; } = true;
    [Parameter] public string? DismissButtonDataSlot { get; set; }
    [Parameter] public bool Visible { get; set; } = true;
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter] public EventCallback OnDismiss { get; set; }
    [Parameter] public string? Role { get; set; }

    private string ResolvedRole =>
        !string.IsNullOrWhiteSpace(Role)
            ? Role
            : Color == ThemeColor.Danger ? "alert" : "status";

    private protected string RootClass => MergeRootClass(AlertStyles.GetRootClasses(this));

    private protected string IndicatorClass => AlertStyles.GetIndicatorClass(this);

    public UiAlert()
    {
        As = "section";
    }

    protected override void OnParametersSet()
    {
        if (!_hasVisibleParameter)
        {
            _isVisible = Visible;
            _lastVisibleParameter = Visible;
            _hasVisibleParameter = true;
            return;
        }

        if (Visible != _lastVisibleParameter)
        {
            _isVisible = Visible;
            _lastVisibleParameter = Visible;
        }
    }

    private async Task DismissAsync()
    {
        if (!_isVisible)
        {
            return;
        }

        if (AutoHideOnDismiss)
        {
            _isVisible = false;
        }

        await VisibleChanged.InvokeAsync(false);
        await OnDismiss.InvokeAsync();
    }
}
