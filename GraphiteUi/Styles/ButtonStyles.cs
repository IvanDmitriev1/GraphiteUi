using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class ButtonStyles
{
    private static readonly string CommonBaseClass = new CssClassBuilder(stackalloc char[384])
        .Add(Utils.InlineFlexCentered)
        .Add("font-medium appearance-none select-none relative rounded-md whitespace-nowrap subpixel-antialiased overflow-hidden cursor-pointer")
        // Border WIDTH only. The color comes from the tone, so the box is always
        // allocated but only Secondary draws an edge. Keeping `border-transparent`
        // here would leave two border-color utilities on the element, and these are
        // concatenated, not TwMerge'd — stylesheet order would decide the winner.
        .Add("border")
        // transition
        .Add("transition-colors-transform-opacity")
        .Add(Utils.MotionReduceTransitionNone)
        // focus ring — one indicator only. The old base also carried a bare
        // `focus:ring`, which drew a second ring and fired on mouse click.
        .Add(Utils.FocusVisible)
        .ToString();

    private static readonly string RootBaseClass = new CssClassBuilder(stackalloc char[384])
        .Add(CommonBaseClass)
        .Add("no-underline gap-1.5 shadow-xs")
        .ToString();

    private static readonly string SolidIconBaseClass = new CssClassBuilder(stackalloc char[384])
        .Add(CommonBaseClass)
        .Add("no-underline shadow-xs")
        .ToString();

    private static readonly string NonSolidBaseClass = new CssClassBuilder(stackalloc char[384])
        .Add(CommonBaseClass)
        .ToString();

    private static readonly string DisabledClass = new CssClassBuilder(stackalloc char[224])
        .Add("data-[disabled=true]:bg-disabled-surface data-[disabled=true]:text-disabled-foreground")
        .Add("data-[disabled=true]:shadow-none data-[disabled=true]:border-transparent")
        .Add(Utils.DataDisabledCursorDefault)
        .ToString();

    private static readonly string[] ContentSlotClasses =
    [
        "inline-flex items-center justify-center gap-1.5",
        "inline-flex items-center justify-center gap-1.5 opacity-0",
        "inline-flex items-center justify-center",
        "inline-flex items-center justify-center opacity-0"
    ];

    internal const string LoadingSpinnerContainerClass =
        "pointer-events-none absolute inset-0 inline-flex items-center justify-center";
    internal const string LoadingSpinnerClass =
        "size-4 animate-spin rounded-full border-2 border-current border-r-transparent motion-reduce:animate-none";

    private static readonly string[] SizeClasses =
    [
        "h-[var(--control-height-sm)] min-w-16 px-3 text-body-sm",
        "h-[var(--control-height-md)] min-w-20 px-4 text-body-sm",
        "h-[var(--control-height-lg)] min-w-24 px-5 text-body"
    ];

    private static readonly string[] LinkSizeClasses =
    [
        "h-[var(--control-height-sm)] text-body-sm",
        "h-[var(--control-height-md)] text-body-sm",
        "h-[var(--control-height-lg)] text-body"
    ];

    private static readonly string[] IconSizeClasses =
    [
        "size-[var(--control-height-sm)] p-0 text-body-sm",
        "size-[var(--control-height-md)] p-0 text-body-sm",
        "size-[var(--control-height-lg)] p-0 text-body"
    ];

    private static readonly int VariantCount = Enum.GetValues<ButtonVariant>().Length;
    private static readonly string[] RootClassesByVariantColorSizeIcon = BuildRootClasses();

    internal static string GetContentSlotClass(bool iconOnly, bool isLoading) =>
        ContentSlotClasses[(iconOnly ? 2 : 0) + (isLoading ? 1 : 0)];

    public static string GetRootClasses(UiButton component) =>
        GetRootClasses(component.Color, component.Size, component.Variant, component.IconOnly);

    public static string GetRootClasses(ThemeColor color, Size size) =>
        GetRootClasses(color, size, ButtonVariant.Solid, iconOnly: false);

    public static string GetRootClasses(ThemeColor color, Size size, ButtonVariant variant, bool iconOnly)
    {
        int colorIndex = (int)color;
        int sizeIndex = (int)size;
        int variantIndex = (int)variant;
        int sizeCount = SizeClasses.Length;

        if ((uint)colorIndex >= (uint)SemanticTones.Count)
        {
            colorIndex = (int)ThemeColor.Primary;
        }

        if ((uint)sizeIndex >= (uint)sizeCount)
        {
            sizeIndex = (int)Size.Medium;
        }

        if ((uint)variantIndex >= (uint)VariantCount)
        {
            variantIndex = (int)ButtonVariant.Solid;
        }

        int index = ((colorIndex * VariantCount + variantIndex) * sizeCount + sizeIndex) * 2 + (iconOnly ? 1 : 0);
        return RootClassesByVariantColorSizeIcon[index];
    }

    private static string[] BuildRootClasses()
    {
        int colorCount = SemanticTones.Count;
        int sizeCount = SizeClasses.Length;
        var rootClasses = new string[colorCount * VariantCount * sizeCount * 2];

        for (int colorIndex = 0; colorIndex < colorCount; colorIndex++)
        {
            var color = (ThemeColor)colorIndex;

            for (int variantIndex = 0; variantIndex < VariantCount; variantIndex++)
            {
                var variant = (ButtonVariant)variantIndex;

                for (int sizeIndex = 0; sizeIndex < sizeCount; sizeIndex++)
                {
                    for (int iconIndex = 0; iconIndex < 2; iconIndex++)
                    {
                        bool iconOnly = iconIndex == 1;
                        int index = ((colorIndex * VariantCount + variantIndex) * sizeCount + sizeIndex) * 2 + iconIndex;
                        rootClasses[index] = BuildRootClass(color, (Size)sizeIndex, variant, iconOnly);
                    }
                }
            }
        }

        return rootClasses;
    }

    private static string BuildRootClass(ThemeColor color, Size size, ButtonVariant variant, bool iconOnly)
    {
        string appearance = variant switch
        {
            ButtonVariant.Solid => SemanticTones.Solid(color),
            ButtonVariant.Outline => $"bg-transparent {SemanticTones.ButtonNonSolidForeground(color)} {SemanticTones.ButtonOutlineBorder(color)} {SemanticTones.ButtonSubtleStates(color)}",
            ButtonVariant.Ghost => $"bg-transparent {SemanticTones.ButtonNonSolidForeground(color)} border-transparent {SemanticTones.ButtonSubtleStates(color)}",
            ButtonVariant.Link => $"bg-transparent {SemanticTones.ButtonNonSolidForeground(color)} border-transparent",
            _ => SemanticTones.Solid(color)
        };

        string baseClasses = variant == ButtonVariant.Solid
            ? iconOnly ? SolidIconBaseClass : RootBaseClass
            : NonSolidBaseClass;

        return CssClassBuilder.Empty()
            .Add(baseClasses)
            .Add(variant == ButtonVariant.Link ? "underline underline-offset-4"
                : variant == ButtonVariant.Solid ? string.Empty : "no-underline")
            .Add(variant == ButtonVariant.Solid || iconOnly ? string.Empty : "gap-1.5")
            .Add(appearance)
            .Add(iconOnly ? IconSizeClasses[(int)size]
                : variant == ButtonVariant.Link ? LinkSizeClasses[(int)size] : SizeClasses[(int)size])
            .Add(DisabledClass)
            .ToString();
    }
}
