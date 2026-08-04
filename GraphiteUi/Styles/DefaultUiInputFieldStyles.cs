using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class DefaultUiInputFieldStyles
{
    public static string InputWrapperClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("group")
        .Add("relative")
        .Add(Utils.InlineFlexStart)
        .Add("w-full")
        .Add("shadow-xs")
        .Add("cursor-text")
        .Add("h-10 min-h-10")
        .Add("px-3")
        .Add("text-body")
        .Add("transition-[background-color,border-color,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("has-[label]:mt-[calc(var(--text-body-sm)_+_10px)]")
        .Add(CommonStyles.ControlFrameClass)
        .Add(ColorVariants.Border.ControlHover)
        .Add(Utils.FocusWithin)
        // The focus ring is the sole focus indicator. Keep the 1px border slot
        // allocated, but make it transparent while the ring is active so focus
        // cannot render as a second inner line.
        .Add("focus-within:border-transparent")
        .Add("focus-within:hover:border-transparent")
        .Add("data-invalid:focus-within:border-transparent")
        .Add("data-invalid:border-invalid")
        .Add(ColorVariants.Ring.Invalid)
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add(Utils.DisabledCursorDefault)
        .Add("data-disabled:border-transparent")
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[512])
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
        .Add("text-body-sm")
        .Add("text-muted-foreground")
        .Add("group-data-active:left-0")
        .Add("group-data-active:pointer-events-auto")
        .Add("group-data-active:-translate-y-[calc(100%_+_var(--text-body-sm)/2_+_18px)]")
        .Add("group-data-active:text-foreground")
        .Add("group-focus-within:text-focus")
        .Add(ColorVariants.Disabled.GroupForeground)
        .ToString();

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("w-full")
        .Add("bg-transparent")
        .Add("outline-none")
        // Must sit on the input itself. It used to be on the wrapper, where the
        // `placeholder:` variant matched nothing, so Tailwind's preflight default
        // (50% of currentColor) applied instead — 3.35:1 on the light theme.
        .Add(ColorVariants.Placeholder.Foreground)
        .Add("group-data-[trailing=true]:pe-8")
        .ToString();

    public static string InputTrailingClass { get; } = new CssClassBuilder(stackalloc char[192])
        .Add("inline-flex")
        .Add("items-center")
        .Add("shrink-0")
        .ToString();
}
