using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Extensions;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GraphiteUi.Components;

public partial class UiToastHost : UiComponentBase, IAsyncDisposable
{
    private const int DefaultDurationMs = 4000;
    private readonly List<ToastEntry> _toastItems = [];
    private ElementReference _rootReference;
    private IJSObjectReference? _module;
    private int _nextToastId;

    [Inject] private IToastService ToastService { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Parameter] public ToastPlacement Placement { get; set; } = ToastPlacement.TopRight;
    [Parameter] public int MaxVisible { get; set; } = 4;
    [Parameter] public bool DangerStickyByDefault { get; set; } = true;
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
            if (_toastItems.Count == 0)
            {
                return;
            }

            _toastItems.Clear();
            StateHasChanged();
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

    private void AddToast(ToastOptions options)
    {
        EnsureCapacity();

        (ToastOptions normalizedOptions, bool sticky) = NormalizeOptions(options);

        var entry = new ToastEntry(
            Id: Interlocked.Increment(ref _nextToastId),
            Options: normalizedOptions,
            Sticky: sticky);

        if (NewestOnTop)
        {
            _toastItems.Insert(0, entry);
        }
        else
        {
            _toastItems.Add(entry);
        }
    }

    private void EnsureCapacity()
    {
        int maxVisible = Math.Max(1, MaxVisible);

        while (_toastItems.Count >= maxVisible)
        {
            int oldestIndex = NewestOnTop ? _toastItems.Count - 1 : 0;
            _toastItems.RemoveAt(oldestIndex);
        }
    }

    private (ToastOptions Options, bool Sticky) NormalizeOptions(ToastOptions raw)
    {
        ThemeColor color = NormalizeColor(raw.Color);
        int durationMs;
        bool sticky;

        if (raw.DurationMs is { } explicitDuration)
        {
            durationMs = Math.Max(0, explicitDuration);
            sticky = durationMs == 0;
        }
        else if (DangerStickyByDefault && color == ThemeColor.Danger)
        {
            sticky = true;
            durationMs = 0;
        }
        else
        {
            sticky = false;
            durationMs = DefaultDurationMs;
        }

        var normalized = new ToastOptions
        {
            Color = color,
            Title = raw.Title,
            Content = raw.Content,
            DurationMs = durationMs,
            Class = raw.Class
        };

        return (normalized, sticky);
    }

    private void RemoveToast(int id)
    {
        if (_toastItems.RemoveAll(item => item.Id == id) == 0)
        {
            return;
        }

        StateHasChanged();
    }

    private static ThemeColor NormalizeColor(ThemeColor color) =>
        color == ThemeColor.Inherit ? ThemeColor.Primary : color;

    private sealed record ToastEntry(
        int Id,
        ToastOptions Options,
        bool Sticky);
}
