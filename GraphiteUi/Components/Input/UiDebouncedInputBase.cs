using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public abstract class UiDebouncedInputBase<TValue> : UiInputBase<TValue>
{
    /// <summary>
    /// Gets or sets the delay, in milliseconds, for debouncing input events.
    /// A non-positive delay commits the value immediately.
    /// </summary>
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

        // Each invocation owns its token source until its wait completes.
        // A newer input cancels the previous wait without disposing its source early.
        using var cancellation = new CancellationTokenSource();
        _cts = cancellation;
        try
        {
            // Debounce needs one wait after the latest input, not recurring timer ticks.
            await Task.Delay(DebounceDelay, cancellation.Token);
            if (!_disposed && !Disabled && !ReadOnly && ReferenceEquals(_cts, cancellation))
                CurrentValueAsString = value;
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            // Superseded input and disposal are normal cancellation paths;
            // do not propagate them to Blazor's event handler.
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
