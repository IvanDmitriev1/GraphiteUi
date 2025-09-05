namespace GraphiteUi.Extensions;

public static class BooleanExtensions
{
    public static string ToAttributeValue(this bool value) => value ? "true" : "false";
}