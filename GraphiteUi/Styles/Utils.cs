using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class Utils
{
    public static readonly string VisuallyHidden = new CssClassBuilder(stackalloc char[32])
        .Add("sr-only")
        .ToString();

    public const string InlineFlexCentered = "inline-flex items-center justify-center";
    public const string InlineFlexStart = "inline-flex items-center justify-start";
    public const string MotionReduceTransitionNone = "motion-reduce:transition-none";
    public const string DisabledCursorDefault = "disabled:cursor-default";
    public const string DataDisabledCursorDefault = "data-[disabled=true]:cursor-default";
    public const string DataReadonlyCursorDefault = "data-[readonly=true]:cursor-default";
    public const string GroupDataDisabledCursorDefault = "group-data-[disabled=true]:cursor-default";
    public const string GroupDataReadonlyCursorDefault = "group-data-[readonly=true]:cursor-default";

    /// <summary>
    /// Dimming + pointer suppression for a whole subtree. <c>opacity-disabled</c> is
    /// declared as a custom utility in <c>_theme.css</c>; before that it silently
    /// compiled to nothing.
    /// </summary>
    public static readonly string Disabled = new CssClassBuilder(stackalloc char[128])
        .Add("opacity-disabled")
        .Add("pointer-events-none")
        .ToString();

    /// <summary>Same as <see cref="Disabled"/>, driven by <c>data-disabled="true"</c>.</summary>
    public const string DataDisabled =
        "data-[disabled=true]:opacity-disabled data-[disabled=true]:pointer-events-none";

    /// <summary>
    /// The single focus indicator for the library: a 2px accent ring offset from the
    /// element. Do not pair this with a bare <c>focus:ring</c> — that draws two
    /// indicators and fires on mouse click as well as keyboard.
    /// </summary>
    public static readonly string FocusVisible = new CssClassBuilder(stackalloc char[192])
        .Add("outline-hidden")
        .Add("focus-visible:z-10")
        .Add(ColorVariants.Ring.Focus)
        .Add("focus-visible:ring-offset-2")
        .Add("focus-visible:ring-offset-background")
        .ToString();

    public static readonly string FocusWithin = new CssClassBuilder(stackalloc char[192])
        .Add("outline-hidden")
        .Add("focus-within:z-10")
        .Add(ColorVariants.Ring.FocusWithin)
        .Add("focus-within:ring-offset-2")
        .Add("focus-within:ring-offset-background")
        .ToString();
}
