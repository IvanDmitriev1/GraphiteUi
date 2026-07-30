using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class EmptyStateStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[384])
        .Add(CommonStyles.SurfaceFrameRegularPaddedClass)
        .Add("flex")
        .Add("w-full")
        .Add("flex-col")
        .Add("items-center")
        .Add("justify-center")
        .Add("gap-2")
        .Add("text-center")
        .ToString();

    public static string IconClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-subtle-foreground")
        .ToString();

    public static string TitleClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-h6")
        .Add("text-foreground")
        .ToString();

    public static string DescriptionClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-body-sm")
        .Add("text-muted-foreground")
        .ToString();
}
