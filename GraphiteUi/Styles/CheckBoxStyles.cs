using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class CheckBoxStyles
{
    private static readonly string BaseClass = new CssClassBuilder(stackalloc char[512])
        .Add("p-1 -m-1")
        .Add("relative")
        .Add("group")
        .Add("max-w-fit")
        .Add(Utils.InlineFlexStart)
        .Add("gap-2")
        .Add("flex-shrink-0")
        .Add("outline-hidden")
        .Add("cursor-pointer")
        .Add("transition-transform")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("group-active:scale-95")
        .Add(Utils.DataDisabledCursorDefault)
        .Add(Utils.DataReadonlyCursorDefault)
        .ToString();

    public static string GetRootClasses(UiCheckbox component) => BaseClass;

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add(Utils.VisuallyHidden)
        .Add("peer")
        .ToString();

    public static string ControlClass { get; } = new CssClassBuilder(stackalloc char[1024])
        .Add("relative")
        .Add(Utils.InlineFlexCentered)
        // Was size-8 (32px) against a 20px radio — the two controls did not match.
        .Add("size-5")
        .Add("shrink-0")
        .Add("rounded-sm")
        .Add("border")
        .Add(ColorVariants.Border.Control)
        .Add("bg-surface1")
        .Add("text-transparent")
        .Add("select-none")
        .Add("transition-[background-color,border-color,color,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        // static -> hovered
        .Add(ColorVariants.Border.GroupControlHover)
        // checked state (driven by native input) — the accent, so a checked box
        // reads as a deliberate selection instead of an unlabelled white square.
        .Add("peer-checked:bg-control-checked")
        .Add("peer-checked:border-control-checked")
        .Add("peer-checked:text-control-checked-foreground")
        .Add("group-hover:peer-checked:bg-control-checked-hover")
        .Add("group-hover:peer-checked:border-control-checked-hover")
        // validation state
        .Add("data-[invalid=true]:border-invalid")
        // focus state
        .Add("peer-focus-visible:ring-2")
        .Add("peer-focus-visible:ring-focus")
        .Add("peer-focus-visible:ring-offset-2")
        .Add("peer-focus-visible:ring-offset-background")
        // disabled state
        .Add(ColorVariants.Disabled.PeerBackground)
        .Add(ColorVariants.Disabled.PeerBorder)
        .Add(ColorVariants.Disabled.PeerForeground)
        .ToString();

    public static string IconClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("size-3.5")
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("text-body")
        .Add("text-foreground")
        .Add("leading-none")
        .Add(ColorVariants.Disabled.PeerForeground)
        .ToString();
}
