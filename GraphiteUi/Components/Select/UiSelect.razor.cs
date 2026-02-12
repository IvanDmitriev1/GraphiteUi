using GraphiteUi.Extensions;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GraphiteUi.Components;

public partial class UiSelect<TValue> : UiInputFieldBase<TValue>
{
    private readonly List<UiSelectItem<TValue>> _items = [];
    private ElementReference _rootReference;
    private UiSelectItem<TValue>? _selectedItem;
    private string? _lastValueAsString;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string ClearText { get; set; } = "Clear value";
    [Parameter] public string NoSuggestionsText { get; set; } = "No suggestions";

    private protected string RootClass => MergeRootClass(SelectStyles.RootClass);

    private bool HasSelectedItem => _selectedItem is not null;

    private string HiddenValue => HasSelectedItem ? CurrentValueAsString ?? string.Empty : string.Empty;

    private string SelectedText => _selectedItem?.DisplayText ?? string.Empty;

    private bool CanClear => !Disabled && !ReadOnly && HasSelectedItem;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (string.Equals(_lastValueAsString, CurrentValueAsString, StringComparison.Ordinal))
        {
            return;
        }

        _lastValueAsString = CurrentValueAsString;
        _selectedItem = FindItemByValue(CurrentValue);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return Task.CompletedTask;
        }

        return SetUpJsAsync();
    }

    private async Task SetUpJsAsync()
    {
        await using var module = await JsRuntime.LoadModule("js/uiSelect.js");
        await module.InvokeVoidAsync("refreshUiSelectBlazor", _rootReference);
    }

    internal void RegisterItem(UiSelectItem<TValue> item)
    {
        if (_items.Contains(item))
        {
            return;
        }

        _items.Add(item);

        if (_selectedItem is null && EqualityComparer<TValue>.Default.Equals(item.Value, CurrentValue))
        {
            _selectedItem = item;
            _lastValueAsString = CurrentValueAsString;
        }

        _ = InvokeAsync(StateHasChanged);
    }

    internal void UnregisterItem(UiSelectItem<TValue> item)
    {
        if (_items.Remove(item))
        {
            if (ReferenceEquals(_selectedItem, item))
            {
                _selectedItem = null;
            }

            _ = InvokeAsync(StateHasChanged);
        }
    }

    internal bool IsSelected(UiSelectItem<TValue> item)
    {
        return ReferenceEquals(item, _selectedItem);
    }

    internal string GetItemValueString(UiSelectItem<TValue> item)
    {
        return FormatValueAsString(item.Value) ?? string.Empty;
    }

    internal Task SelectAsync(UiSelectItem<TValue> item)
    {
        if (Disabled || ReadOnly || item.Disabled)
        {
            return Task.CompletedTask;
        }

        _selectedItem = item;
        CurrentValue = item.Value;
        _lastValueAsString = CurrentValueAsString;

        return Task.CompletedTask;
    }

    private Task ClearAsync()
    {
        if (!CanClear)
        {
            return Task.CompletedTask;
        }

        _selectedItem = null;
        CurrentValue = default!;
        _lastValueAsString = CurrentValueAsString;

        return Task.CompletedTask;
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

    private UiSelectItem<TValue>? FindItemByValue(TValue? value)
    {
        var comparer = EqualityComparer<TValue>.Default;

        foreach (UiSelectItem<TValue> item in _items)
        {
            if (comparer.Equals(item.Value, value))
            {
                return item;
            }
        }

        return null;
    }
}
