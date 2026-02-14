using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiTooltip : UiComponentBase
{
    [Parameter] public string? Text { get; set; }
    [Parameter] public Align Align { get; set; } = Align.Center;
    [Parameter] public bool Bottom { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private string RootClass => MergeRootClass(TooltipStyles.RootClass);
    private string BubbleClass => TooltipStyles.GetBubbleClass(this);

    public UiTooltip()
    {
        As = "span";
    }
}
