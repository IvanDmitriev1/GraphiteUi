namespace GraphiteUi.Components;

public interface IDialogService
{
    Task<DialogResult<TResult>> ShowAsync<TDialog, TResult>(
        DialogOptions options,
        IReadOnlyDictionary<string, object>? parameters,
        CancellationToken ct = default)
        where TDialog : BaseDialog<TResult>;

    Task ShowAsync<TDialog>(
        DialogOptions options,
        IReadOnlyDictionary<string, object>? parameters,
        CancellationToken ct = default)
        where TDialog : BaseDialog;

    void RegisterHost(DialogHost host);
}