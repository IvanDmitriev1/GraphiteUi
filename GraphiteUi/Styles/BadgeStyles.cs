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
        "px-2 py-0.5 text-caption",
        "px-2.5 py-1 text-body-sm",
        "px-3 py-1.5 text-body"
    ];

    private static readonly string[] RootClassesByColorAndSize = BuildRootClasses();

    public static string GetRootClasses(UiBadge component)
    {
        int colorIndex = (int)NormalizeColor(component.Color);
        int sizeIndex = (int)component.Size;
        int sizeCount = SizeClasses.Length;

        if ((uint)colorIndex >= (uint)SemanticTones.Count)
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
        int colorCount = SemanticTones.Count;
        int sizeCount = SizeClasses.Length;
        var classes = new string[colorCount * sizeCount];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            var color = (ThemeColor)colorIndex;

            for (int sizeIndex = 0; sizeIndex < sizeCount; sizeIndex++)
            {
                classes[colorIndex * sizeCount + sizeIndex] = CssClassBuilder.Empty()
                    .Add(BaseClass)
                    // Tinted rather than fully saturated: a badge is a label, not a
                    // call to action, and the subtle tones carry their own AA-safe
                    // foreground in both themes.
                    .Add(SemanticTones.Subtle(color))
                    .Add(SizeClasses[sizeIndex])
                    .ToString();
            }
        }

        return classes;
    }
}
