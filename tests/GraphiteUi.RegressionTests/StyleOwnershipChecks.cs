using GraphiteUi.Styles;

namespace GraphiteUi.RegressionTests;

internal static class StyleOwnershipChecks
{
    internal static void Run()
    {
        RequireEqual(CardStyles.RootClass, CommonStyles.CardClass, "CommonStyles card root alias diverged.");
        RequireEqual(CardStyles.HeaderClass, CommonStyles.CardHeaderClass, "CommonStyles card header alias diverged.");
        RequireEqual(CardStyles.BodyClass, CommonStyles.CardBodyClass, "CommonStyles card body alias diverged.");
        RequireEqual(CardStyles.FooterClass, CommonStyles.CardFooterClass, "CommonStyles card footer alias diverged.");

        RequireTokens(CardStyles.RootClass,
            "rounded-lg", "border", "border-border", "bg-surface1", "text-surface1-foreground",
            "p-4", "text-body", "shadow-sm");
        RequireTokens(CardStyles.HeaderClass, "mb-3", "text-h6", "text-foreground");
        RequireTokens(CardStyles.BodyClass, "text-body", "text-surface1-foreground");
        RequireTokens(CardStyles.FooterClass, "mt-4", "pt-3", "border-t", "border-border");
    }

    private static void RequireTokens(string classes, params string[] expected)
    {
        var tokens = classes.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string token in expected)
        {
            if (!tokens.Contains(token, StringComparer.Ordinal))
            {
                throw new InvalidOperationException($"Card styles are missing the expected {token} utility.");
            }
        }
    }

    private static void RequireEqual(string expected, string actual, string message)
    {
        if (!string.Equals(expected, actual, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(message);
        }
    }
}
