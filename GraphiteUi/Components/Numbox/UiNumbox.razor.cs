using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace GraphiteUi.Components;

public partial class UiNumbox<TValue> : UiInputFieldBase<TValue> 
    where TValue : INumber<TValue>
{
    public static readonly bool IsFloatingPoint =
        default(TValue) is double or float or decimal;

    protected override string? InputMode => AdditionalAttributes?.TryGetValue("inputmode", out var value) == true
        ? Convert.ToString(value, CultureInfo.InvariantCulture)
        : IsFloatingPoint ? "decimal" : "numeric";
    protected override string? InputStep => AdditionalAttributes?.TryGetValue("step", out var value) == true
        ? Convert.ToString(value, CultureInfo.InvariantCulture)
        : IsFloatingPoint ? "any" : "1";

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        TypeString = "number";
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (!string.IsNullOrWhiteSpace(value) &&
            (TValue.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result) ||
            TValue.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out result))
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
