using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class DefaultUiInputFieldStyles
{
    public static string InputWrapperClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("group relative")
        .Add(Utils.InlineFlexStart)
        .Add("w-full shadow-xs cursor-text px-3 text-body")
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

    private static readonly string SmallWrapperClass = InputWrapperClass + " h-[var(--control-height-sm)] min-h-[var(--control-height-sm)]";
    private static readonly string MediumWrapperClass = InputWrapperClass + " h-[var(--control-height-md)] min-h-[var(--control-height-md)]";
    private static readonly string LargeWrapperClass = InputWrapperClass + " h-[var(--control-height-lg)] min-h-[var(--control-height-lg)]";

    public static string GetWrapperClass(GraphiteUi.Common.Size size) => size switch
    {
        GraphiteUi.Common.Size.Small => SmallWrapperClass,
        GraphiteUi.Common.Size.Large => LargeWrapperClass,
        _ => MediumWrapperClass
    };

    public const string DescriptionClass = "mt-1 mb-0 text-body-sm text-muted-foreground";

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("absolute block origin-top-left pointer-events-none subpixel-antialiased pe-2 max-w-full text-ellipsis")
        .Add("transition-[transform,color,left,opacity,translate,scale]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("z-20 top-1/2 -translate-y-1/2 left-3 text-body-sm text-muted-foreground")
        .Add("group-data-active:left-0 group-data-active:pointer-events-auto group-data-active:-translate-y-[calc(100%_+_var(--text-body-sm)/2_+_18px)] group-data-active:text-foreground group-focus-within:text-focus")
        .Add(ColorVariants.Disabled.GroupForeground)
        .ToString();

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("w-full bg-transparent outline-none")
        // Must sit on the input itself. It used to be on the wrapper, where the
        // `placeholder:` variant matched nothing, so Tailwind's preflight default
        // (50% of currentColor) applied instead — 3.35:1 on the light theme.
        .Add(ColorVariants.Placeholder.Foreground)
        .Add("group-data-[trailing=true]:pe-8")
        .ToString();

    public static string InputTrailingClass { get; } = "inline-flex items-center shrink-0";
}
