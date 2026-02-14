using GraphiteUi.Extensions;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GraphiteUi.Components;

public partial class UiSelect<TValue> : UiInputFieldBase<TValue>
{
    private string _selectedText = string.Empty;
    private string? _lastValueAsString;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string ClearText { get; set; } = "Clear value";
    [Parameter] public string NoSuggestionsText { get; set; } = "No suggestions";

    private protected string RootClass => MergeRootClass(SelectStyles.RootClass);

    private bool HasSelectedItem => CurrentValue is not null;
    private string HiddenValue => HasSelectedItem ? CurrentValueAsString ?? string.Empty : string.Empty;
    private bool CanClear => !Disabled && !ReadOnly && HasSelectedItem;
    private string SelectedText => _selectedText;

    private IReadOnlyDictionary<string, object> TriggerAttributes =>
        new Dictionary<string, object>
        {
            ["aria-invalid"] = IsInvalid.ToAttributeValue()
        };

    protected override void OnParametersSet()
    {
        if (string.Equals(_lastValueAsString, CurrentValueAsString, StringComparison.Ordinal))
            return;

        _lastValueAsString = CurrentValueAsString;

        if (string.IsNullOrEmpty(CurrentValueAsString))
        {
            _selectedText = string.Empty;
            return;
        }

        _selectedText = CurrentValueAsString;
    }

    internal void RegisterItem(TValue value, string displayText)
    {
        if (!EqualityComparer<TValue>.Default.Equals(value, CurrentValue))
        {
            return;
        }

        _selectedText = displayText;
    }

    internal void UnregisterItem(TValue value)
    {
        
    }

    internal bool IsSelected(TValue value)
    {
        return HasSelectedItem && EqualityComparer<TValue>.Default.Equals(value, CurrentValue);
    }

    internal string FormatItemValueString(TValue value)
    {
        return FormatValueAsString(value) ?? string.Empty;
    }

    internal void Select(TValue value, string displayText)
    {
        _selectedText = displayText;
        CurrentValue = value;
        _lastValueAsString = CurrentValueAsString;
    }

    private void Clear()
    {
        _selectedText = string.Empty;
        CurrentValue = default!;
        _lastValueAsString = CurrentValueAsString;
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
