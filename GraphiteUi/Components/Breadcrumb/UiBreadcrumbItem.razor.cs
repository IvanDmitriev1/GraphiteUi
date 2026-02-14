using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiBreadcrumbItem
{
    [Parameter] public string? Href { get; set; }
    [Parameter] public string? Target { get; set; }
    [Parameter] public string? Rel { get; set; }
    [Parameter] public bool Current { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
