using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiDivider : UiComponentBase
{
    [Parameter] public bool Vertical { get; set; }

    private string RootClass => MergeRootClass(Vertical ? DividerStyles.VerticalClass : DividerStyles.HorizontalClass);
}
