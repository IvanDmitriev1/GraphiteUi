namespace GraphiteUi.Common;

/// <summary>
/// Specifies alignment options of a component.
/// </summary>
public enum Align
{
    /// <summary>
    /// Alignment to the start.
    /// </summary>
    Start,

    /// <summary>
    /// Alignment to the center.
    /// </summary>
    Center,

    /// <summary>
    /// Alignment to the end.
    /// </summary>
    End
}

public static class AlignExtensions
{
    public static string ToTailwindCss(this Align align) => align switch
    {
        Align.Center => "text-center",
        Align.Start => "text-start",
        Align.End => "text-end",
        _ => throw new ArgumentOutOfRangeException(nameof(align), align, null)
    };
}