using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class EmptyStateStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[384])
        .Add(CommonStyles.SurfaceFrameRegularPaddedClass)
        .Add("flex w-full flex-col items-center justify-center gap-2 text-center")
        .ToString();

    public static string IconClass { get; } = "text-subtle-foreground";

    public static string TitleClass { get; } = "text-h6 text-foreground";

    public static string DescriptionClass { get; } = "text-body-sm text-muted-foreground";
}
