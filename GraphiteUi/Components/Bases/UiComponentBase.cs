using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using TailwindMerge;

namespace GraphiteUi.Components.Bases;

/// <summary>
/// Represents a base class for all components.
/// </summary>
public abstract class UiComponentBase : ComponentBase
{
    /// <summary>
    /// Gets or sets an HTML tag of the component.
    /// </summary>
    [Parameter]
    public string As { get; set; } = "div";

    /// <summary>
    /// Gets or sets CSS class names that will be applied to the component.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Gets or sets the associated <see cref="ElementReference"/>.
    /// <para>
    /// May be <see langword="null"/> if accessed before the component is rendered.
    /// </para>
    /// </summary>
    [DisallowNull]
    public ElementReference? ElementReference { get; protected set; }

    [Inject] internal TwMerge TwMerge { get; set; } = default!;


    private protected virtual string? RootClass => Class;

    /// <summary>
    /// Triggers a re-render of the component.
    /// </summary>
    public void Rerender() => StateHasChanged();
}