using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class DrawerStyles
{
    public static string ToggleInputClass { get; } = "peer sr-only";

    public static string RootClass { get; } = "fixed inset-0 z-[110] pointer-events-none peer-checked:pointer-events-auto peer-checked:[&_[data-slot=drawer-backdrop]]:opacity-100 peer-checked:[&_[data-slot=drawer-backdrop]]:pointer-events-auto peer-checked:[&_[data-slot=drawer-panel]]:translate-x-0";

    public static string TriggerClass { get; } = new CssClassBuilder(stackalloc char[896])
        .Add(PopupStyles.TriggerClass)
        .Add("peer-disabled:opacity-disabled peer-disabled:pointer-events-none")
        .ToString();

    public static string BackdropClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("absolute inset-0 bg-scrim pointer-events-none opacity-0 transition-opacity duration-150 ease-out")
        .Add(Utils.MotionReduceTransitionNone)
        .ToString();

    public static string PanelBaseClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("absolute top-0 h-full w-[min(90dvw,24rem)] max-w-full rounded-lg border")
        .Add(ColorVariants.Border.Default)
        .Add(ColorVariants.Surface.PopupBackground)
        .Add("text-body p-4 shadow-xl transition-transform duration-150 ease-out")
        .Add(Utils.MotionReduceTransitionNone)
        .ToString();

    public static string HeaderClass { get; } = "mb-3 text-h6 text-foreground";

    public static string ContentClass { get; } = "flex flex-col gap-2";

    private static readonly string[] PanelClassesBySide =
    [
        BuildPanelClass("left-0 -translate-x-full"),
        BuildPanelClass("right-0 translate-x-full")
    ];

    public static string GetPanelClass(UiDrawer drawer) => GetPanelClass(drawer.Side);

    public static string GetPanelClass(Align side) => PanelClassesBySide[side == Align.End ? 1 : 0];

    private static string BuildPanelClass(string placementClass) =>
        CssClassBuilder.Empty()
            .Add(PanelBaseClass)
            .Add(placementClass)
            .ToString();
}
