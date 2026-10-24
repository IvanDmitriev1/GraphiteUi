using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class ButtonStyles
{
    private static readonly string RootBaseClass = new CssClassBuilder(stackalloc char[384])
        .Add(Utils.InlineFlexCentered)
        .Add("font-medium no-underline gap-1.5 appearance-none select-none rounded-md whitespace-nowrap subpixel-antialiased overflow-hidden cursor-pointer")
        // Border WIDTH only. The color comes from the tone, so the box is always
        // allocated but only Secondary draws an edge. Keeping `border-transparent`
        // here would leave two border-color utilities on the element, and these are
        // concatenated, not TwMerge'd — stylesheet order would decide the winner.
        .Add("border shadow-xs")
        // transition
        .Add("transition-colors-transform-opacity")
        .Add(Utils.MotionReduceTransitionNone)
        // focus ring — one indicator only. The old base also carried a bare
        // `focus:ring`, which drew a second ring and fired on mouse click.
        .Add(Utils.FocusVisible)
        .ToString();

    /// <summary>
    /// Fixed heights, so buttons line up with the 40px input field. The old scale
    /// set vertical padding only, which made the large button ~69px against a ~46px
    /// medium and left nothing aligned in a form row.
    /// </summary>
    private static readonly string[] SizeClasses =
    [
        "h-[var(--control-height-sm)] min-w-16 px-3 text-body-sm",
        "h-[var(--control-height-md)] min-w-20 px-4 text-body-sm",
        "h-[var(--control-height-lg)] min-w-24 px-5 text-body"
    ];

    private static readonly string DisabledClass = new CssClassBuilder(stackalloc char[224])
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add("disabled:shadow-none disabled:border-transparent")
        .Add(Utils.DisabledCursorDefault)
        .ToString();

    private static readonly string[] RootClassesByColorAndSize = BuildRootClasses();

    public static string GetRootClasses(UiButton component) =>
        GetRootClasses(component.Color, component.Size);

    public static string GetRootClasses(ThemeColor color, Size size)
    {
        int colorIndex = (int)color;
        int sizeIndex = (int)size;
        int sizeCount = SizeClasses.Length;

        if ((uint)colorIndex >= (uint)SemanticTones.Count)
        {
            colorIndex = (int)ThemeColor.Primary;
        }

        if ((uint)sizeIndex >= (uint)sizeCount)
        {
            sizeIndex = (int)Size.Medium;
        }

        return RootClassesByColorAndSize[colorIndex * sizeCount + sizeIndex];
    }

    private static string[] BuildRootClasses()
    {
        int colorCount = SemanticTones.Count;
        int sizeCount = SizeClasses.Length;
        var rootClasses = new string[colorCount * sizeCount];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            var color = (ThemeColor)colorIndex;

            for (int sizeIndex = 0; sizeIndex < sizeCount; sizeIndex++)
            {
                rootClasses[colorIndex * sizeCount + sizeIndex] = CssClassBuilder.Empty()
                    .Add(RootBaseClass)
                    .Add(SemanticTones.Solid(color))
                    .Add(SizeClasses[sizeIndex])
                    .Add(DisabledClass)
                    .ToString();
            }
        }

        return rootClasses;
    }
}
