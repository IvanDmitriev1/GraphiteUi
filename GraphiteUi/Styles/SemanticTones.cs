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
        // Secondary — the graphite fill. Built from the neutral overlay ramp, not
        // from `surface2`, so all three states composite over whatever is behind
        // the button and step in the same direction in both themes. `bg-surface2`
        // is opaque, so its hover had to jump to an alpha value and the step size
        // was whatever the two happened to differ by.
        // It is the one tone that draws an edge: its fill sits close to the page,
        // so the border is what gives it a shape.
        "bg-neutral-10 text-foreground border-neutral-20 hover:bg-neutral-15 hover:border-neutral-30 active:bg-neutral-20",
        "bg-success text-success-foreground border-transparent hover:bg-success-hover active:bg-success-active",
        "bg-warning text-warning-foreground border-transparent hover:bg-warning-hover active:bg-warning-active",
        "bg-danger text-danger-foreground border-transparent hover:bg-danger-hover active:bg-danger-active",
        "bg-info text-info-foreground border-transparent hover:bg-info-hover active:bg-info-active"
    ];

    /// <summary>Tinted, low-emphasis tone. Used by alerts and badges.</summary>
    private static readonly string[] SubtleByColor =
    [
        "bg-surface2 text-surface2-foreground border-neutral-10",
        "bg-accent-subtle text-accent-subtle-foreground border-accent-border",
        "bg-surface2 text-surface2-foreground border-neutral-15",
        "bg-success-subtle text-success-subtle-foreground border-success-border",
        "bg-warning-subtle text-warning-subtle-foreground border-warning-border",
        "bg-danger-subtle text-danger-subtle-foreground border-danger-border",
        "bg-info-subtle text-info-subtle-foreground border-info-border"
    ];

    /// <summary>Solid accent bar / dot, for the leading indicator on alerts.</summary>
    private static readonly string[] IndicatorByColor =
    [
        "bg-neutral-50",
        "bg-accent",
        "bg-neutral-50",
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
