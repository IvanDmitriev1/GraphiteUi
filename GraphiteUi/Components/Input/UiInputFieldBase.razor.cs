using GraphiteUi.Common;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public abstract partial class UiInputFieldBase<TValue> : UiDebouncedInputBase<TValue>
{
    /// <summary>
    /// Gets or sets the label for the textbox.
    /// </summary>
    [Parameter] public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the placeholder for the textbox.
    /// </summary>
    [Parameter] public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the description for the textbox.
    /// </summary>
    [Parameter] public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the size of the input.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="Size.Medium"/>
    /// </remarks>
    [Parameter] public Size Size { get; set; } = Size.Medium;

    protected string TypeString { get; set; } = "text";
    protected virtual bool IsMultiline => false;
    protected virtual int TextareaRows => 4;
    protected virtual string InputWrapperClass => DefaultUiInputFieldStyles.InputWrapperClass;
    protected virtual string InputElementClass => DefaultUiInputFieldStyles.InputClass;
    protected virtual RenderFragment? TrailingContent => null;
}
