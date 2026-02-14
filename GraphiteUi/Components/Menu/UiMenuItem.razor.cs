using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GraphiteUi.Components;

public partial class UiMenuItem : UiComponentBase
{
    [Parameter] public string? Href { get; set; }
    [Parameter] public string? Target { get; set; }
    [Parameter] public string? Rel { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public RenderFragment? StartContent { get; set; }
    [Parameter] public RenderFragment? EndContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    private string ResolvedAs => string.IsNullOrWhiteSpace(Href) ? "button" : "a";
    private string? ResolvedHref => Disabled || ResolvedAs != "a" ? null : Href;

    private string RootClass => MergeRootClass(MenuStyles.ItemClass);

    private Task OnClickAsync(MouseEventArgs args)
    {
        return Disabled ? Task.CompletedTask : OnClick.InvokeAsync(args);
    }
}
