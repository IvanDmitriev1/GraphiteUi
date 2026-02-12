using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Extensions;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GraphiteUi.Components;

public partial class UiToastHost : UiComponentBase, IAsyncDisposable
{
    private readonly List<ToastItem> _entries = [];
    private ElementReference _rootReference;
    private IJSObjectReference? _module;
    private int _nextToastId;

    [Inject] private IToastService ToastService { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Parameter] public ToastPlacement Placement { get; set; } = ToastPlacement.TopRight;
    [Parameter] public int MaxVisible { get; set; } = 4;
    [Parameter] public int DefaultDurationMs { get; set; } = 6000;
    [Parameter] public bool DangerStickyByDefault { get; set; } = true;
    [Parameter] public bool PauseOnHover { get; set; } = true;
    [Parameter] public bool NewestOnTop { get; set; } = true;

    private protected string RootClass => MergeRootClass(ToastStyles.GetHostRootClasses(this));

    public void Show(ToastOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _ = InvokeAsync(() =>
        {
            AddToast(options);
            StateHasChanged();
        });
    }

    public void Clear()
    {
        _ = InvokeAsync(() =>
        {
            var ids = new List<int>();

            foreach (ToastItem entry in _entries)
            {
                if (!entry.IsClosing)
                {
                    ids.Add(entry.Id);
                }
            }

            foreach (int id in ids)
            {
                _ = DismissAsync(id);
            }

            return Task.CompletedTask;
        });
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ToastService.RegisterHost(this);
            ElementReference = _rootReference;
            _module = await JsRuntime.LoadModule("js/uiToast.js");
            await _module.InvokeVoidAsync("setupUiToastHostBlazor", _rootReference);
        }
    }

    public async ValueTask DisposeAsync()
    {
        ToastService.UnregisterHost(this);

        if (_module is not null)
        {
            try
            {
                await _module.InvokeVoidAsync("destroyUiToastHostBlazor", _rootReference);
            }
            catch (JSDisconnectedException)
            {
                //
            }
        }

        await _module.HandleDispose();
        _module = null;
    }

    private async Task DismissAsync(int id)
    {
        if (!SetEntryClosing(id))
        {
            return;
        }

        StateHasChanged();

        await Task.Delay(ToastStyles.CloseAnimationMs);

        if (RemoveEntry(id))
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    private void AddToast(ToastOptions options)
    {
        EnsureCapacity();

        ThemeColor color = NormalizeColor(options.Color);
        int durationMs = ResolveDuration(color, options, out bool sticky);
        bool dismissible = options.Dismissible ?? true;
        bool pauseOnHover = options.PauseOnHover ?? PauseOnHover;

        var entry = new ToastItem(
            Id: Interlocked.Increment(ref _nextToastId),
            Color: color,
            Title: options.Title,
            Message: options.Message,
            Content: options.Content,
            Class: options.Class,
            DurationMs: durationMs,
            Sticky: sticky,
            Dismissible: dismissible,
            PauseOnHover: pauseOnHover);

        if (NewestOnTop)
        {
            _entries.Insert(0, entry);
        }
        else
        {
            _entries.Add(entry);
        }
    }

    private void EnsureCapacity()
    {
        int maxVisible = Math.Max(1, MaxVisible);

        while (_entries.Count >= maxVisible)
        {
            int oldestIndex = NewestOnTop ? _entries.Count - 1 : 0;
            _entries.RemoveAt(oldestIndex);
        }
    }

    private int ResolveDuration(ThemeColor color, ToastOptions options, out bool sticky)
    {
        if (options.DurationMs is { } explicitDuration)
        {
            int duration = Math.Max(0, explicitDuration);
            sticky = duration == 0;
            return duration;
        }

        if (DangerStickyByDefault && color == ThemeColor.Danger)
        {
            sticky = true;
            return 0;
        }

        sticky = false;
        return Math.Max(1, DefaultDurationMs);
    }

    private bool SetEntryClosing(int id)
    {
        for (int i = 0; i < _entries.Count; i++)
        {
            ToastItem entry = _entries[i];

            if (entry.Id != id)
            {
                continue;
            }

            if (entry.IsClosing)
            {
                return false;
            }

            _entries[i] = entry with { IsClosing = true };
            return true;
        }

        return false;
    }

    private bool RemoveEntry(int id)
    {
        int removedCount = _entries.RemoveAll(entry => entry.Id == id);
        return removedCount > 0;
    }

    private static ThemeColor NormalizeColor(ThemeColor color) =>
        color == ThemeColor.Inherit ? ThemeColor.Primary : color;

    private sealed record ToastItem(
        int Id,
        ThemeColor Color,
        string? Title,
        string? Message,
        RenderFragment? Content,
        string? Class,
        int DurationMs,
        bool Sticky,
        bool Dismissible,
        bool PauseOnHover,
        bool IsClosing = false);
}
