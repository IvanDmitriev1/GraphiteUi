using GraphiteUi.Styles;
using GraphiteUi.Utilities;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using TailwindMerge;

namespace GraphiteUi.Components;

public partial class UiCheckbox : UiInputBase<bool>
{
    /// <summary>
    /// Gets or sets content to be rendered inside the input.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] internal TwMerge TwMerge { get; set; } = null!;

    private protected string RootClass => RootClassMergeCache.GetOrAdd(
        TwMerge,
        CheckBoxStyles.GetCoreClasses(this),
        Class);

    private protected string WrapperClass = CheckBoxStyles.WrapperClass;
    private protected string InputClass = CheckBoxStyles.InputClass;
    private protected string ControlClass = CheckBoxStyles.ControlClass;
    private protected string IconClass = CheckBoxStyles.IconClass;
    private protected string LabelClass = CheckBoxStyles.LabelClass;

    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        throw new NotSupportedException(
            $"This component does not parse string inputs. " +
            $"Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
    }

    protected virtual Task OnChangeAsync(ChangeEventArgs args)
    {
        if (Disabled || ReadOnly)
        {
            return Task.CompletedTask;
        }

        CurrentValue = (bool)args.Value!;
        return Task.CompletedTask;
    }
}
