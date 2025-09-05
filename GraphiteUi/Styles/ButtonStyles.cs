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
        .Add("rounded")
        .Add("whitespace-nowrap")
        .Add("subpixel-antialiased")
        .Add("overflow-hidden")
        .Add("cursor-pointer")
        .Add("focus:ring")
        .Add("focus:outline-none")
        .Add("border")
        .Add("shadow-sm")
        // transition
        .Add("transition-colors-transform-opacity")
        .Add("motion-reduce:transition-none")
        // focus ring
        .Add(Utils.FocusVisible)
        .ToString();

    private static CssClassBuilder GetSizeStyles(Size size) => CssClassBuilder.Empty()
        .Add("min-w-16 py-2.5 px-3 gap-1.5 text-small", when: size is Size.Small)
        .Add("min-w-20 py-4 px-5 gap-1.5 text-medium", when: size is Size.Medium)
        .Add("min-w-24 py-5 px-6 gap-1.5 font-bold text-large", when: size is Size.Large);

    public static string GetClasses(UiButton component)
    {
        var swappedColor = component.Color == ThemeColor.Primary ? ThemeColor.Secondary : component.Color;

        return CssClassBuilder.Empty()
            .Add(Base)
            .Add(ColorVariants.Background.Solid[component.Color])
            .Add(ColorVariants.HoverBackground.Solid[swappedColor])
            .Add(ColorVariants.ActiveBackground.Solid[component.Color])
            .Add(ColorVariants.Ring.Default)
            .Add(ColorVariants.Border.Default)
            .Add(ColorVariants.Ring.Focus[component.Color])
            .Add(GetSizeStyles(component.Size))
            .Add(ColorVariants.DisabledStyles)
            .Add(component.Class)
            .ToString();
    }
}
