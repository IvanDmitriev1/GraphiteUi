using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class MenuStyles
{
    public static string RootClass { get; } = PopupStyles.RootClass;

    public static string TriggerClass { get; } = PopupStyles.TriggerClass;

    internal const string ContentDelta = "min-w-44";

    public static string ContentClass { get; } = new CssClassBuilder(stackalloc char[896])
        .Add(PopupStyles.ContentBaseClass)
        .Add(ContentDelta)
        .ToString();

    public static string ListClass { get; } = "flex flex-col gap-1";

    public static string ItemClass { get; } = new CssClassBuilder(stackalloc char[640])
        .Add("inline-flex h-9 w-full items-center justify-start gap-2 rounded-md px-2 text-left text-body-sm cursor-pointer transition-[background-color,color]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add(ColorVariants.Surface.HoverSurfaceClass)
        .Add(Utils.FocusVisible)
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add(Utils.DisabledCursorDefault)
        .ToString();
}
