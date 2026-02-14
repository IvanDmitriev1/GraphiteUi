using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class DrawerStyles
{
    public static string ToggleInputClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("peer")
        .Add("sr-only")
        .ToString();

    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[1024])
        .Add("fixed")
        .Add("inset-0")
        .Add("z-[110]")
        .Add("pointer-events-none")
        .Add("peer-checked:pointer-events-auto")
        .Add("peer-checked:[&_[data-slot=drawer-backdrop]]:opacity-100")
        .Add("peer-checked:[&_[data-slot=drawer-backdrop]]:pointer-events-auto")
        .Add("peer-checked:[&_[data-slot=drawer-panel]]:translate-x-0")
        .ToString();

    public static string TriggerClass { get; } = new CssClassBuilder(stackalloc char[640])
        .Add(PopupStyles.TriggerClass)
        .Add("peer-disabled:opacity-disabled")
        .Add("peer-disabled:pointer-events-none")
        .ToString();

    public static string BackdropClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("absolute")
        .Add("inset-0")
        .Add("bg-black/30")
        .Add("pointer-events-none")
        .Add("opacity-0")
        .Add("transition-opacity")
        .Add("duration-150")
        .Add("ease-out")
        .Add(Utils.MotionReduceTransitionNone)
        .ToString();

    public static string PanelBaseClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("absolute")
        .Add("top-0")
        .Add("h-full")
        .Add("w-[min(90dvw,24rem)]")
        .Add("max-w-full")
        .Add("rounded-md")
        .Add("border")
        .Add(ColorVariants.Border.Default)
        .Add(ColorVariants.Surface.PopupBackground)
        .Add("text-regular")
        .Add("p-4")
        .Add("shadow-xl")
        .Add("transition-transform")
        .Add("duration-150")
        .Add("ease-out")
        .Add(Utils.MotionReduceTransitionNone)
        .ToString();

    public static string HeaderClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("mb-3")
        .Add("text-medium")
        .Add("text-foreground")
        .ToString();

    public static string ContentClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("flex")
        .Add("flex-col")
        .Add("gap-2")
        .ToString();

    public static string GetPanelClass(UiDrawer drawer)
    {
        string placementClass = drawer.Side switch
        {
            Align.End => "right-0 translate-x-full",
            _ => "left-0 -translate-x-full"
        };

        return CssClassBuilder.Empty()
            .Add(PanelBaseClass)
            .Add(placementClass)
            .ToString();
    }
}
