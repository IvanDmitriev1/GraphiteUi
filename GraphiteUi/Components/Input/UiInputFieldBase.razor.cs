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

    private readonly string _generatedId = $"graphite-input-{Guid.NewGuid():N}";
    protected string InputId => AdditionalAttributes?.TryGetValue("id", out var id) == true
        ? Convert.ToString(id, System.Globalization.CultureInfo.InvariantCulture) ?? _generatedId
        : _generatedId;
    protected string DescriptionId => InputId + "-description";
    protected string? DescribedBy
    {
        get
        {
            var existing = AdditionalAttributes?.TryGetValue("aria-describedby", out var value) == true
                ? Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)
                : null;
            return string.IsNullOrWhiteSpace(Description) ? existing
                : string.IsNullOrWhiteSpace(existing) ? DescriptionId : existing + " " + DescriptionId;
        }
    }
    protected string? AccessibleLabel => Label ?? GetAttributeText("aria-label");
    protected virtual string? InputMode => GetAttributeText("inputmode");
    protected virtual string? InputStep => GetAttributeText("step");

    protected string? GetAttributeText(string name) => AdditionalAttributes?.TryGetValue(name, out var value) == true
        ? Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)
        : null;

    protected string TypeString { get; set; } = "text";
    protected virtual bool IsMultiline => false;
    protected virtual int TextareaRows => 4;
    protected virtual string InputWrapperClass => DefaultUiInputFieldStyles.GetWrapperClass(Size);
    protected virtual string InputElementClass => DefaultUiInputFieldStyles.InputClass;
    protected virtual RenderFragment? TrailingContent => null;
}
