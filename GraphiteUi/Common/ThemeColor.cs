namespace GraphiteUi.Common;

public enum ThemeColor
{
    Inherit,
    Primary,
    Secondary,
    Success,
    Warning,
    Danger,
    Info,
}

public static class ThemeColorExtensions
{
    public static string ToAttributeValue(this ThemeColor color, bool preserveInherit = false) => color switch
    {
        ThemeColor.Inherit when preserveInherit => "inherit",
        ThemeColor.Secondary => "secondary",
        ThemeColor.Success => "success",
        ThemeColor.Warning => "warning",
        ThemeColor.Danger => "danger",
        ThemeColor.Info => "info",
        _ => "primary"
    };
}
