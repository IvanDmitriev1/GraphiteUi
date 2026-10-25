using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GraphiteUi.RegressionTests;

internal static class ControlTokenChecks
{
    public static async Task RunAsync(HtmlRenderer renderer)
    {
        string html = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var component = await renderer.RenderComponentAsync<ControlTokenProbe>(ParameterView.Empty);
            return component.ToHtmlString();
        });

        string[] inputWrappers = Tags(html, "data-slot", "input-wrapper");
        Require(inputWrappers.Length == 6, "Control token probe did not render each input size, type and fallback.");
        string[] expectedSizes = ["small", "medium", "large", "small", "medium", "medium"];
        for (int index = 0; index < inputWrappers.Length; index++)
        {
            Require(inputWrappers[index].Contains("data-part=\"wrapper\"", StringComparison.Ordinal)
                && inputWrappers[index].Contains($"data-size=\"{expectedSizes[index]}\"", StringComparison.Ordinal),
                $"Input wrapper {index} emitted the wrong CSS size state.");
        }

        string inputLabel = LabelElement(html, "control-small");
        string input = InputElement(html, "control-small");
        string smallWrapper = Wrapper(html, "control-small");
        Require(inputLabel.Contains("Small input", StringComparison.Ordinal)
            && input.Contains("placeholder=\"Enter text\"", StringComparison.Ordinal)
            && smallWrapper.Contains("data-active=\"true\"", StringComparison.Ordinal),
            "Placeholder input lost its linked floating-label state.");
        Require(LabelElement(html, "control-medium").Contains("Medium input", StringComparison.Ordinal)
            && InputElement(html, "control-medium").Contains("aria-readonly=\"true\"", StringComparison.Ordinal)
            && Wrapper(html, "control-medium").Contains("data-active=\"false\"", StringComparison.Ordinal),
            "Empty read-only input lost its linked label or inactive state.");
        Require(LabelElement(html, "control-large").Contains("Large input", StringComparison.Ordinal)
            && InputElement(html, "control-large").Contains("aria-disabled=\"true\"", StringComparison.Ordinal)
            && Wrapper(html, "control-large").Contains("data-active=\"true\"", StringComparison.Ordinal),
            "Disabled populated input lost its linked floating-label state.");
        Require(LabelElement(html, "control-number").Contains("Number input", StringComparison.Ordinal)
            && InputElement(html, "control-number").Contains("value=\"1.25\"", StringComparison.Ordinal)
            && Wrapper(html, "control-number").Contains("data-active=\"true\"", StringComparison.Ordinal),
            "Populated number input lost its linked floating-label state.");

        string[] selectTriggers = Tags(html, "data-slot", "popup-trigger");
        Require(selectTriggers.Length == 5, "Select token probe did not render all five trigger sizes and state cases.");
        Require(selectTriggers.All(trigger => trigger.Contains("data-part=\"trigger\"", StringComparison.Ordinal)),
            "Select's migrated Popup trigger is missing its CSS part marker.");

        string smallInput = InputElement(html, "select-small");
        string smallLabel = LabelElement(html, "select-small");
        string smallRoot = SelectRoot(html, "select-small");
        Require(smallLabel.Contains("Small select", StringComparison.Ordinal)
            && smallLabel.Contains("data-part=\"label\"", StringComparison.Ordinal)
            && smallLabel.Contains("for=\"select-small\"", StringComparison.Ordinal),
            "Select should render a visible linked label with its part marker.");
        Require(smallRoot.Contains("data-size=\"small\"", StringComparison.Ordinal)
            && smallRoot.Contains("data-ui=\"select\"", StringComparison.Ordinal),
            "Select did not emit its normalized size and root identity.");
        Require(Regex.Matches(html, "\\bid=\"select-small\"").Count == 1,
            "Select emitted a duplicate custom input id.");
        Require(smallInput.Contains("aria-describedby=\"caller-help select-small-description\"", StringComparison.Ordinal),
            "Select did not merge its description id with caller aria-describedby.");
        Require(smallRoot.Contains("data-invalid=\"true\"", StringComparison.Ordinal),
            "Select's invalid state is missing from its CSS root.");

        foreach (string id in new[] { "select-small", "select-medium", "select-large", "select-fallback", "select-no-label" })
        {
            string expected = id switch
            {
                "select-small" => "small",
                "select-large" => "large",
                _ => "medium"
            };
            Require(SelectRoot(html, id).Contains($"data-size=\"{expected}\"", StringComparison.Ordinal),
                $"Select {id} emitted a non-normalized size state.");
        }

        string[] selectRootTags = Tags(html, "data-select-root", "true");
        Require(selectRootTags.Length == 5, "Select root probe did not render every Popup root.");
        string selectRoot = selectRootTags[0];
        Require(selectRoot.Contains("data-ui=\"popup\"", StringComparison.Ordinal)
            && WebUtility.HtmlDecode(Regex.Match(selectRoot, "\\bclass=\"([^\"]*)\"").Groups[1].Value)
                == "select-root-probe px-2 px-4",
            "Select lost Popup identity or changed its raw Class pass-through.");

        Require(InputElement(html, "select-medium").Contains("aria-readonly=\"true\"", StringComparison.Ordinal),
            "Readonly Select lost its non-editable input state.");
        Require(InputElement(html, "select-large").Contains("aria-disabled=\"true\"", StringComparison.Ordinal),
            "Disabled Select lost its non-editable input state.");
        string noLabel = InputElement(html, "select-no-label");
        Require(noLabel.Contains("aria-label=\"Caller select label\"", StringComparison.Ordinal)
            && Regex.Matches(html, "<label[^>]*for=\"select-no-label\"").Count == 0,
            "Select without Label did not preserve the caller accessible name.");
    }

    private static string[] Tags(string html, string attribute, string value) =>
        Regex.Matches(html, "<[^>]*\\b" + Regex.Escape(attribute) + "=\"" + Regex.Escape(value) + "\"[^>]*>")
            .Select(match => WebUtility.HtmlDecode(match.Value))
            .ToArray();

    private static string InputElement(string html, string id)
    {
        Match match = Regex.Match(html, "<input[^>]*\\bid=\"" + Regex.Escape(id) + "\"[^>]*>");
        Require(match.Success, $"Missing Select input {id}.");
        return WebUtility.HtmlDecode(match.Value);
    }

    private static string LabelElement(string html, string inputId)
    {
        Match match = Regex.Match(html, "<label[^>]*\\bfor=\"" + Regex.Escape(inputId) + "\"[^>]*>.*?</label>");
        return match.Success ? WebUtility.HtmlDecode(match.Value) : string.Empty;
    }

    private static string SelectRoot(string html, string inputId)
    {
        int inputIndex = html.IndexOf($"id=\"{inputId}\"", StringComparison.Ordinal);
        Require(inputIndex >= 0, $"Missing Select input {inputId}.");
        int rootIndex = html.LastIndexOf("<div data-ui=\"select\"", inputIndex, StringComparison.Ordinal);
        Require(rootIndex >= 0, $"Missing Select root for {inputId}.");
        Match match = Regex.Match(html[rootIndex..], "<div[^>]*>");
        Require(match.Success, $"Missing Select root tag for {inputId}.");
        return WebUtility.HtmlDecode(match.Value);
    }

    private static string Wrapper(string html, string inputId)
    {
        int inputIndex = html.IndexOf($"id=\"{inputId}\"", StringComparison.Ordinal);
        Require(inputIndex >= 0, $"Missing input {inputId}.");
        int wrapperIndex = html.LastIndexOf("data-part=\"wrapper\"", inputIndex, StringComparison.Ordinal);
        Require(wrapperIndex >= 0, $"Missing input wrapper for {inputId}.");
        int start = html.LastIndexOf("<div", wrapperIndex, StringComparison.Ordinal);
        int end = html.IndexOf('>', wrapperIndex);
        Require(start >= 0 && end >= 0, $"Missing input wrapper tag for {inputId}.");
        return WebUtility.HtmlDecode(html[start..(end + 1)]);
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
