using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class CommonStyles
{
    public static string SurfaceFrameClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("rounded-md border")
        .Add(ColorVariants.Border.Default)
        .Add(ColorVariants.Surface.Background)
        .ToString();

    /// <summary>
    /// Same frame, but with the 3:1 control border. Separate rather than layered on
    /// top of <see cref="SurfaceFrameClass"/>: these are raw concatenations, not
    /// TwMerge merges, so adding a second border-color class leaves both on the
    /// element and lets stylesheet order decide the winner.
    /// </summary>
    public static string ControlFrameClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("rounded-md border")
        .Add(ColorVariants.Border.Control)
        .Add(ColorVariants.Surface.Background)
        .ToString();

    public static string SurfaceFrameRegularClass { get; } = new CssClassBuilder(stackalloc char[320])
        .Add(SurfaceFrameClass)
        .Add("text-body")
        .ToString();

    public static string ControlFrameRegularClass { get; } = new CssClassBuilder(stackalloc char[384])
        .Add(ControlFrameClass)
        .Add("text-body")
        .ToString();

    public static string SurfaceFrameRegularPaddedClass { get; } = new CssClassBuilder(stackalloc char[384])
        .Add(SurfaceFrameRegularClass)
        .Add("p-3")
        .ToString();

    public static string CardClass => CardStyles.RootClass;

    public static string CardHeaderClass => CardStyles.HeaderClass;

    public static string CardBodyClass => CardStyles.BodyClass;

    public static string CardFooterClass => CardStyles.FooterClass;
}
