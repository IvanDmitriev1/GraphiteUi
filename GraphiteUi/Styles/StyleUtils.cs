using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class StyleUtils
{
    public static string VisuallyHidden { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("sr-only")
        .ToString();
}