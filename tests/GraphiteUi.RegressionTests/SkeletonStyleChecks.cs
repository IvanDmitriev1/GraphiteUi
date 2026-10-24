using System.Net;
using System.Text.RegularExpressions;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GraphiteUi.RegressionTests;

internal static class SkeletonStyleChecks
{
    private const string BlockClassOverride = "w-48 h-10 bg-[image:url(/block.png)] bg-[length:12px_8px] animate-[pulse_2s_linear_infinite]";
    private const string LineClassOverride = "w-56 h-6 bg-[image:url(/line.png)] bg-[length:16px_10px] animate-[bounce_3s_ease-in-out_infinite]";

    public static async Task RunAsync(HtmlRenderer renderer, TailwindMerge.TwMerge merger)
    {
        var html = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var component = await renderer.RenderComponentAsync<SkeletonProbe>(ParameterView.Empty);
            return component.ToHtmlString();
        });

        CheckLoadingStates(html);
        CheckBlockAndLine(html, merger);
    }

    private static void CheckLoadingStates(string html)
    {
        AssertLoadingState(html, "automatic-root", "true");
        AssertLoadingState(html, "loaded-root", "false");
        AssertLoadingState(html, "custom-root", "true");
        AssertLoadingState(html, "loaded-template-root", "false");

        AssertRendered(html, "automatic-child", true);
        AssertRendered(html, "loaded-child", true);
        AssertRendered(html, "custom-child", false);
        AssertRendered(html, "custom-skeleton", true);
        AssertRendered(html, "loaded-template-child", true);
        AssertRendered(html, "loaded-template-skeleton", false);

        var boundControls = ElementWithId(html, "bound-controls-root");
        Assert(boundControls.Contains("data-loading=\"true\"", StringComparison.Ordinal),
            "Bound controls are not rendered inside a loading skeleton.");
        Assert(html.Contains("data-slot=\"input-wrapper\"", StringComparison.Ordinal),
            "Skeleton probe Textbox is missing its input wrapper slot.");
        Assert(html.Contains("data-slot=\"popup-trigger\"", StringComparison.Ordinal),
            "Skeleton probe Select is missing its popup trigger slot.");
        Assert(Count(html, "data-slot=\"label\"") >= 2,
            "Bound Textbox and Select labels are missing from the skeleton probe.");

        var automaticContent = ParentDivClassTokens(html, "automatic-child");
        AssertTokens(automaticContent, "graphite-skeleton-mimic");
        Assert(Count(html, "graphite-skeleton-mimic") == 3,
            "Only automatically mimicked loading content should receive the named marker.");
        var nativeContent = ElementWithId(html, "native-content-root");
        Assert(nativeContent.Contains("data-loading=\"true\"", StringComparison.Ordinal),
            "Native content fixture is not inside a loading skeleton.");
        foreach (var id in new[] { "native-label", "native-notes", "native-select", "native-paragraph", "native-inline", "native-strong", "native-em", "native-code", "native-image", "native-icon", "native-button", "button-inline", "button-icon" })
            Assert(html.Contains($"id=\"{id}\"", StringComparison.Ordinal),
                $"Native skeleton fixture is missing {id}.");
        Assert(!ElementWithId(html, "outside-native-label").Contains("graphite-skeleton-mimic", StringComparison.Ordinal),
            "Content outside auto-mimic was unexpectedly changed.");
        Assert(!automaticContent.Any(token => token.StartsWith("[&", StringComparison.Ordinal)),
            "Automatic skeleton content still carries descendant utility selectors.");
    }

    private static void AssertLoadingState(string html, string id, string expected)
    {
        var root = ElementWithId(html, id);
        Assert(root.Contains($"data-loading=\"{expected}\"", StringComparison.Ordinal),
            $"Skeleton {id} rendered the wrong data-loading value.");
        Assert(root.Contains($"aria-busy=\"{expected}\"", StringComparison.Ordinal),
            $"Skeleton {id} rendered the wrong aria-busy value.");
    }

    private static void CheckBlockAndLine(string html, TailwindMerge.TwMerge merger)
    {
        var defaultBlock = ElementWithId(html, "default-block");
        var defaultLine = ElementWithId(html, "default-line");
        var block = ElementWithId(html, "block");
        var line = ElementWithId(html, "line");
        AssertTokens(ClassTokens(defaultBlock), "bg-[image:var(--skeleton-shimmer-image)]",
            "bg-[length:var(--skeleton-shimmer-size)]", "animate-[var(--skeleton-shimmer-animation)]", "motion-reduce:animate-none");
        AssertTokens(ClassTokens(defaultLine), "bg-[image:var(--skeleton-shimmer-image)]",
            "bg-[length:var(--skeleton-shimmer-size)]", "animate-[var(--skeleton-shimmer-animation)]", "motion-reduce:animate-none");
        Assert(block.Contains("aria-hidden=\"true\"", StringComparison.Ordinal), "Skeleton block is not hidden from assistive technology.");
        Assert(line.Contains("aria-hidden=\"true\"", StringComparison.Ordinal), "Skeleton line is not hidden from assistive technology.");

        var blockTokens = ClassTokens(block);
        var lineTokens = ClassTokens(line);
        AssertMergedClasses(blockTokens, SkeletonStyles.BlockClass, BlockClassOverride, merger, "block");
        AssertMergedClasses(lineTokens, SkeletonStyles.LineClass, LineClassOverride, merger, "line");

        AssertTokens(blockTokens,
            "w-48", "h-10", "bg-[image:url(/block.png)]", "bg-[length:12px_8px]", "animate-[pulse_2s_linear_infinite]");
        AssertTokens(lineTokens,
            "w-56", "h-6", "bg-[image:url(/line.png)]", "bg-[length:16px_10px]", "animate-[bounce_3s_ease-in-out_infinite]");
        AssertNoTokens(blockTokens,
            "w-full", "bg-[image:var(--skeleton-shimmer-image)]", "bg-[length:var(--skeleton-shimmer-size)]",
            "animate-[var(--skeleton-shimmer-animation)]");
        AssertNoTokens(lineTokens,
            "w-full", "h-4", "bg-[image:var(--skeleton-shimmer-image)]", "bg-[length:var(--skeleton-shimmer-size)]",
            "animate-[var(--skeleton-shimmer-animation)]");
    }

    private static void AssertMergedClasses(
        string[] actual,
        string core,
        string consumer,
        TailwindMerge.TwMerge merger,
        string component)
    {
        var expected = (merger.Merge(core, consumer) ?? string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Assert(actual.SequenceEqual(expected, StringComparer.Ordinal),
            $"Skeleton {component} did not render the configured TailwindMerge result.");
    }

    private static string ElementWithId(string html, string id)
    {
        var match = Regex.Match(html, "<[^>]+\\bid=\"" + Regex.Escape(id) + "\"[^>]*>");
        Assert(match.Success, $"Skeleton probe is missing element {id}.");
        return WebUtility.HtmlDecode(match.Value);
    }

    private static string[] ParentDivClassTokens(string html, string childId)
    {
        var childIndex = html.IndexOf($"id=\"{childId}\"", StringComparison.Ordinal);
        Assert(childIndex >= 0, $"Skeleton probe is missing child {childId}.");
        var parentStart = html.LastIndexOf("<div", childIndex, StringComparison.Ordinal);
        Assert(parentStart >= 0, $"Skeleton child {childId} has no content wrapper.");
        var parentEnd = html.IndexOf('>', parentStart);
        Assert(parentEnd >= 0, $"Skeleton child {childId} has a malformed content wrapper.");
        return ClassTokens(WebUtility.HtmlDecode(html[parentStart..(parentEnd + 1)]));
    }

    private static string[] ClassTokens(string tag)
    {
        var match = Regex.Match(tag, "\\bclass=\"([^\"]*)\"");
        Assert(match.Success, "Skeleton element is missing its class attribute.");
        return WebUtility.HtmlDecode(match.Groups[1].Value)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }

    private static void AssertRendered(string html, string id, bool expected)
    {
        var rendered = html.Contains($"id=\"{id}\"", StringComparison.Ordinal);
        Assert(rendered == expected, $"Skeleton rendered {id} when expected state was {expected}.");
    }

    private static int Count(string value, string token) =>
        Regex.Matches(value, Regex.Escape(token)).Count;

    private static void AssertTokens(string[] actual, params string[] expected)
    {
        foreach (var token in expected)
        {
            Assert(actual.Contains(token, StringComparer.Ordinal), $"Skeleton class is missing {token}.");
        }
    }

    private static void AssertNoTokens(string[] actual, params string[] unexpected)
    {
        foreach (var token in unexpected)
        {
            Assert(!actual.Contains(token, StringComparer.Ordinal), $"Skeleton class retained conflicting utility {token}.");
        }
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
