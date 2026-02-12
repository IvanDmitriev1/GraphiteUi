using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GraphiteUi.Components;

public partial class UiButton : UiComponentBase
{
    /// <summary>
    /// Gets or sets content to be rendered inside the button.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets content to be rendered before the label of the button.
    /// </summary>
    [Parameter] public RenderFragment? StartContent { get; set; }

    /// <summary>
    /// Gets or sets content to be rendered after the label of the button.
    /// </summary>
    [Parameter] public RenderFragment? EndContent { get; set; }

    /// <summary>
    /// Gets or sets the type of the button.
    /// </summary>
    /// <remarks>
    /// Default value is <see cref="ButtonType.Button"/>
    /// </remarks>
    [Parameter] public ButtonType Type { get; set; }

    /// <summary>
    /// Gets or sets a color of the button.
    /// </summary>
    /// <remarks>
    /// Default value is <see cref="ThemeColor.Primary"/>
    /// </remarks>
    [Parameter] public ThemeColor Color { get; set; } = ThemeColor.Primary;

    /// <summary>
    /// Gets or sets the size of the button.
    /// </summary>
    /// <remarks>
    /// Default value is <see cref="Size.Medium"/>
    /// </remarks>
    [Parameter] public Size Size { get; set; } = Size.Medium;

    /// <summary>
    /// Gets or sets a value indicating whether the button is disabled.
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the button is full-width.
    /// </summary>
    [Parameter] public bool FullWidth { get; set; }

    /// <summary>
    /// Gets or sets a callback that is fired whenever the button is clicked.
    /// </summary>
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    private protected string RootClass => MergeRootClass(ButtonStyles.GetRootClasses(this));

    public UiButton()
    {
        As = "button";
    }

    private protected virtual Task OnClickAsync(MouseEventArgs args)
    {
        return Disabled ? Task.CompletedTask : OnClick.InvokeAsync(args);
    }
}
