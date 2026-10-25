namespace GraphiteUi.Common;

public enum ButtonVariant
{
    Solid,
    Outline,
    Ghost,
    Link
}

public static class ButtonVariantExtensions
{
    public static string ToAttributeValue(this ButtonVariant variant) => variant switch
    {
        ButtonVariant.Outline => "outline",
        ButtonVariant.Ghost => "ghost",
        ButtonVariant.Link => "link",
        _ => "solid"
    };
}
