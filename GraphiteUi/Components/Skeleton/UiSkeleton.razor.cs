using GraphiteUi.Components.Bases;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiSkeleton : UiComponentBase
{
    [Parameter, EditorRequired] public bool IsLoading { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? SkeletonContent { get; set; }

    private bool UseCustomSkeleton => IsLoading && SkeletonContent is not null;

    private bool UseAutoMimic => IsLoading && SkeletonContent is null;

    public UiSkeleton()
    {
        As = "div";
    }
}
