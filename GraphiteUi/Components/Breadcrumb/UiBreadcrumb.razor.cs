using GraphiteUi.Components.Bases;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiBreadcrumb : UiComponentBase
{
    [Parameter] public string AriaLabel { get; set; } = "Breadcrumb";
    [Parameter] public RenderFragment? ChildContent { get; set; }

    public UiBreadcrumb()
    {
        As = "nav";
    }
}
