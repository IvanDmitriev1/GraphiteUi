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

    public static string GetBubbleClass(UiTooltip tooltip)
    {
        string verticalClass = tooltip.Bottom
            ? "top-full mt-2"
            : "bottom-full mb-2";

        string alignClass = tooltip.Align switch
        {
            Align.Start => "left-0",
            Align.End => "right-0",
            _ => "left-1/2 -translate-x-1/2"
        };

        return CssClassBuilder.Empty()
            .Add(BubbleBaseClass)
            .Add(verticalClass)
            .Add(alignClass)
            .ToString();
    }
}
