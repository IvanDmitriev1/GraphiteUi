using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;

namespace GraphiteUi.Components;

public partial class UiSkeletonBlock : UiComponentBase
{
    private protected string RootClass => MergeRootClass(SkeletonStyles.BlockClass);

    public UiSkeletonBlock()
    {
        As = "div";
    }
}
