namespace GraphiteUi.Components;

internal sealed class ToastService : IToastService
{
    private WeakReference<UiToastHost>? _hostRef;

    private UiToastHost Host =>
        _hostRef is { } wr && wr.TryGetTarget(out UiToastHost? host)
            ? host
            : throw new InvalidOperationException(
                "No UiToastHost present. Place <UiToastHost /> in your layout.");

    public void Show(ToastOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        Host.Show(options);
    }

    public void Clear()
    {
        Host.Clear();
    }

    public void RegisterHost(UiToastHost host)
    {
        _hostRef = new WeakReference<UiToastHost>(host);
    }

    public void UnregisterHost(UiToastHost host)
    {
        if (_hostRef is not { } wr || !wr.TryGetTarget(out UiToastHost? current))
        {
            return;
        }

        if (ReferenceEquals(current, host))
        {
            _hostRef = null;
        }
    }
}
