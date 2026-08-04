using GraphiteUi.Common;

namespace GraphiteUi.Styles;

/// <summary>
/// Fill/foreground/state class strings for each <see cref="ThemeColor"/>.
/// <para>
/// Every entry has the same shape — <c>bg-X text-X-foreground hover:bg-X-hover
/// active:bg-X-active</c> — because hover and active are now theme tokens rather
/// than references to a numbered shade. Previously each color picked its own
/// shade, and <c>-400</c> happened to be darker than the base for success and
/// warning but lighter for danger and info, so hover changed direction depending
/// on which color you asked for.
/// </para>
/// </summary>
internal static class SemanticTones
{
    /// <summary>Filled, high-emphasis tone. Used by buttons.</summary>
    private static readonly string[] SolidByColor =
    [
        // Inherit — no fill, the consumer supplies the color.
        "border-transparent",
        // Primary — the accent: a lightness inversion of the page (white fill in
        // dark, near-black fill in light).
        "bg-accent text-accent-foreground border-transparent hover:bg-accent-hover active:bg-accent-active",
        // Secondary — the graphite fill, and the one tone that draws an edge: its
        // fill sits close to the page, so the border is what gives it a shape.
        // Every step is a `secondary-*` role token, so retuning it is a CSS edit.
        "bg-secondary text-secondary-foreground border-secondary-border hover:bg-secondary-hover hover:border-secondary-border-hover active:bg-secondary-active",
        "bg-success text-success-foreground border-transparent hover:bg-success-hover active:bg-success-active",
        "bg-warning text-warning-foreground border-transparent hover:bg-warning-hover active:bg-warning-active",
        "bg-danger text-danger-foreground border-transparent hover:bg-danger-hover active:bg-danger-active",
        "bg-info text-info-foreground border-transparent hover:bg-info-hover active:bg-info-active"
    ];

    /// <summary>Tinted, low-emphasis tone. Used by alerts and badges.</summary>
    private static readonly string[] SubtleByColor =
    [
        // Inherit and Secondary share the neutral subtle tone. They used to differ
        // by a single alpha step on the border, which no role could justify.
        "bg-secondary-subtle text-secondary-subtle-foreground border-secondary-border-subtle",
        "bg-accent-subtle text-accent-subtle-foreground border-accent-border",
        "bg-secondary-subtle text-secondary-subtle-foreground border-secondary-border-subtle",
        "bg-success-subtle text-success-subtle-foreground border-success-border",
        "bg-warning-subtle text-warning-subtle-foreground border-warning-border",
        "bg-danger-subtle text-danger-subtle-foreground border-danger-border",
        "bg-info-subtle text-info-subtle-foreground border-info-border"
    ];

    /// <summary>Solid accent bar / dot, for the leading indicator on alerts.</summary>
    private static readonly string[] IndicatorByColor =
    [
        "bg-secondary-indicator",
        "bg-accent",
        "bg-secondary-indicator",
        "bg-success",
        "bg-warning",
        "bg-danger",
        "bg-info"
    ];

    public static string Solid(ThemeColor color) => Get(SolidByColor, color);

    public static string Subtle(ThemeColor color) => Get(SubtleByColor, color);

    public static string Indicator(ThemeColor color) => Get(IndicatorByColor, color);

    public static int Count => SolidByColor.Length;

    private static string Get(string[] values, ThemeColor color)
    {
        int index = (int)color;

        return (uint)index >= (uint)values.Length
            ? values[(int)ThemeColor.Primary]
            : values[index];
    }
}
