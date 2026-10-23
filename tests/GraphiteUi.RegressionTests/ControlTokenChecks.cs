using System.Net;
using System.Text.RegularExpressions;
using GraphiteUi.Common;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GraphiteUi.RegressionTests;

internal static class ControlTokenChecks
{
    private static readonly string[] HeightTokens =
    [
        "h-[var(--control-height-sm)]",
        "h-[var(--control-height-md)]",
        "h-[var(--control-height-lg)]"
    ];

    private static readonly string[] MinHeightTokens =
    [
        "min-h-[var(--control-height-sm)]",
        "min-h-[var(--control-height-md)]",
        "min-h-[var(--control-height-lg)]"
    ];

    public static async Task RunAsync(HtmlRenderer renderer, TailwindMerge.TwMerge merger)
    {
        string html = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var component = await renderer.RenderComponentAsync<ControlTokenProbe>(ParameterView.Empty);
            return component.ToHtmlString();
        });
        string[] inputWrappers = InputWrapperClasses(html);
        Assert(inputWrappers.Length == 5, "Control token probe did not render each input size and fallback.");

        for (int sizeIndex = 0; sizeIndex < HeightTokens.Length; sizeIndex++)
        {
            var size = (Size)sizeIndex;
            AssertToken(ButtonStyles.GetRootClasses(ThemeColor.Primary, size), HeightTokens[sizeIndex],
                $"Button size {size} is missing its inherited control height.");
        }

        AssertEqual(ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium),
            ButtonStyles.GetRootClasses(ThemeColor.Primary, (Size)(-1)), "Negative button size did not fall back to medium.");
        AssertEqual(ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium),
            ButtonStyles.GetRootClasses(ThemeColor.Primary, (Size)99), "Out-of-range button size did not fall back to medium.");

        for (int wrapperIndex = 0; wrapperIndex < inputWrappers.Length; wrapperIndex++)
        {
            int sizeIndex = wrapperIndex < HeightTokens.Length ? wrapperIndex : (int)Size.Medium;
            AssertToken(inputWrappers[wrapperIndex], HeightTokens[sizeIndex],
                $"Input wrapper {wrapperIndex} is missing its inherited control height.");
            AssertToken(inputWrappers[wrapperIndex], MinHeightTokens[sizeIndex],
                $"Input wrapper {wrapperIndex} is missing its matching minimum height.");
        }

        Assert(!HasToken(inputWrappers[3], HeightTokens[(int)Size.Small])
            && !HasToken(inputWrappers[3], MinHeightTokens[(int)Size.Small]), "Negative input size did not fall back to medium.");
        Assert(!HasToken(inputWrappers[4], HeightTokens[(int)Size.Large])
            && !HasToken(inputWrappers[4], MinHeightTokens[(int)Size.Large]), "Out-of-range input size did not fall back to medium.");

        AssertToken(PopupStyles.TriggerClass, HeightTokens[(int)Size.Medium],
            "Popup trigger is missing its inherited medium control height.");
        AssertMergeOverride(merger, ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Small), "h-[37px]",
            HeightTokens[(int)Size.Small], null, "Button height override did not replace the control token.");
        AssertMergeOverride(merger, inputWrappers[1], "h-[42px]",
            HeightTokens[(int)Size.Medium], MinHeightTokens[(int)Size.Medium], "Input height override did not replace the control token.");
        AssertMergeOverride(merger, PopupStyles.TriggerClass, "h-[36px]",
            HeightTokens[(int)Size.Medium], null, "Popup trigger height override did not replace the control token.");

        const string backdropOpen = "data-[state=open]:duration-[var(--popup-backdrop-duration)]";
        const string backdropClosed = "data-[state=closed]:duration-[var(--popup-backdrop-duration)]";
        const string contentOpen = "data-[state=open]:duration-[var(--popup-content-duration)]";
        const string contentClosed = "data-[state=closed]:duration-[var(--popup-content-duration)]";
        AssertMergeOverride(merger, PopupStyles.BackdropClass, "data-[state=open]:duration-[360ms]",
            backdropOpen, backdropClosed, "Open backdrop duration override changed the wrong state or did not apply.");
        AssertMergeOverride(merger, PopupStyles.BackdropClass, "data-[state=closed]:duration-[180ms]",
            backdropClosed, backdropOpen, "Closed backdrop duration override changed the wrong state or did not apply.");
        AssertMergeOverride(merger, PopupStyles.ContentClass, "data-[state=open]:duration-[360ms]",
            contentOpen, contentClosed, "Open popup content duration override changed the wrong state or did not apply.");
        AssertMergeOverride(merger, PopupStyles.ContentClass, "data-[state=closed]:duration-[180ms]",
            contentClosed, contentOpen, "Closed popup content duration override changed the wrong state or did not apply.");
    }

    private static string[] InputWrapperClasses(string html)
    {
        MatchCollection matches = Regex.Matches(html, "<div[^>]*data-slot=\"input-wrapper\"[^>]*>");
        var classes = new string[matches.Count];

        for (int index = 0; index < matches.Count; index++)
        {
            Match classMatch = Regex.Match(matches[index].Value, "\\bclass=\"([^\"]*)\"");
            Assert(classMatch.Success, "Rendered input wrapper is missing its class attribute.");
            classes[index] = WebUtility.HtmlDecode(classMatch.Groups[1].Value);
        }

        return classes;
    }

    private static void AssertMergeOverride(
        TailwindMerge.TwMerge merger,
        string defaults,
        string consumerClass,
        string replacedClass,
        string? retainedClass,
        string message)
    {
        string merged = merger.Merge(defaults, consumerClass) ?? string.Empty;
        AssertToken(merged, consumerClass, message);
        Assert(!HasToken(merged, replacedClass), message);
        if (retainedClass is not null)
        {
            AssertToken(merged, retainedClass, message);
        }
    }

    private static bool HasToken(string classes, string token) =>
        classes.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(token, StringComparer.Ordinal);

    private static void AssertToken(string classes, string token, string message) =>
        Assert(HasToken(classes, token), message);

    private static void AssertEqual(string expected, string actual, string message) =>
        Assert(string.Equals(expected, actual, StringComparison.Ordinal), message);

    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
