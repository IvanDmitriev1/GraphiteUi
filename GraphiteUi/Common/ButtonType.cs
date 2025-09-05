using GraphiteUi.Components;

namespace GraphiteUi.Common;

/// <summary>
/// Specifies the type of the <see cref="UiButton"/>.
/// </summary>
public enum ButtonType
{
    /// <summary>
    /// A standard button with no default behavior.
    /// </summary>
    Button,

    /// <summary>
    /// A button that submits form data.
    /// </summary>
    Submit,

    /// <summary>
    /// A button that resets all form fields to their initial values.
    /// </summary>
    Reset
}

public static class ButtonTypeExtensions
{
    public static string ToHtmlValue(this ButtonType buttonType) => buttonType switch
    {
        ButtonType.Button => "button",
        ButtonType.Submit => "submit",
        _ => throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null)
    };
}