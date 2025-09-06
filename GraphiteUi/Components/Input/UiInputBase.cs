using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace GraphiteUi.Components;

public abstract class UiInputBase<TValue> : InputBase<TValue>
{
    /// <summary>
    /// Gets or sets CSS class names that will be applied to the component.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the input is disabled.
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the input is read-only.
    /// </summary>
    [Parameter] public bool ReadOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the input is required.
    /// </summary>
    [Parameter] public bool Required { get; set; }
}