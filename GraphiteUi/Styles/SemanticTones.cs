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
        // Inherit — contributes nothing, the consumer supplies the color.
        string.Empty,
        // Primary — the accent. Was a 10%-white ghost.
        "bg-accent text-accent-foreground hover:bg-accent-hover active:bg-accent-active",
        // Secondary — neutral, low emphasis. Was a near-black slab that could not
        // survive a light theme.
        "bg-surface2 text-surface2-foreground hover:bg-neutral-10 active:bg-neutral-15",
        "bg-success text-success-foreground hover:bg-success-hover active:bg-success-active",
        "bg-warning text-warning-foreground hover:bg-warning-hover active:bg-warning-active",
        "bg-danger text-danger-foreground hover:bg-danger-hover active:bg-danger-active",
        "bg-info text-info-foreground hover:bg-info-hover active:bg-info-active"
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
