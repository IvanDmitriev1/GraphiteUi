using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class CardStyles
{
    /// <summary>
    /// Built from its own parts rather than from <see cref="CommonStyles.SurfaceFrameRegularClass"/>:
    /// these strings are raw concatenations, not TwMerge merges, so inheriting the
    /// frame and then adding <c>rounded-lg</c> left both radii on the element and let
    /// stylesheet order pick the winner.
    /// </summary>
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("rounded-lg border")
        .Add(ColorVariants.Border.Default)
        .Add(ColorVariants.Surface.Background)
        .Add("text-body shadow-sm p-4")
        .ToString();

    public static string HeaderClass { get; } = "mb-3 text-h6 text-foreground";

    public static string BodyClass { get; } = "text-body text-surface1-foreground";

    public static string FooterClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("mt-4 pt-3 border-t")
        .Add(ColorVariants.Border.Default)
        .ToString();

    public static string TitleClass { get; } = "text-h6 text-foreground";
}
