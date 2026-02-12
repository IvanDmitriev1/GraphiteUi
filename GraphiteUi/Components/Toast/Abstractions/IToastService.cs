namespace GraphiteUi.Components;

public interface IToastService
{
    void Show(ToastOptions options);
    void Clear();
    void RegisterHost(UiToastHost host);
    void UnregisterHost(UiToastHost host);
}
