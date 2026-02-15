using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class ButtonStyles
{
    private static readonly string RootBaseClass = new CssClassBuilder(stackalloc char[256])
        .Add(Utils.InlineFlexCentered)
        .Add("min-w-max")
        .Add("font-normal")
        .Add("gap-1.5")
        .Add("appearance-none")
        .Add("select-none")
        .Add("rounded-md")
        .Add("whitespace-nowrap")
        .Add("subpixel-antialiased")
        .Add("overflow-hidden")
        .Add("cursor-pointer")
        .Add("border")
        .Add("shadow-sm")
        // transition
        .Add("transition-colors-transform-opacity")
        .Add(Utils.MotionReduceTransitionNone)
        // focus ring
        .Add("focus:ring")
        .Add(Utils.FocusVisible)
        .ToString();

    private static readonly string[] SizeClasses =
    [
        "min-w-16 py-1.5 px-2 text-small",
        "min-w-20 py-2.5 px-3 text-medium",
        "min-w-24 py-5 px-6 font-bold text-large"
    ];

    private static readonly string[] ToneByColor =
    [
        string.Empty,
        "bg-primary text-primary-foreground hover:bg-white hover:text-secondary-foreground active:bg-white active:text-black",
        "bg-secondary text-secondary-foreground hover:bg-primary-15 hover:text-primary-foreground active:bg-primary active:text-primary-foreground",
        "bg-success text-success-foreground hover:bg-success-400 active:bg-success-500",
        "bg-warning text-warning-foreground hover:bg-warning-400 active:bg-warning-500",
        "bg-danger text-danger-foreground hover:bg-danger-400 active:bg-danger-500",
        "bg-info text-info-foreground hover:bg-info-400 active:bg-info-500"
    ];

    private static readonly string DisabledClass = new CssClassBuilder(stackalloc char[128])
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add(Utils.DisabledCursorDefault)
        .ToString();

    private static readonly string[] RootClassesByColorAndSize = BuildRootClasses();

    public static string GetRootClasses(UiButton component)
    {
        int colorIndex = (int)component.Color;
        int sizeIndex = (int)component.Size;
        int sizeCount = SizeClasses.Length;

        if ((uint)colorIndex >= (uint)ToneByColor.Length)
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
        int colorCount = ToneByColor.Length;
        int sizeCount = SizeClasses.Length;
        var rootClasses = new string[colorCount * sizeCount];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            var color = (ThemeColor)colorIndex;

            for (int sizeIndex = 0; sizeIndex < sizeCount; sizeIndex++)
            {
                rootClasses[colorIndex * sizeCount + sizeIndex] = CssClassBuilder.Empty()
                    .Add(RootBaseClass)
                    .Add(ToneByColor[colorIndex])
                    .Add(ColorVariants.Ring.Default)
                    .Add(ColorVariants.Border.Default)
                    .Add(ColorVariants.Ring.Focus(color))
                    .Add(SizeClasses[sizeIndex])
                    .Add(DisabledClass)
                    .ToString();
            }
        }

        return rootClasses;
    }
}
