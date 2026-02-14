using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace GraphiteUi.Components;

public partial class UiNumbox<TValue> : UiInputFieldBase<TValue> 
    where TValue : INumber<TValue>
{
    public static readonly bool IsFloatingPoint =
        default(TValue) is double or float or decimal;

    protected override void OnInitialized()
    {
        TypeString = "number";

        var attr = new Dictionary<string, object>(AdditionalAttributes ??
                                                  Enumerable.Empty<KeyValuePair<string, object>>())
        {
            ["inputmode"] = IsFloatingPoint ? "decimal" : "numeric",
            ["step"] = "any"
        };

        AdditionalAttributes = attr;
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (!string.IsNullOrWhiteSpace(value) &&
            (TValue.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) ||
            TValue.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
        )
        {
            validationErrorMessage = null;
            return true;
        }

        result = TValue.Zero;
        validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";
        return false;
    }

    protected override string? FormatValueAsString(TValue? value)
    {
        if (value is null)
            return null;

        return value.ToString(null, CultureInfo.InvariantCulture);
    }
}
