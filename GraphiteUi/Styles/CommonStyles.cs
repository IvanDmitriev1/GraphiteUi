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

    /// <summary>
    /// Same frame, but with the 3:1 control border. Separate rather than layered on
    /// top of <see cref="SurfaceFrameClass"/>: these are raw concatenations, not
    /// TwMerge merges, so adding a second border-color class leaves both on the
    /// element and lets stylesheet order decide the winner.
    /// </summary>
    public static string ControlFrameClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("rounded-md")
        .Add("border")
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

    /// <summary>
    /// Built from its own parts rather than from <see cref="SurfaceFrameRegularClass"/>:
    /// these strings are raw concatenations, not TwMerge merges, so inheriting the
    /// frame and then adding <c>rounded-lg</c> left both radii on the element and let
    /// stylesheet order pick the winner.
    /// </summary>
    public static string CardClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("rounded-lg")
        .Add("border")
        .Add(ColorVariants.Border.Default)
        .Add(ColorVariants.Surface.Background)
        .Add("text-body")
        .Add("shadow-sm")
        .Add("p-4")
        .ToString();

    public static string CardHeaderClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("mb-3")
        .Add("text-h6")
        .Add("text-foreground")
        .ToString();

    public static string CardBodyClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("text-body")
        .Add("text-surface1-foreground")
        .ToString();

    public static string CardFooterClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("mt-4")
        .Add("pt-3")
        .Add("border-t")
        .Add(ColorVariants.Border.Default)
        .ToString();
}
