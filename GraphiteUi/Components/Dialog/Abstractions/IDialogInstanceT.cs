namespace GraphiteUi.Components;

public interface IDialogInstance<TResult> : IDialogInstance
{
    new Task<DialogResult<TResult>> WaitAsync(CancellationToken ct);
    void SetResult(TResult result);
}