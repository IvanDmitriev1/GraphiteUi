using GraphiteUi.Styles;

namespace GraphiteUi.RegressionTests;

internal static class StyleCompositionChecks
{
    public static void Run()
    {
        var popup = Tokens(PopupStyles.ContentClass);
        var menu = Tokens(MenuStyles.ContentClass);

        RequireOnlyMinimumWidth(popup, "min-w-40", "Popup");
        RequireOnlyMinimumWidth(menu, "min-w-44", "Menu");
        Require(WithoutMinimumWidth(popup).SequenceEqual(WithoutMinimumWidth(menu)),
            "Popup and menu content differ beyond their minimum widths.");

        RequireInOrder(popup,
            "absolute",
            "left-0",
            "top-[calc(100%+0.5rem)]",
            "z-50",
            "bg-surface3",
            "text-surface3-foreground",
            "transition-[opacity,translate]",
            "motion-reduce:transition-none",
            "data-[state=open]:opacity-100",
            "data-[state=open]:translate-y-0",
            "data-[state=closed]:opacity-0",
            "data-[state=closed]:translate-y-1");
    }

    private static string[] Tokens(string classes) => classes.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    private static string[] WithoutMinimumWidth(string[] tokens) =>
        tokens.Where(token => !token.StartsWith("min-w-", StringComparison.Ordinal)).ToArray();

    private static void RequireOnlyMinimumWidth(string[] tokens, string expected, string component)
    {
        var widths = tokens.Where(token => token.StartsWith("min-w-", StringComparison.Ordinal)).ToArray();
        Require(widths.Length == 1 && widths[0] == expected,
            $"{component} content must contain only {expected} as its minimum-width utility.");
    }

    private static void RequireInOrder(string[] tokens, params string[] expected)
    {
        var position = -1;
        foreach (var utility in expected)
        {
            var nextPosition = Array.IndexOf(tokens, utility, position + 1);
            Require(nextPosition > position, $"Content is missing or misorders the {utility} utility.");
            position = nextPosition;
        }
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
