using System.Diagnostics.CodeAnalysis;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace GraphiteUi.Components;

public partial class UiFileInput : UiInputBase<IReadOnlyList<IBrowserFile>>
{
    [Parameter] public string? Label { get; set; }
    [Parameter] public bool Multiple { get; set; }
    [Parameter] public int MaxFiles { get; set; } = 10;
    [Parameter] public EventCallback<IReadOnlyList<IBrowserFile>> FilesChanged { get; set; }

    private protected string RootClass => MergeRootClass(FileInputStyles.RootClass);

    protected override void OnInitialized()
    {
        base.OnInitialized();
        CurrentValue = [];
    }

    protected override bool TryParseValueFromString(string? value,
        [MaybeNullWhen(false)] out IReadOnlyList<IBrowserFile> result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        throw new NotSupportedException(
            $"This component does not parse string inputs. Bind to '{nameof(CurrentValue)}'.");
    }

    private async Task OnFilesChangedAsync(InputFileChangeEventArgs args)
    {
        IReadOnlyList<IBrowserFile> files = Multiple
            ? args.GetMultipleFiles(Math.Max(1, MaxFiles))
            : [args.File];

        CurrentValue = files;
        await FilesChanged.InvokeAsync(files);
    }
}
