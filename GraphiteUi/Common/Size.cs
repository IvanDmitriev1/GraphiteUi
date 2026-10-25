namespace GraphiteUi.Common;

/// <summary>
/// Specifies size options for a component.
/// </summary>
public enum Size
{
    /// <summary>
    /// A small size.
    /// </summary>
    Small,

    /// <summary>
    /// A medium size.
    /// </summary>
    Medium,

    /// <summary>
    /// A large size.
    /// </summary>
    Large
}

public static class SizeExtensions
{
    public static string ToAttributeValue(this Size size, Size fallback = Size.Medium) => size switch
    {
        Size.Small => "small",
        Size.Large => "large",
        Size.Medium => "medium",
        _ => fallback == Size.Small ? "small" : fallback == Size.Large ? "large" : "medium"
    };
}
