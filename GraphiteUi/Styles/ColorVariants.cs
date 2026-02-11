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
            .Add("ring-primary-5")
            .ToString();

        private static readonly string[] FocusByThemeColor =
        [
            string.Empty,
            "focus:ring-primary-5",
            "focus:ring-primary-5",
            "focus:ring-success-600/20",
            "focus:ring-warning-600/20",
            "focus:ring-danger-600/20",
            string.Empty
        ];

        private static readonly string[] FocusWithinByThemeColor =
        [
            string.Empty,
            "focus-within:ring-primary-5",
            "focus-within:ring-primary-5",
            "focus-within:ring-success-600/20",
            "focus-within:ring-warning-600/20",
            "focus-within:ring-danger-600/20",
            string.Empty
        ];

        public static string Focus(ThemeColor color) =>
            GetByThemeColor(FocusByThemeColor, color, nameof(Focus));

        public static string FocusWithin(ThemeColor color) =>
            GetByThemeColor(FocusWithinByThemeColor, color, nameof(FocusWithin));
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

    private static string GetByThemeColor(string[] values, ThemeColor color, string mapName)
    {
        int index = (int)color;

        if ((uint)index >= (uint)values.Length)
        {
            throw new ArgumentOutOfRangeException(
                nameof(color),
                color,
                $"ThemeColor '{color}' is outside of {mapName} map bounds ({values.Length}).");
        }

        return values[index];
    }
}
