using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiBadge : UiComponentBase
{
    [Parameter] public ThemeColor Color { get; set; } = ThemeColor.Primary;
    [Parameter] public Size Size { get; set; } = Size.Small;
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private protected string RootClass => MergeRootClass(BadgeStyles.GetRootClasses(this));

    public UiBadge()
    {
        As = "span";
    }
}
