using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class ToastStyles
{
    public const int CloseAnimationMs = 160;

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
        .Add("[&_[data-slot=toast-dismiss]]:size-10")
        .Add("sm:[&_[data-slot=toast-dismiss]]:size-7")
        .Add("transition-[opacity,transform]")
        .Add("duration-150")
        .Add("ease-out")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("data-[state=open]:opacity-100")
        .Add("data-[state=open]:translate-y-0")
        .Add("data-[state=open]:scale-100")
        .Add("data-[state=closing]:opacity-0")
        .Add("data-[state=closing]:translate-y-2")
        .Add("data-[state=closing]:scale-[0.98]")
        .ToString();

    public static string StackClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("flex")
        .Add("w-[calc(100dvw-1rem)]")
        .Add("sm:w-[min(100dvw-2rem,28rem)]")
        .Add("max-w-full")
        .Add("flex-col")
        .Add("gap-2")
        .Add("[&>[data-slot=toast-item]:nth-child(n+3)]:hidden")
        .Add("sm:[&>[data-slot=toast-item]:nth-child(n+3)]:block")
        .ToString();

    public static string AlertClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("w-full")
        .ToString();

    public static string MessageClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-small")
        .Add("text-primary-60")
        .ToString();

    public static string ContentClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-small")
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
