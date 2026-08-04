using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class MenuStyles
{
    public static string RootClass { get; } = PopupStyles.RootClass;

    public static string TriggerClass { get; } = PopupStyles.TriggerClass;

    public static string ContentClass { get; } = new CssClassBuilder(stackalloc char[896])
        .Add(PopupStyles.ContentClass)
        .Add("min-w-44")
        .ToString();

    public static string ListClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("flex")
        .Add("flex-col")
        .Add("gap-1")
        .ToString();

    public static string ItemClass { get; } = new CssClassBuilder(stackalloc char[640])
        .Add("inline-flex")
        .Add("h-9")
        .Add("w-full")
        .Add("items-center")
        .Add("justify-start")
        .Add("gap-2")
        .Add("rounded-md")
        .Add("px-2")
        .Add("text-left")
        .Add("text-body-sm")
        .Add("cursor-pointer")
        .Add("transition-[background-color,color]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add(ColorVariants.Surface.HoverBackground)
        .Add(Utils.FocusVisible)
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add(Utils.DisabledCursorDefault)
        .ToString();
}
