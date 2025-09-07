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
        result = TValue.Zero;
        validationErrorMessage = string.Empty;

        if (value is null)
        {
            return false;
        }

        return TValue.TryParse(value, CultureInfo.CurrentCulture, out result);
    }
}