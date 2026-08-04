using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class RadioStyles
{
    public static string GroupClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("flex")
        .Add("flex-col")
        .Add("gap-2")
        .ToString();

    public static string ItemClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("group")
        .Add("inline-flex")
        .Add("items-center")
        // Was gap-0 with an ml-4 label, which did not match the checkbox's gap-1.5.
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

    public static string ControlClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("relative")
        .Add("inline-flex")
        .Add("size-5")
        .Add("shrink-0")
        .Add("items-center")
        .Add("justify-center")
        .Add("rounded-full")
        .Add("border")
        .Add(ColorVariants.Border.Control)
        .Add("bg-surface1")
        .Add("transition-[background-color,border-color,border-width,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add(ColorVariants.Border.GroupControlHover)
        .Add("peer-focus-visible:ring-2")
        .Add("peer-focus-visible:ring-focus")
        .Add("peer-focus-visible:ring-offset-2")
        .Add("peer-focus-visible:ring-offset-background")
        // Accent dot via a thick border, matching the checked checkbox fill.
        .Add("peer-checked:border-control-checked")
        .Add("peer-checked:border-[6px]")
        .Add("group-hover:peer-checked:border-control-checked-hover")
        .Add("data-[invalid=true]:border-invalid")
        .Add(ColorVariants.Disabled.PeerBorder)
        .Add(ColorVariants.Disabled.PeerBackground)
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-body")
        .Add("text-foreground")
        .Add(ColorVariants.Disabled.PeerForeground)
        .ToString();
}
