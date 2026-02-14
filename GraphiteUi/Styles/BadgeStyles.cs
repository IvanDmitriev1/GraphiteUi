using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class BadgeStyles
{
    private static readonly string BaseClass = new CssClassBuilder(stackalloc char[256])
        .Add("inline-flex")
        .Add("items-center")
        .Add("rounded-full")
        .Add("border")
        .Add("font-medium")
        .Add("whitespace-nowrap")
        .ToString();

    private static readonly string[] SizeClasses =
    [
        "px-2 py-0.5 text-small",
        "px-2.5 py-1 text-small",
        "px-3 py-1.5 text-regular"
    ];

    private static readonly string[] ToneByColor =
    [
        "bg-surface2 text-surface2-foreground border-primary-10",
        "bg-surface2 text-surface2-foreground border-primary-10",
        "bg-secondary text-secondary-foreground border-secondary-30",
        "bg-success text-success-foreground border-success-400",
        "bg-warning text-warning-foreground border-warning-400",
        "bg-danger text-danger-foreground border-danger-400",
        "bg-info text-info-foreground border-info-400"
    ];

    private static readonly string[] RootClassesByColorAndSize = BuildRootClasses();

    public static string GetRootClasses(UiBadge component)
    {
        int colorIndex = (int)NormalizeColor(component.Color);
        int sizeIndex = (int)component.Size;
        int sizeCount = SizeClasses.Length;

        if ((uint)colorIndex >= (uint)ToneByColor.Length)
        {
            colorIndex = (int)ThemeColor.Primary;
        }

        if ((uint)sizeIndex >= (uint)sizeCount)
        {
            sizeIndex = (int)Size.Small;
        }

        return RootClassesByColorAndSize[colorIndex * sizeCount + sizeIndex];
    }

    private static ThemeColor NormalizeColor(ThemeColor color) =>
        color == ThemeColor.Inherit ? ThemeColor.Primary : color;

    private static string[] BuildRootClasses()
    {
        int colorCount = ToneByColor.Length;
        int sizeCount = SizeClasses.Length;
        var classes = new string[colorCount * sizeCount];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            for (int sizeIndex = 0; sizeIndex < sizeCount; sizeIndex++)
            {
                classes[colorIndex * sizeCount + sizeIndex] = CssClassBuilder.Empty()
                    .Add(BaseClass)
                    .Add(ToneByColor[colorIndex])
                    .Add(SizeClasses[sizeIndex])
                    .ToString();
            }
        }

        return classes;
    }
}
