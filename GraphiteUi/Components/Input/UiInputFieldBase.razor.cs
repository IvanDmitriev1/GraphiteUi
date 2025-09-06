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

    protected string InputTypeString { get; set; } = null!;

    protected bool IsInvalid => !EditContext.IsValid(FieldIdentifier);


    private readonly IUiInputFieldStyles _styles = DefaultUiInputFieldStyles.Instance;
}