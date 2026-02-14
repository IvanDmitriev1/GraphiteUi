using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiDrawer : UiComponentBase
{
    private readonly string _toggleId = $"drawer-{Guid.NewGuid():N}";

    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool ShowBackdrop { get; set; } = true;
    [Parameter] public bool CloseOnEscape { get; set; } = true;
    [Parameter] public Align Side { get; set; } = Align.Start;
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? TriggerContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private string ToggleId => _toggleId;

    private string RootClass => MergeRootClass(DrawerStyles.RootClass);
    private string PanelClass => DrawerStyles.GetPanelClass(this);
}
