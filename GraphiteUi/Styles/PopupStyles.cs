using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class PopupStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("relative inline-flex flex-col")
        .Add(Utils.DataDisabled)
        .ToString();

    public static string TriggerClass { get; } = new CssClassBuilder(stackalloc char[640])
        .Add("inline-flex relative z-50 items-center justify-center gap-2 h-[var(--control-height-md)]")
        .Add(CommonStyles.ControlFrameClass)
        .Add("text-body-sm font-medium px-4 cursor-pointer transition-[background-color,border-color,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add(ColorVariants.Surface.HoverSurfaceClass)
        .Add(Utils.FocusVisible)
        .Add("data-[open=true]:bg-surface-hover data-[open=true]:border-control-hover")
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add(Utils.DisabledCursorDefault)
        .ToString();

    public static string BackdropClass { get; } = new CssClassBuilder(stackalloc char[320])
        .Add("fixed inset-0 z-40")
        // Single scrim token. Popup, drawer and dialog previously used black/20,
        // black/30 and background/40 respectively.
        .Add("bg-scrim")
        .Add("opacity-0 transition-opacity")
        .Add("data-[state=open]:duration-[var(--popup-backdrop-duration)] data-[state=open]:ease-[var(--popup-easing)] data-[state=closed]:duration-[var(--popup-backdrop-duration)] data-[state=closed]:ease-[var(--popup-easing)]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("data-[state=open]:opacity-100 data-[state=closed]:opacity-0 data-[state=closed]:pointer-events-none")
        .ToString();

    internal static string ContentBaseClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("absolute left-0")
        .Add("top-[calc(100%+0.5rem)] z-50 w-full rounded-lg border")
        .Add(ColorVariants.Border.Default)
        .Add(ColorVariants.Surface.PopupBackground)
        .Add("p-2 shadow-lg origin-top opacity-0 translate-y-1 transition-[opacity,translate]")
        .Add("data-[state=open]:duration-[var(--popup-content-duration)] data-[state=open]:ease-[var(--popup-easing)] data-[state=closed]:duration-[var(--popup-content-duration)] data-[state=closed]:ease-[var(--popup-easing)]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("data-[state=open]:opacity-100 data-[state=open]:translate-y-0 data-[state=closed]:opacity-0 data-[state=closed]:translate-y-1 data-[state=closed]:pointer-events-none")
        .ToString();

    public static string ContentClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add(ContentBaseClass)
        .Add("min-w-40")
        .ToString();
}
