using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class BreadcrumbStyles
{
    public static string RootClass { get; } = "w-full";

    public static string ListClass { get; } = "inline-flex items-center gap-2 text-body-sm";

    public static string ItemClass { get; } = "inline-flex items-center gap-2";

    public static string LinkClass { get; } = "text-muted-foreground transition-colors hover:text-foreground";

    public static string CurrentClass { get; } = "text-foreground";

    public static string SeparatorClass { get; } = "text-subtle-foreground";
}
