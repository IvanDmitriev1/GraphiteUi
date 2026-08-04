using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class DialogStyles
{
    public static string Classes { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("p-6")
        // Was `rounded` (4px) + `border-2` while every other container used
        // `rounded-lg` + a 1px border. Dialog is the largest surface, so it takes
        // the largest radius.
        .Add("rounded-xl")
        .Add("border")
        .Add("shadow-2xl")
        .Add("m-auto")
        .Add("w-full")
        .Add("max-w-[min(100dvw-2rem,42rem)]")
        .Add("max-h-[min(100dvh-2rem,80vh)]")
        .Add("outline-none")
        .Add("focus:outline-none")
        .Add("focus-visible:outline-none")
        .Add(ColorVariants.Border.Strong)
        .Add(ColorVariants.Surface.PopupBackground)
        .Add("backdrop:bg-scrim")
        .Add("backdrop:backdrop-blur-[2px]")
        .ToString();
}
