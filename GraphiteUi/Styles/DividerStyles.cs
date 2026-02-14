using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class DividerStyles
{
    public static string HorizontalClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("h-px")
        .Add("w-full")
        .Add("bg-divider")
        .ToString();

    public static string VerticalClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("h-full")
        .Add("min-h-4")
        .Add("w-px")
        .Add("bg-divider")
        .ToString();
}
