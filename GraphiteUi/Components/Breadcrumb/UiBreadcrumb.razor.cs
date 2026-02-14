using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiBreadcrumb : UiComponentBase
{
    [Parameter] public string AriaLabel { get; set; } = "Breadcrumb";
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private protected string RootClass => MergeRootClass(BreadcrumbStyles.RootClass);

    public UiBreadcrumb()
    {
        As = "nav";
    }
}
