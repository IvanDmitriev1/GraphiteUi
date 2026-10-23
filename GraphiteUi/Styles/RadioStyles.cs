using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class RadioStyles
{
    public static string GroupClass { get; } = "flex flex-col gap-2";

    public static string ItemClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("group inline-flex items-center")
        // Was gap-0 with an ml-4 label, which did not match the checkbox's gap-1.5.
        .Add("gap-2 cursor-pointer select-none")
        .Add(Utils.DataDisabled)
        .Add(Utils.DataDisabledCursorDefault)
        .ToString();

    public static string InputClass { get; } = "peer sr-only";

    public static string ControlClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("relative inline-flex size-5 shrink-0 items-center justify-center rounded-full border")
        .Add(ColorVariants.Border.Control)
        .Add("bg-surface1 transition-[background-color,border-color,border-width,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add(ColorVariants.Border.GroupControlHover)
        .Add("peer-focus-visible:ring-2 peer-focus-visible:ring-focus peer-focus-visible:ring-offset-2 peer-focus-visible:ring-offset-background")
        // Accent dot via a thick border, matching the checked checkbox fill.
        .Add("peer-checked:border-control-checked peer-checked:border-[6px] group-hover:peer-checked:border-control-checked-hover data-[invalid=true]:border-invalid")
        .Add(ColorVariants.Disabled.PeerBorder)
        .Add(ColorVariants.Disabled.PeerBackground)
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-body text-foreground")
        .Add(ColorVariants.Disabled.PeerForeground)
        .ToString();
}
