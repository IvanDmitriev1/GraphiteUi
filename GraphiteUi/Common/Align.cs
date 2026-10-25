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
    public static string ToAttributeValue(this Align align) => align switch
    {
        Align.Start => "start",
        Align.End => "end",
        _ => "center"
    };

    public static string ToDrawerSideAttribute(this Align side) => side == Align.End ? "end" : "start";
}
