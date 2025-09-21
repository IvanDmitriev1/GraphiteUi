using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class DialogStyles
{
    public static string Classes { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("p-5")
        .Add("rounded")
        .Add("border-2")
        .Add("drop-shadow-xl")
        .Add("m-auto")
        .Add("w-full")
        .Add("outline-none")
        .Add("focus:outline-none")
        .Add("focus-visible:outline-none")
        .Add(ColorVariants.Border.Secondary)
        .Add(ColorVariants.PageColors)
        .Add("backdrop:bg-background/80")
        .ToString();
}