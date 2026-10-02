using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public abstract class UiDebouncedInputBase<TValue> : UiInputBase<TValue>
{
    [Parameter] public int DebounceDelay { get; set; }
    private CancellationTokenSource? _cts;
    private bool _disposed;

    protected Task OnInputAsync(ChangeEventArgs args) => DebounceAsync(args.Value as string);

    public async Task DebounceAsync(string? value)
    {
        if (_disposed || Disabled || ReadOnly)
            return;

        _cts?.Cancel();
        if (DebounceDelay <= 0)
        {
            CurrentValueAsString = value;
            return;
        }

        using var cancellation = new CancellationTokenSource();
        _cts = cancellation;
        try
        {
            await Task.Delay(DebounceDelay, cancellation.Token);
            if (!_disposed && !Disabled && !ReadOnly && ReferenceEquals(_cts, cancellation))
                CurrentValueAsString = value;
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            // A later keystroke or component disposal superseded this value.
        }
        finally
        {
            if (ReferenceEquals(_cts, cancellation))
                _cts = null;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _disposed = true;
            _cts?.Cancel();
        }
        base.Dispose(disposing);
    }
}
