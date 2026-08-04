using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class SwitchStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("relative")
        .Add("inline-flex")
        .Add("items-center")
        .Add("gap-2")
        .Add("cursor-pointer")
        .Add("select-none")
        .Add(Utils.DataDisabled)
        .Add(Utils.DataDisabledCursorDefault)
        .ToString();

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("peer")
        .Add("sr-only")
        .ToString();

    public static string TrackClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("relative")
        .Add("inline-flex")
        .Add("h-6")
        .Add("w-11")
        .Add("shrink-0")
        .Add("overflow-hidden")
        .Add("items-center")
        .Add("rounded-full")
        .Add("border")
        .Add(ColorVariants.Border.Control)
        .Add("bg-control-track")
        .Add(ColorVariants.Border.ControlHover)
        .Add("hover:bg-control-track-hover")
        .Add("transition-[background-color,border-color,box-shadow]")
        .Add("duration-200")
        .Add("ease-in-out")
        .Add(Utils.MotionReduceTransitionNone)
        // Checked is the accent, not plain white.
        .Add("peer-checked:bg-control-checked")
        .Add("peer-checked:border-control-checked")
        .Add("peer-checked:hover:bg-control-checked-hover")
        .Add("peer-checked:hover:border-control-checked-hover")
        .Add("peer-checked:[&>span]:left-[1.375rem]")
        .Add("peer-checked:[&>span]:bg-control-checked-foreground")
        .Add("peer-focus-visible:ring-2")
        .Add("peer-focus-visible:ring-focus")
        .Add("peer-focus-visible:ring-offset-2")
        .Add("peer-focus-visible:ring-offset-background")
        .Add("data-[invalid=true]:border-invalid")
        .Add(ColorVariants.Disabled.PeerBackground)
        .Add(ColorVariants.Disabled.PeerBorder)
        .ToString();

    public static string ThumbClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("pointer-events-none")
        .Add("absolute")
        .Add("left-1")
        .Add("top-1/2")
        .Add("h-4")
        .Add("w-4")
        .Add("rounded-full")
        .Add("-translate-y-1/2")
        .Add("bg-control-thumb")
        .Add("shadow-xs")
        .Add("transition-[left,background-color]")
        .Add("duration-300")
        .Add("ease-[cubic-bezier(0.22,1,0.36,1)]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("peer-disabled:bg-control-thumb-disabled")
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-body")
        .Add("text-foreground")
        .ToString();
}
