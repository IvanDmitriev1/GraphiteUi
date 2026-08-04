using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class TooltipStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("group")
        .Add("relative")
        .Add("inline-flex")
        .ToString();

    public static string BubbleBaseClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("pointer-events-none")
        .Add("absolute")
        .Add("z-[130]")
        .Add("max-w-56")
        .Add("rounded-md")
        .Add("border")
        .Add(ColorVariants.Border.Default)
        .Add(ColorVariants.Surface.PopupBackground)
        .Add("px-2")
        .Add("py-1")
        .Add("text-body-sm")
        .Add("shadow-md")
        .Add("whitespace-nowrap")
        .Add("opacity-0")
        .Add("scale-[0.98]")
        .Add("transition-[opacity,transform]")
        .Add("duration-150")
        .Add("ease-out")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("group-hover:opacity-100")
        .Add("group-hover:scale-100")
        .Add("group-focus-within:opacity-100")
        .Add("group-focus-within:scale-100")
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

    public static string GetBubbleClass(UiTooltip tooltip)
    {
        int alignIndex = tooltip.Align switch
        {
            Align.Start => 0,
            Align.End => 2,
            _ => 1
        };

        return BubbleClassesByPlacement[(tooltip.Bottom ? 3 : 0) + alignIndex];
    }

    private static string BuildBubbleClass(string verticalClass, string alignClass) =>
        CssClassBuilder.Empty()
            .Add(BubbleBaseClass)
            .Add(verticalClass)
            .Add(alignClass)
            .ToString();
}
