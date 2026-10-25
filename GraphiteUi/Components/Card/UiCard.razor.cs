using GraphiteUi.Components.Bases;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiCard : UiComponentBase
{
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? HeaderContent { get; set; }
    [Parameter] public RenderFragment? FooterContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    public UiCard()
    {
        As = "section";
    }
}
