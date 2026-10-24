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

    [Parameter] public string? DataUi { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, As);
        builder.AddAttribute(1, "class", Class);
        builder.AddMultipleAttributes(2, AdditionalAttributes);
        if (DataUi is not null)
        {
            builder.AddAttribute(3, "data-ui", DataUi);
        }
        builder.AddElementReferenceCapture(4, er => ElementReference = er);
        builder.AddContent(5, ChildContent);
        builder.CloseElement();
    }
}
