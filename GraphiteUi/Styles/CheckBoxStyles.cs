using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class CheckBoxStyles
{
    private static readonly string BaseClass = new CssClassBuilder(stackalloc char[512])
        .Add("p-1 -m-1 relative group max-w-full min-w-0")
        .Add(Utils.InlineFlexStart)
        .Add("gap-2 flex-shrink-0 outline-hidden cursor-pointer transition-transform")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("group-active:scale-95")
        .Add(Utils.DataDisabledCursorDefault)
        .Add(Utils.DataReadonlyCursorDefault)
        .ToString();

    public static string GetRootClasses(UiCheckbox component) => GetRootClasses();

    public static string GetRootClasses() => BaseClass;

    public static string InputClass { get; } = "sr-only peer";

    public static string ControlClass { get; } = new CssClassBuilder(stackalloc char[1024])
        .Add("relative")
        .Add(Utils.InlineFlexCentered)
        // Was size-8 (32px) against a 20px radio — the two controls did not match.
        .Add("size-5 shrink-0 rounded-sm border")
        .Add(ColorVariants.Border.Control)
        .Add("bg-surface1 text-transparent select-none transition-[background-color,border-color,color,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        // static -> hovered
        .Add(ColorVariants.Border.GroupControlHover)
        // checked state (driven by native input) — the accent, so a checked box
        // reads as a deliberate selection instead of an unlabelled white square.
        .Add("peer-checked:bg-control-checked peer-checked:border-control-checked peer-checked:text-control-checked-foreground group-hover:peer-checked:bg-control-checked-hover group-hover:peer-checked:border-control-checked-hover")
        // validation state
        .Add("data-[invalid=true]:border-invalid")
        // focus state
        .Add("peer-focus-visible:ring-2 peer-focus-visible:ring-focus peer-focus-visible:ring-offset-2 peer-focus-visible:ring-offset-background")
        // disabled state
        .Add(ColorVariants.Disabled.PeerBackground)
        .Add(ColorVariants.Disabled.PeerBorder)
        .Add(ColorVariants.Disabled.PeerForeground)
        .ToString();

    public static string IconClass { get; } = "size-3.5";

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("text-body text-foreground leading-normal")
        .Add(ColorVariants.Disabled.PeerForeground)
        .ToString();
}
