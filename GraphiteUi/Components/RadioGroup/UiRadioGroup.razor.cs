using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiRadioGroup<TValue> : UiInputBase<TValue>
{ 
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Name { get; set; }

    private string FormFieldName
    {
        get => !string.IsNullOrWhiteSpace(NameAttributeValue)
            ? NameAttributeValue!
            : string.IsNullOrWhiteSpace(Name)
                ? field
                : Name;
    } = $"radio-{Guid.NewGuid():N}";

    internal string InputGroupName => $"{FormFieldName}__items";
    internal string FormFieldValue => CurrentValueAsString ?? string.Empty;
    private protected string RootClass => MergeRootClass(RadioStyles.GroupClass);

    internal bool IsSelected(TValue value) =>
        EqualityComparer<TValue>.Default.Equals(value, CurrentValue);

    internal void Select(TValue value)
    {
        if (Disabled || ReadOnly)
        {
            return;
        }

        CurrentValue = value;
    }

    protected override bool TryParseValueFromString(string? value,
        [MaybeNullWhen(false)] out TValue result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out TValue? parsedValue))
        {
            result = parsedValue!;
            validationErrorMessage = null;
            return true;
        }

        result = default;
        validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";
        return false;
    }
}
