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
        .Add("rounded-md")
        .Add("border")
        .Add("p-3")
        .Add("shadow-xs")
        .ToString();

    private static readonly string[] ToneByColor =
    [
        "bg-surface2 border-primary-5 text-surface2-foreground",
        "bg-surface2 border-primary-5 text-surface2-foreground",
        "bg-surface2 border-primary-5 text-surface2-foreground",
        "bg-success border-success-400 text-success-foreground",
        "bg-warning border-warning-400 text-warning-foreground",
        "bg-danger border-danger-400 text-danger-foreground",
        "bg-info border-info-400 text-info-foreground"
    ];

    private static readonly string[] IndicatorByColor =
    [
        "bg-primary-50",
        "bg-primary-50",
        "bg-primary-50",
        "bg-success-500",
        "bg-warning-500",
        "bg-danger-500",
        "bg-info-500"
    ];

    private static readonly string[] RootClassesByColor = BuildRootClassesByColor();
    private static readonly string[] IndicatorClassesByColor = BuildIndicatorClassesByColor();

    public static string  StartContentClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("mt-0.5")
        .Add("shrink-0")
        .ToString();

    public static string IndicatorClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("mt-1")
        .Add("size-2")
        .Add("shrink-0")
        .Add("rounded-full")
        .ToString();

    public static string ContentClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("flex")
        .Add("min-w-0")
        .Add("flex-1")
        .Add("flex-col")
        .Add("gap-1")
        .ToString();

    public static string TitleClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-small")
        .Add("text-current")
        .ToString();

    public static string BodyClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-small")
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
        .Add("outline-hidden")
        .Add("transition-[color,background-color,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("hover:bg-black/10")
        .Add("hover:opacity-100")
        .Add("focus-visible:ring")
        .Add("focus-visible:ring-black/25")
        .ToString();

    public static string DismissIconClass { get; } = new CssClassBuilder(stackalloc char[64])
        .Add("size-4")
        .ToString();

    public static string GetRootClasses(UiAlert component)
    {
        int colorIndex = (int)NormalizeColor(component.Color);

        if ((uint)colorIndex >= (uint)RootClassesByColor.Length)
        {
            colorIndex = (int)ThemeColor.Primary;
        }

        return RootClassesByColor[colorIndex];
    }

    public static string GetIndicatorClass(UiAlert component)
    {
        int colorIndex = (int)NormalizeColor(component.Color);

        if ((uint)colorIndex >= (uint)IndicatorClassesByColor.Length)
        {
            colorIndex = (int)ThemeColor.Primary;
        }

        return IndicatorClassesByColor[colorIndex];
    }

    private static ThemeColor NormalizeColor(ThemeColor color) =>
        color == ThemeColor.Inherit ? ThemeColor.Primary : color;

    private static string[] BuildRootClassesByColor()
    {
        int colorCount = ToneByColor.Length;
        var rootClasses = new string[colorCount];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            rootClasses[colorIndex] = CssClassBuilder.Empty()
                .Add(RootBaseClass)
                .Add(ToneByColor[colorIndex])
                .ToString();
        }

        return rootClasses;
    }

    private static string[] BuildIndicatorClassesByColor()
    {
        int colorCount = IndicatorByColor.Length;
        var indicatorClasses = new string[colorCount];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            indicatorClasses[colorIndex] = CssClassBuilder.Empty()
                .Add(IndicatorClass)
                .Add(IndicatorByColor[colorIndex])
                .ToString();
        }

        return indicatorClasses;
    }
}
