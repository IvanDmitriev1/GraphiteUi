using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;

namespace GraphiteUi.Components;

public partial class UiSkeletonLine : UiComponentBase
{
    private protected string RootClass => MergeRootClass(SkeletonStyles.LineClass);

    public UiSkeletonLine()
    {
        As = "div";
    }
}
