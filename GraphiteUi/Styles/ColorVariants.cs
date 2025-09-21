using GraphiteUi.Common;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class ColorVariants
{
    public static readonly string PageColors = new CssClassBuilder(stackalloc char[128])
        .Add("bg-background")
        .Add("text-foreground")
        .ToString();

    public static class Surface
    {
        public static readonly string Background = new CssClassBuilder(stackalloc char[128])
            .Add("bg-surface1")
            .Add("text-surface1-foreground")
            .ToString();

        public static readonly string HoverBackground = new CssClassBuilder(stackalloc char[128])
            .Add("hover:bg-focus")
            .Add("text-surface1-foreground")
            .ToString();
    }

    public static class Border
    {
        public static readonly string Default = new CssClassBuilder(stackalloc char[128])
            .Add("border-primary-5")
            .ToString();

        public static readonly string Secondary = new CssClassBuilder(stackalloc char[128])
            .Add("border-primary")
            .ToString();
    }

    public static class Ring
    {
        public static readonly string Default = new CssClassBuilder(stackalloc char[128])
            .Add("ring:primary-5")
            .ToString();

        public static readonly IReadOnlyDictionary<ThemeColor, string> Focus = new Dictionary<ThemeColor, string>()
        {
            [ThemeColor.Inherit] = string.Empty,
            [ThemeColor.Primary] = "focus:ring-primary-5",
            [ThemeColor.Secondary] = "focus:ring-primary-5",
            [ThemeColor.Success] = "focus:ring-success-600/20",
            [ThemeColor.Warning] = "focus:ring-warning-600/20",
            [ThemeColor.Danger] = "focus:ring-danger-600/20",
            [ThemeColor.Info] = string.Empty
        };

        public static readonly IReadOnlyDictionary<ThemeColor, string> FocusWithin = new Dictionary<ThemeColor, string>()
        {
            [ThemeColor.Inherit] = string.Empty,
            [ThemeColor.Primary] = "focus-within:ring-primary-5",
            [ThemeColor.Secondary] = "focus-within:ring-primary-5",
            [ThemeColor.Success] = "focus-within:ring-success-600/20",
            [ThemeColor.Warning] = "focus-within:ring-warning-600/20",
            [ThemeColor.Danger] = "focus-within:ring-danger-600/20",
            [ThemeColor.Info] = string.Empty
        };
    }

    public static class Disabled
    {
        public const string Background = "disabled:bg-primary-5 data-disabled:bg-primary-5";
        public const string Foreground = "disabled:text-primary-20 data-disabled:text-primary-20";

        public const string GroupBackground = "group-data-disabled:bg-primary-5";
        public const string GroupForeground = "group-data-disabled:text-primary-20";
    }

    public static class Placeholder
    {
        public const string Foreground = "placeholder:text-primary-30";
    }
}