using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Components.Bases;
using GraphiteUi.Utilities;
using Size = GraphiteUi.Common.Size;

namespace GraphiteUi.Styles;

public sealed class ButtonStyles : IUiComponentStyle<UiButton>
{
    private static readonly string Base = new CssClassBuilder(stackalloc char[256])
        .Add("inline-flex")
        .Add("items-center")
        .Add("justify-center")
        .Add("min-w-max")
        .Add("font-normal")
        .Add("appearance-none")
        .Add("select-none")
        .Add("rounded-md")
        .Add("whitespace-nowrap")
        .Add("subpixel-antialiased")
        .Add("overflow-hidden")
        .Add("cursor-pointer")
        .Add("border")
        .Add("shadow-sm")
        // transition
        .Add("transition-colors-transform-opacity")
        .Add("motion-reduce:transition-none")
        // focus ring
        .Add("focus:ring")
        .Add(Utils.FocusVisible)
        .ToString();

    private static CssClassBuilder GetSizeStyles(Size size) => CssClassBuilder.Empty()
        .Add("min-w-16 py-2.5 px-3 gap-1.5 text-small", when: size is Size.Small)
        .Add("min-w-20 py-4 px-5 gap-1.5 text-medium", when: size is Size.Medium)
        .Add("min-w-24 py-5 px-6 gap-1.5 font-bold text-large", when: size is Size.Large);

    private static readonly IReadOnlyDictionary<ThemeColor, string> Background = new Dictionary<ThemeColor, string>()
    {
        [ThemeColor.Inherit] = string.Empty,
        [ThemeColor.Primary] = "bg-primary text-primary-foreground",
        [ThemeColor.Secondary] = "bg-secondary text-secondary-foreground",
        [ThemeColor.Success] = "bg-success text-success-foreground",
        [ThemeColor.Warning] = "bg-warning text-warning-foreground",
        [ThemeColor.Danger] = "bg-danger text-danger-foreground",
        [ThemeColor.Info] = "bg-info text-info-foreground"
    };

    private static readonly IReadOnlyDictionary<ThemeColor, string> HoverBackground = new Dictionary<ThemeColor, string>()
    {
        [ThemeColor.Inherit] = string.Empty,
        [ThemeColor.Primary] = "hover:bg-white hover:text-secondary-foreground",
        [ThemeColor.Secondary] = "hover:bg-primary-15 hover:text-primary-foreground",
        [ThemeColor.Success] = "hover:bg-success-400 hover:text-success-foreground",
        [ThemeColor.Warning] = "hover:bg-warning-400 hover:text-warning-foreground",
        [ThemeColor.Danger] = "hover:bg-danger-400 hover:text-danger-foreground",
        [ThemeColor.Info] = "hover:bg-info-400 hover:text-info-foreground"
    };

    private static readonly IReadOnlyDictionary<ThemeColor, string> ActiveBackground = new Dictionary<ThemeColor, string>()
    {
        [ThemeColor.Inherit] = string.Empty,
        [ThemeColor.Primary] = "active:bg-white active:text-black",
        [ThemeColor.Secondary] = "active:bg-primary active:text-primary-foreground",
        [ThemeColor.Success] = "active:bg-success-500 active:text-success-foreground",
        [ThemeColor.Warning] = "active:bg-warning-500 active:text-warning-foreground",
        [ThemeColor.Danger] = "active:bg-danger-500 active:text-danger-foreground",
        [ThemeColor.Info] = "active:bg-info-500 active:text-info-foreground"
    };

    public static string GetClasses(UiButton component)
    {
        return CssClassBuilder.Empty()
            .Add(Base)
            .Add(Background[component.Color])
            .Add(HoverBackground[component.Color])
            .Add(ActiveBackground[component.Color])
            .Add(ColorVariants.Ring.Default)
            .Add(ColorVariants.Border.Default)
            .Add(ColorVariants.Ring.Focus[component.Color])
            .Add(GetSizeStyles(component.Size))
            .Add(ColorVariants.DisabledStyles)
            .Add(component.Class)
            .ToString();
    }
}
