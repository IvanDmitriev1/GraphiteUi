using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiSelectItem<TValue> : ComponentBase, IDisposable
{
    [CascadingParameter] internal UiSelect<TValue> Parent { get; set; } = default!;

    [Parameter, EditorRequired] public TValue Value { get; set; } = default!;
    [Parameter] public string? Text { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public RenderFragment<TValue>? Template { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    internal string DisplayText => string.IsNullOrWhiteSpace(Text)
        ? Value?.ToString() ?? string.Empty
        : Text;

    private bool IsDisabled => Disabled || Parent.Disabled || Parent.ReadOnly;

    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(UiSelectItem<TValue>)} must be placed inside {nameof(UiSelect<TValue>)}.");
        }

        Parent.RegisterItem(this);
    }

    private Task OnClickAsync()
    {
        return Parent.SelectAsync(this);
    }

    public void Dispose()
    {
        Parent.UnregisterItem(this);
    }
}
