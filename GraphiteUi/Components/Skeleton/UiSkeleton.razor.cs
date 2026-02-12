using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiSkeleton : UiComponentBase
{
    [Parameter, EditorRequired] public bool IsLoading { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? SkeletonContent { get; set; }

    private bool UseCustomSkeleton => IsLoading && SkeletonContent is not null;

    private string ContentClass => IsLoading ? SkeletonStyles.AutoMimicClass : SkeletonStyles.ContentClass;

    private protected string RootClass => MergeRootClass(SkeletonStyles.RootClass);

    public UiSkeleton()
    {
        As = "div";
    }
}
