using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class ToastStyles
{
    private static readonly string HostBaseClass = new CssClassBuilder(stackalloc char[256])
        .Add("fixed")
        .Add("z-[120]")
        .Add("flex")
        .Add("w-full")
        .Add("p-2")
        .Add("sm:p-4")
        .Add("pointer-events-none")
        .ToString();

    private static readonly string[] HostPlacementClasses =
    [
        "top-0 left-0 items-start justify-center sm:left-auto sm:right-0 sm:justify-end",
        "top-0 left-0 items-start justify-center",
        "bottom-0 left-0 items-end justify-center sm:left-auto sm:right-0 sm:justify-end"
    ];

    private static readonly string[] HostClassesByPlacement = BuildHostClassesByPlacement();

    private static readonly string ItemBaseClass = new CssClassBuilder(stackalloc char[384])
        .Add("relative")
        .Add("pointer-events-auto")
        .Add("w-full")
        .Add("[&_[data-slot=alert-dismissButton]]:size-10")
        .Add("sm:[&_[data-slot=alert-dismissButton]]:size-7")
        .ToString();

    public static string StackClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("flex")
        .Add("w-[calc(100dvw-1rem)]")
        .Add("sm:w-[min(100dvw-2rem,28rem)]")
        .Add("max-w-full")
        .Add("flex-col")
        .Add("gap-2")
        .Add("[&>[data-slot=toast-item]:nth-child(n+3)]:hidden")
        .Add("sm:[&>[data-slot=toast-item]:nth-child(n+3)]:flex")
        .ToString();

    public static string GetHostRootClasses(UiToastHost host)
    {
        int placementIndex = (int)host.Placement;

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
