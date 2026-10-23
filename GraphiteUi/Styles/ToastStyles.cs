using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class ToastStyles
{
    private static readonly string HostBaseClass = "fixed z-[120] flex w-full p-2 sm:p-4 pointer-events-none";

    private static readonly string[] HostPlacementClasses =
    [
        "top-0 left-0 items-start justify-center sm:left-auto sm:right-0 sm:justify-end",
        "top-0 left-0 items-start justify-center",
        "bottom-0 left-0 items-end justify-center sm:left-auto sm:right-0 sm:justify-end"
    ];

    private static readonly string[] HostClassesByPlacement = BuildHostClassesByPlacement();

    private static readonly string ItemBaseClass = "relative pointer-events-auto w-full [&_[data-slot=alert-dismissButton]]:size-10 sm:[&_[data-slot=alert-dismissButton]]:size-7";

    public static string StackClass { get; } = "flex w-[calc(100dvw-1rem)] sm:w-[min(100dvw-2rem,28rem)] max-w-full flex-col gap-2 [&>[data-slot=toast-item]:nth-child(n+3)]:hidden sm:[&>[data-slot=toast-item]:nth-child(n+3)]:flex";

    public static string GetHostRootClasses(UiToastHost host) => GetHostRootClasses(host.Placement);

    public static string GetHostRootClasses(ToastPlacement placement)
    {
        int placementIndex = (int)placement;

        if ((uint)placementIndex >= (uint)HostClassesByPlacement.Length)
        {
            placementIndex = (int)ToastPlacement.TopRight;
        }

        return HostClassesByPlacement[placementIndex];
    }

    public static string ItemClass => ItemBaseClass;

    private static string[] BuildHostClassesByPlacement()
    {
        int placementCount = HostPlacementClasses.Length;
        var classesByPlacement = new string[placementCount];

        for (int i = 0; i < placementCount; i++)
        {
            classesByPlacement[i] = CssClassBuilder.Empty()
                .Add(HostBaseClass)
                .Add(HostPlacementClasses[i])
                .ToString();
        }

        return classesByPlacement;
    }
}
