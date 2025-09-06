using System.Diagnostics.CodeAnalysis;
using GraphiteUi.Common;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiTextbox : UiInputFieldBase<string>
{
    /// <summary>
    /// Gets or sets the input type of the textbox.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="InputType.Text"/>
    /// </remarks>
    [Parameter] public InputType Type { get; set; } = InputType.Text;


    protected override void OnInitialized()
    {
        InputTypeString = Type.ToHtmlValue();
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (value == null)
        {
            result = null;
            validationErrorMessage = "value is null";
            return false;
        }

        result = value;
        validationErrorMessage = null;
        return true;
    }
}