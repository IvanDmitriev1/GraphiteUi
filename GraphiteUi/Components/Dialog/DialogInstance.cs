namespace GraphiteUi.Components;

internal sealed class DialogInstance<TDialog>(
    DialogId id,
    DialogOptions options,
    IReadOnlyDictionary<string, object>? parameters)
    : DialogInstanceBase<TDialog>(id, options, parameters)
    where TDialog : BaseDialog
{
    private readonly TaskCompletionSource _tcs = 
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    public override Task WaitAsync(CancellationToken ct) => _tcs.Task.WaitAsync(ct);

    public override void Close() => _tcs.TrySetResult();
}

internal sealed class DialogInstance<TDialog, TResult>(
    DialogId id,
    DialogOptions options,
    IReadOnlyDictionary<string, object>? parameters)
    : DialogInstanceBase<TDialog>(id, options, parameters), IDialogInstance<TResult>
    where TDialog : BaseDialog<TResult>
{
    private readonly TaskCompletionSource<DialogResult<TResult>> _tcs =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    public override Task<DialogResult<TResult>> WaitAsync(CancellationToken ct) => _tcs.Task.WaitAsync(ct);
    public void SetResult(TResult result) => _tcs.TrySetResult(DialogResult<TResult>.FromValue(result));
    public override void Close() => _tcs.TrySetResult(DialogResult<TResult>.Canceled);
}