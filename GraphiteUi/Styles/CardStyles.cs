using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class CardStyles
{
    public static string RootClass { get; } = CommonStyles.CardClass;

    public static string HeaderClass { get; } = CommonStyles.CardHeaderClass;

    public static string BodyClass { get; } = CommonStyles.CardBodyClass;

    public static string FooterClass { get; } = CommonStyles.CardFooterClass;

    public static string TitleClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-medium")
        .Add("text-foreground")
        .ToString();
}
