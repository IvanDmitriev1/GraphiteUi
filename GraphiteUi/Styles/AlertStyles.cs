using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class AlertStyles
{
    private static readonly string RootBaseClass = "relative flex w-full items-start gap-3 rounded-lg border p-3 shadow-xs";

    private static readonly string[] RootClassesByColor = BuildRootClassesByColor();
    private static readonly string[] IndicatorClassesByColor = BuildIndicatorClassesByColor();

    public static string StartContentClass { get; } = "mt-0.5 shrink-0";

    public static string ContentClass { get; } = "flex min-w-0 flex-1 flex-col gap-1";

    public static string TitleClass { get; } = "text-body-sm font-semibold text-current";

    public static string BodyClass { get; } = "text-body-sm text-current opacity-90";

    public static string DismissButtonClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("inline-flex size-7 shrink-0 items-center justify-center rounded-md text-current opacity-75 cursor-pointer transition-[color,background-color,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        // Was `hover:bg-black/10` + `focus-visible:ring-black/25`: both hardcoded
        // black, and therefore invisible on a dark surface.
        .Add("hover:bg-surface-active hover:opacity-100")
        .Add(Utils.FocusVisible)
        .ToString();

    public static string DismissIconClass { get; } = "size-4";

    public static string GetRootClasses(UiAlert component) => GetRootClasses(component.Color);

    public static string GetRootClasses(ThemeColor color) =>
        RootClassesByColor[IndexOf(color, RootClassesByColor.Length)];

    public static string GetIndicatorClass(UiAlert component) => GetIndicatorClass(component.Color);

    public static string GetIndicatorClass(ThemeColor color) =>
        IndicatorClassesByColor[IndexOf(color, IndicatorClassesByColor.Length)];

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
