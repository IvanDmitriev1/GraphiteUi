using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Components.Bases;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public sealed class ButtonStyles : IUiComponentStyle<UiButton>
{
    private readonly record struct ColorStyles(string Background, string Hover, string Active);

    private static readonly string Base = new CssClassBuilder(stackalloc char[256])
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

    private static readonly ColorStyles[] Colors =
    [
        new(string.Empty, string.Empty, string.Empty),
        new("bg-primary text-primary-foreground", "hover:bg-white hover:text-secondary-foreground", "active:bg-white active:text-black"),
        new("bg-secondary text-secondary-foreground", "hover:bg-primary-15 hover:text-primary-foreground", "active:bg-primary active:text-primary-foreground"),
        new("bg-success text-success-foreground", "hover:bg-success-400", "active:bg-success-500"),
        new("bg-warning text-warning-foreground", "hover:bg-warning-400", "active:bg-warning-500"),
        new("bg-danger text-danger-foreground", "hover:bg-danger-400", "active:bg-danger-500"),
        new("bg-info text-info-foreground", "hover:bg-info-400", "active:bg-info-500")
    ];

    private static readonly string Disabled = new CssClassBuilder(stackalloc char[128])
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add(Utils.DisabledCursorDefault)
        .ToString();

    private static readonly string[] CoreClassesByColorAndSize = BuildCoreClasses();

    private static string[] BuildCoreClasses()
    {
        int colorCount = Colors.Length;
        int sizeCount = SizeClasses.Length;
        var coreClasses = new string[colorCount * sizeCount];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            var color = (ThemeColor)colorIndex;
            var colorStyles = Colors[colorIndex];

            for (int sizeIndex = 0; sizeIndex < sizeCount; sizeIndex++)
            {
                coreClasses[colorIndex * sizeCount + sizeIndex] = CssClassBuilder.Empty()
                    .Add(Base)
                    .Add(colorStyles.Background)
                    .Add(colorStyles.Hover)
                    .Add(colorStyles.Active)
                    .Add(ColorVariants.Ring.Default)
                    .Add(ColorVariants.Border.Default)
                    .Add(ColorVariants.Ring.Focus(color))
                    .Add(SizeClasses[sizeIndex])
                    .Add(Disabled)
                    .ToString();
            }
        }

        return coreClasses;
    }

    public static string GetCoreClasses(UiButton component)
    {
        int colorIndex = (int)component.Color;
        int sizeIndex = (int)component.Size;
        int sizeCount = SizeClasses.Length;

        return CoreClassesByColorAndSize[colorIndex * sizeCount + sizeIndex];
    }
}
