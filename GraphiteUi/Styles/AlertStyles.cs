using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class AlertStyles
{
    private static readonly string RootBaseClass = new CssClassBuilder(stackalloc char[512])
        .Add("relative")
        .Add("flex")
        .Add("w-full")
        .Add("items-start")
        .Add("gap-3")
        .Add("rounded-lg")
        .Add("border")
        .Add("p-3")
        .Add("shadow-xs")
        .ToString();

    private static readonly string[] RootClassesByColor = BuildRootClassesByColor();
    private static readonly string[] IndicatorClassesByColor = BuildIndicatorClassesByColor();

    public static string StartContentClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("mt-0.5")
        .Add("shrink-0")
        .ToString();

    public static string ContentClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("flex")
        .Add("min-w-0")
        .Add("flex-1")
        .Add("flex-col")
        .Add("gap-1")
        .ToString();

    public static string TitleClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-body-sm")
        .Add("font-semibold")
        .Add("text-current")
        .ToString();

    public static string BodyClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-body-sm")
        .Add("text-current")
        .Add("opacity-90")
        .ToString();

    public static string DismissButtonClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("inline-flex")
        .Add("size-7")
        .Add("shrink-0")
        .Add("items-center")
        .Add("justify-center")
        .Add("rounded-md")
        .Add("text-current")
        .Add("opacity-75")
        .Add("cursor-pointer")
        .Add("transition-[color,background-color,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        // Was `hover:bg-black/10` + `focus-visible:ring-black/25`: both hardcoded
        // black, and therefore invisible on a dark surface.
        .Add("hover:bg-neutral-15")
        .Add("hover:opacity-100")
        .Add(Utils.FocusVisible)
        .ToString();

    public static string DismissIconClass { get; } = new CssClassBuilder(stackalloc char[64])
        .Add("size-4")
        .ToString();

    public static string GetRootClasses(UiAlert component) =>
        RootClassesByColor[IndexOf(component.Color, RootClassesByColor.Length)];

    public static string GetIndicatorClass(UiAlert component) =>
        IndicatorClassesByColor[IndexOf(component.Color, IndicatorClassesByColor.Length)];

    private static int IndexOf(ThemeColor color, int length)
    {
        int index = (int)NormalizeColor(color);

        return (uint)index >= (uint)length ? (int)ThemeColor.Primary : index;
    }

    private static ThemeColor NormalizeColor(ThemeColor color) =>
        color == ThemeColor.Inherit ? ThemeColor.Primary : color;

    private static string[] BuildRootClassesByColor()
    {
        int colorCount = SemanticTones.Count;
        var rootClasses = new string[colorCount];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            rootClasses[colorIndex] = CssClassBuilder.Empty()
                .Add(RootBaseClass)
                // Tinted, not fully saturated. A full-bleed success or danger fill
                // reads as an error page rather than an inline message, and the
                // subtle tokens keep an AA-safe foreground in both themes.
                .Add(SemanticTones.Subtle((ThemeColor)colorIndex))
                .ToString();
        }

        return rootClasses;
    }

    private static string[] BuildIndicatorClassesByColor()
    {
        int colorCount = SemanticTones.Count;
        var indicatorClasses = new string[colorCount];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            indicatorClasses[colorIndex] = SemanticTones.Indicator((ThemeColor)colorIndex);
        }

        return indicatorClasses;
    }
}
