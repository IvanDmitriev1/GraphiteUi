namespace GraphiteUi.Components;

public enum ToastPlacement
{
    TopRight,
    TopCenter,
    BottomRight
}

public static class ToastPlacementExtensions
{
    public static string ToAttributeValue(this ToastPlacement placement) => placement switch
    {
        ToastPlacement.TopCenter => "top-center",
        ToastPlacement.BottomRight => "bottom-right",
        _ => "top-right"
    };
}
