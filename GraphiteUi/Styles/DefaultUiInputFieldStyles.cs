using GraphiteUi.Common;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class DefaultUiInputFieldStyles
{
    public static string InputWrapperClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("group")
        .Add("relative")
        .Add(Utils.InlineFlexStart)
        .Add("w-full")
        .Add("shadow-xs")
        .Add("cursor-text")
        .Add("h-10 min-h-10")
        .Add("px-3")
        .Add("transition-[background]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("has-[label]:mt-[calc(var(--text-small)_+_10px)]")
        .Add("border")
        .Add("rounded-md")
        .Add(ColorVariants.Placeholder.Foreground)
        .Add(ColorVariants.Surface.Background)
        .Add(ColorVariants.Surface.HoverBackground)
        .Add(ColorVariants.Border.Default)
        .Add(Utils.FocusWithin)
        .Add("focus-within:ring")
        .Add(ColorVariants.Ring.FocusWithin(ThemeColor.Primary))
        .Add("data-invalid:border-danger-400")
        .Add("data-invalid:focus-within:ring-danger-600/20")
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add(Utils.DisabledCursorDefault)
        .Add("data-disabled:border-none")
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("absolute")
        .Add("block")
        .Add("origin-top-left")
        .Add("pointer-events-none")
        .Add("subpixel-antialiased")
        .Add("pe-2")
        .Add("max-w-full")
        .Add("text-ellipsis")
        .Add("transition-[transform,color,left,opacity,translate,scale]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("z-20")
        .Add("top-1/2")
        .Add("-translate-y-1/2")
        .Add("left-3")
        .Add("text-small")
        .Add("group-data-active:left-0")
        .Add("group-data-active:pointer-events-auto")
        .Add("group-data-active:-translate-y-[calc(100%_+_var(--text-small)/2_+_18px)]")
        .Add("group-data-active:text-foreground")
        .Add(ColorVariants.Disabled.GroupForeground)
        .ToString();

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("w-full")
        .Add("outline-none")
        .ToString();
}
