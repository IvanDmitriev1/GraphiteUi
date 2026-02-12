using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class CommonStyles
{
    public static string SurfaceFrameClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("rounded-md")
        .Add("border")
        .Add(ColorVariants.Border.Default)
        .Add(ColorVariants.Surface.Background)
        .ToString();

    public static string SurfaceFrameRegularClass { get; } = new CssClassBuilder(stackalloc char[320])
        .Add(SurfaceFrameClass)
        .Add("text-regular")
        .ToString();

    public static string SurfaceFrameRegularPaddedClass { get; } = new CssClassBuilder(stackalloc char[384])
        .Add(SurfaceFrameRegularClass)
        .Add("p-3")
        .ToString();

    public static string CardClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add(SurfaceFrameRegularClass)
        .Add("rounded-lg")
        .Add("shadow-xs")
        .Add("p-4")
        .ToString();

    public static string CardHeaderClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("mb-3")
        .Add("text-medium")
        .Add("text-foreground")
        .ToString();

    public static string CardBodyClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("text-regular")
        .Add("text-surface1-foreground")
        .ToString();

    public static string CardFooterClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("mt-4")
        .Add("pt-3")
        .Add("border-t")
        .Add(ColorVariants.Border.Default)
        .ToString();
}
