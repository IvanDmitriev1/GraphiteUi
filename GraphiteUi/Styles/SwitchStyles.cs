using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class SwitchStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("relative inline-flex items-center gap-2 cursor-pointer select-none")
        .Add(Utils.DataDisabled)
        .Add(Utils.DataDisabledCursorDefault)
        .ToString();

    public static string InputClass { get; } = "peer sr-only";

    public static string TrackClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("relative inline-flex h-6 w-11 shrink-0 overflow-hidden items-center rounded-full border")
        .Add(ColorVariants.Border.Control)
        .Add("bg-control-track")
        .Add(ColorVariants.Border.ControlHover)
        .Add("hover:bg-control-track-hover transition-[background-color,border-color,box-shadow] duration-200 ease-in-out")
        .Add(Utils.MotionReduceTransitionNone)
        // Checked is the accent, not plain white.
        .Add("peer-checked:bg-control-checked peer-checked:border-control-checked peer-checked:hover:bg-control-checked-hover peer-checked:hover:border-control-checked-hover peer-checked:[&>span]:left-[1.375rem] peer-checked:[&>span]:bg-control-checked-foreground peer-focus-visible:ring-2 peer-focus-visible:ring-focus peer-focus-visible:ring-offset-2 peer-focus-visible:ring-offset-background data-[invalid=true]:border-invalid")
        .Add(ColorVariants.Disabled.PeerBackground)
        .Add(ColorVariants.Disabled.PeerBorder)
        .ToString();

    public static string ThumbClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("pointer-events-none absolute left-1 top-1/2 h-4 w-4 rounded-full -translate-y-1/2 bg-control-thumb shadow-xs transition-[left,background-color] duration-300 ease-[cubic-bezier(0.22,1,0.36,1)]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("peer-disabled:bg-control-thumb-disabled")
        .ToString();

    public static string LabelClass { get; } = "text-body text-foreground";
}
