using GraphiteUi.Components.Bases;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace GraphiteUi.Components;

/// <summary>
/// A component representing a polymorphic component.
/// </summary>
public sealed class UiComponent : UiComponentBase
{
    /// <summary>
    /// Gets or sets content to be rendered inside the component.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, As);
        builder.AddAttribute(1, "class", Class);
        builder.AddMultipleAttributes(2, AdditionalAttributes);
        builder.AddElementReferenceCapture(3, er => ElementReference = er);
        builder.AddContent(4, ChildContent);
        builder.CloseElement();
    }
}