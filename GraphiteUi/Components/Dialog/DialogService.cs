namespace GraphiteUi.Components;

internal sealed class DialogService : IDialogService
{
    private WeakReference<DialogHost>? _hostRef;

    private DialogHost Host =>
        _hostRef is { } wr && wr.TryGetTarget(out var host)
            ? host
            : throw new InvalidOperationException(
                "No DialogHost present. Place <DialogHost /> in your layout.");

    public Task<DialogResult<TResult>> ShowAsync<TDialog, TResult>(
        DialogOptions options,
        IReadOnlyDictionary<string, object>? parameters,
        CancellationToken ct = default)
        where TDialog : BaseDialog<TResult>
    {
        return Host.ShowAsync<TDialog, TResult>(options, parameters, ct);
    }

    public Task ShowAsync<TDialog>(
        DialogOptions options,
        IReadOnlyDictionary<string, object>? parameters,
        CancellationToken ct = default) where TDialog : BaseDialog
    {
        return Host.ShowAsync<TDialog>(options, parameters, ct);
    }

    public void RegisterHost(DialogHost host)
    {
        _hostRef = new WeakReference<DialogHost>(host);
    }
}