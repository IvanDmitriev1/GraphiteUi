using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class BreadcrumbStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("w-full")
        .ToString();

    public static string ListClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("inline-flex")
        .Add("items-center")
        .Add("gap-2")
        .Add("text-small")
        .ToString();

    public static string ItemClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("inline-flex")
        .Add("items-center")
        .Add("gap-2")
        .ToString();

    public static string LinkClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("text-primary-60")
        .Add("transition-colors")
        .Add("hover:text-foreground")
        .ToString();

    public static string CurrentClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-foreground")
        .ToString();

    public static string SeparatorClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-primary-30")
        .ToString();
}
