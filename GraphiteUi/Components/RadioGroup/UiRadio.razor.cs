using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiRadio<TValue> : ComponentBase
{
    [CascadingParameter] private UiRadioGroup<TValue>? Group { get; set; }

    [Parameter, EditorRequired] public TValue Value { get; set; } = default!;
    [Parameter] public string? ValueKey { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private string RootClass => RadioStyles.ItemClass;
    private bool IsDisabled => Disabled || (Group?.Disabled ?? false);
    internal string FormValue =>
        !string.IsNullOrWhiteSpace(ValueKey)
            ? ValueKey!
            : BindConverter.FormatValue(Value)?.ToString() ?? throw new InvalidOperationException(
                $"{nameof(UiRadio<TValue>)} requires {nameof(ValueKey)} when {nameof(Value)} cannot be converted to a string.");

    protected override void OnInitialized()
    {
        if (Group is null)
        {
            throw new InvalidOperationException($"{nameof(UiRadio<TValue>)} must be nested inside {nameof(UiRadioGroup<TValue>)}.");
        }
    }
}
