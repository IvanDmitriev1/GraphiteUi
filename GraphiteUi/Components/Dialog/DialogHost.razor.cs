using GraphiteUi.Components.Bases;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class DialogHost : UiComponentBase, IDisposable
{
    [Inject] private IDialogService DialogService { get; set; } = null!;

    private readonly List<IDialogInstance> _instances = [];
    private int _nextDialogId;

    public async Task<DialogResult<TResult>> ShowAsync<TDialog, TResult>(
        DialogOptions options,
        IReadOnlyDictionary<string, object>? parameters,
        CancellationToken ct = default)
        where TDialog : BaseDialog<TResult>
    {
        var id = new DialogId(Interlocked.Increment(ref _nextDialogId));
        var instance = new DialogInstance<TDialog, TResult>(id, options, parameters);

        _instances.Add(instance);
        StateHasChanged();

        try
        {
            return await instance.WaitAsync(ct);
        }
        catch (TaskCanceledException)
        {
            return DialogResult<TResult>.Canceled;
        }
        finally
        {
            _instances.Remove(instance);
            StateHasChanged();
        }
    }

    public async Task ShowAsync<TDialog>(
        DialogOptions options,
        IReadOnlyDictionary<string, object>? parameters,
        CancellationToken ct = default)
        where TDialog : BaseDialog
    {
        var id = new DialogId(Interlocked.Increment(ref _nextDialogId));
        var instance = new DialogInstance<TDialog>(id, options, parameters);

        _instances.Add(instance);
        StateHasChanged();

        try
        {
            await instance.WaitAsync(ct);
        }
        catch (TaskCanceledException)
        {
            //
        }
        finally
        {
            _instances.Remove(instance);
            StateHasChanged();
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            DialogService.RegisterHost(this);
        }
    }

    public void Dispose()
    {
        foreach (var instance in _instances)
        {
            instance.Close();
        }
    }
}