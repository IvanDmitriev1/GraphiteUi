using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public abstract class UiDebouncedInputBase<TValue> : UiInputBase<TValue>
{
    /// <summary>
    /// Gets or sets the delay, in milliseconds, for debouncing input events.
    /// </summary>
    [Parameter] public int DebounceDelay { get; set; }

    private CancellationTokenSource? _cts;

    protected Task OnInputAsync(ChangeEventArgs args)
    {
        string? value = (string?)args.Value;

        if (DebounceDelay > 0)
        {
            return DebounceAsync(value);
        }

        CurrentValueAsString = value;
        return Task.CompletedTask;
    }

    public async Task DebounceAsync(string? value)
    {
        _cts?.Cancel();
        _cts?.Dispose();

        _cts = new CancellationTokenSource();
        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(DebounceDelay));

        while (await timer.WaitForNextTickAsync(_cts.Token))
        {
            // Debounce time has passed without further input; trigger the debounced event
            CurrentValueAsString = value;
            break;
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing) 
            return;

        _cts?.Cancel();
        _cts?.Dispose();
    }
}