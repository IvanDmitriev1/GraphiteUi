using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class DialogStyles
{
    public static string Classes { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("p-6 box-border overflow-auto")
        // Was `rounded` (4px) + `border-2` while every other container used
        // `rounded-lg` + a 1px border. Dialog is the largest surface, so it takes
        // the largest radius.
        .Add("rounded-xl border shadow-2xl m-auto w-full max-w-[min(100dvw-2rem,42rem)] max-h-[min(100dvh-2rem,80vh)] outline-none focus:outline-none focus-visible:outline-none")
        .Add(ColorVariants.Border.Strong)
        .Add(ColorVariants.Surface.PopupBackground)
        .Add("backdrop:bg-scrim backdrop:backdrop-blur-[2px]")
        .ToString();
}
