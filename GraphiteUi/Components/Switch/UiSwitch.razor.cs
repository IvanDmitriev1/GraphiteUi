using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiSwitch : UiInputBase<bool>
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private protected string RootClass => MergeRootClass(SwitchStyles.RootClass);

    protected override bool TryParseValueFromString(string? value,
        out bool result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        throw new NotSupportedException(
            $"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property.");
    }

    protected virtual Task OnChangeAsync(ChangeEventArgs args)
    {
        if (Disabled || ReadOnly)
        {
            return Task.CompletedTask;
        }

        if (BindConverter.TryConvertToBool(args.Value, CultureInfo.InvariantCulture, out bool isChecked))
        {
            CurrentValue = isChecked;
        }

        return Task.CompletedTask;
    }
}
