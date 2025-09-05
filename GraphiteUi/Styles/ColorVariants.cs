using GraphiteUi.Common;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class ColorVariants
{
    public static class Background
    {
        public static readonly IReadOnlyDictionary<ThemeColor, string> Solid = new Dictionary<ThemeColor, string>()
        {
            [ThemeColor.Inherit] = string.Empty,
            [ThemeColor.Primary] = "bg-primary text-primary-foreground",
            [ThemeColor.Secondary] = "bg-secondary text-secondary-foreground",
            [ThemeColor.Success] = "bg-success text-success-foreground",
            [ThemeColor.Warning] = "bg-warning text-warning-foreground",
            [ThemeColor.Danger] = "bg-danger text-danger-foreground",
            [ThemeColor.Info] = "bg-info text-info-foreground"
        };
    }

    public static class HoverBackground
    {
        public static readonly IReadOnlyDictionary<ThemeColor, string> Solid = new Dictionary<ThemeColor, string>()
        {
            [ThemeColor.Inherit] = string.Empty,
            [ThemeColor.Primary] = "hover:bg-primary-15 hover:text-primary-foreground",
            [ThemeColor.Secondary] = "hover:bg-white hover:text-secondary-foreground",
            [ThemeColor.Success] = "hover:bg-success-400 hover:text-success-foreground",
            [ThemeColor.Warning] = "hover:bg-warning-400 hover:text-warning-foreground",
            [ThemeColor.Danger] = "hover:bg-danger-400 hover:text-danger-foreground",
            [ThemeColor.Info] = "hover:bg-info-400 hover:text-info-foreground"
        };
    }

    public static class ActiveBackground
    {
        public static readonly IReadOnlyDictionary<ThemeColor, string> Solid = new Dictionary<ThemeColor, string>()
        {
            [ThemeColor.Inherit] = string.Empty,
            [ThemeColor.Primary] = "active:bg-white active:text-black",
            [ThemeColor.Secondary] = "active:bg-primary active:text-primary-foreground",
            [ThemeColor.Success] = "active:bg-success-500 active:text-success-foreground",
            [ThemeColor.Warning] = "active:bg-warning-500 active:text-warning-foreground",
            [ThemeColor.Danger] = "active:bg-danger-500 active:text-danger-foreground",
            [ThemeColor.Info] = "active:bg-info-500 active:text-info-foreground"
        };
    }

    public static class Border
    {
        public static readonly string Default = new CssClassBuilder(stackalloc char[128])
            .Add("border-primary-5")
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
    }
    
    public static readonly string DisabledStyles = new CssClassBuilder(stackalloc char[64])
        .Add("disabled:bg-primary-5")
        .Add("disabled:text-primary-20")
        .Add("disabled:cursor-default")
        .ToString();
}