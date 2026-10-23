using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class TooltipStyles
{
    public static string RootClass { get; } = "group relative inline-flex";

    public static string BubbleBaseClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("pointer-events-none absolute z-[130] max-w-56 rounded-md border")
        .Add(ColorVariants.Border.Default)
        .Add(ColorVariants.Surface.PopupBackground)
        .Add("px-2 py-1 text-body-sm shadow-md whitespace-nowrap opacity-0 scale-[0.98] transition-[opacity,transform] duration-150 ease-out")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("group-hover:opacity-100 group-hover:scale-100 group-focus-within:opacity-100 group-focus-within:scale-100")
        .ToString();

    private static readonly string[] BubbleClassesByPlacement =
    [
        BuildBubbleClass("bottom-full mb-2", "left-0"),
        BuildBubbleClass("bottom-full mb-2", "left-1/2 -translate-x-1/2"),
        BuildBubbleClass("bottom-full mb-2", "right-0"),
        BuildBubbleClass("top-full mt-2", "left-0"),
        BuildBubbleClass("top-full mt-2", "left-1/2 -translate-x-1/2"),
        BuildBubbleClass("top-full mt-2", "right-0")
    ];

    public static string GetBubbleClass(UiTooltip tooltip) => GetBubbleClass(tooltip.Align, tooltip.Bottom);

    public static string GetBubbleClass(Align align, bool bottom)
    {
        int alignIndex = align switch
        {
            Align.Start => 0,
            Align.End => 2,
            _ => 1
        };

        return BubbleClassesByPlacement[(bottom ? 3 : 0) + alignIndex];
    }

    private static string BuildBubbleClass(string verticalClass, string alignClass) =>
        CssClassBuilder.Empty()
            .Add(BubbleBaseClass)
            .Add(verticalClass)
            .Add(alignClass)
            .ToString();
}
