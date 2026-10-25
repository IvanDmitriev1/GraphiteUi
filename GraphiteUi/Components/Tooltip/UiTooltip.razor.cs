using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiTooltip : UiComponentBase
{
    [Parameter] public string? Text { get; set; }
    [Parameter] public Align Align { get; set; } = Align.Center;
    [Parameter] public bool Bottom { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    public UiTooltip()
    {
        As = "span";
    }
}
