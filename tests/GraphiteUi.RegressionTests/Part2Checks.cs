using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GraphiteUi.RegressionTests;

internal static class Part2Checks
{
    public static async Task RunAsync(HtmlRenderer renderer)
    {
        string html = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var component = await renderer.RenderComponentAsync<Part2Probe>(ParameterView.Empty);
            return component.ToHtmlString();
        });

        string field = ElementWithAttribute(html, "data-ui", "input-field");
        string inputWrapper = ElementWithAttribute(html, "data-slot", "input-wrapper");
        string fieldInput = Element(html, "field-probe");
        Require(Class(field) == "field-raw px-2 px-4", "Input-field Class no longer passes through on its wrapper.");
        Require(inputWrapper.Contains("data-part=\"wrapper\"", StringComparison.Ordinal)
            && inputWrapper.Contains("data-size=\"small\"", StringComparison.Ordinal)
            && inputWrapper.Contains("data-active=\"true\"", StringComparison.Ordinal),
            "Textbox wrapper lost its CSS parts, normalized size or active state.");
        Require(fieldInput.Contains("data-part=\"input\"", StringComparison.Ordinal)
            && fieldInput.Contains("aria-describedby=\"field-probe-description\"", StringComparison.Ordinal),
            "Textbox input part or description association is missing.");
        Require(html.Contains("id=\"field-probe-description\" data-part=\"description\"", StringComparison.Ordinal)
            && html.Contains("data-slot=\"label\"", StringComparison.Ordinal),
            "Textbox label or description markup was lost.");

        string number = Element(html, "number-probe");
        Require(number.Contains("type=\"number\"", StringComparison.Ordinal)
            && number.Contains("data-part=\"input\"", StringComparison.Ordinal),
            "Numbox lost its native numeric input or CSS part.");

        string fileRoot = ElementWithAttribute(html, "data-ui", "file-input");
        string fileInput = Element(html, "file-probe");
        Require(Class(fileRoot) == "file-raw px-2 px-4"
            && fileRoot.Contains("data-part=\"label\"", StringComparison.Ordinal),
            "File input root or label part is missing, or its Class was changed.");
        Require(fileInput.Contains("type=\"file\"", StringComparison.Ordinal)
            && fileInput.Contains("data-part=\"input\"", StringComparison.Ordinal)
            && HasBooleanAttribute(fileInput, "disabled")
            && fileInput.Contains("aria-invalid=\"true\"", StringComparison.Ordinal),
            "File input native disabled state, invalid ARIA value or CSS part is missing.");

        string checkbox = Element(html, "checkbox-probe");
        string checkboxInput = ElementWithAttribute(html, "data-slot", "checkbox-input");
        Require(checkbox.Contains("data-ui=\"checkbox\"", StringComparison.Ordinal)
            && checkbox.Contains("data-checked=\"true\"", StringComparison.Ordinal)
            && checkbox.Contains("data-slot=\"checkbox\"", StringComparison.Ordinal),
            "Checkbox stable root, slot or state attributes are missing.");
        Require(checkboxInput.Contains("data-part=\"input\"", StringComparison.Ordinal)
            && HasBooleanAttribute(checkboxInput, "checked"),
            "Checkbox appearance state is not represented by its native checked input.");

        string toggle = Element(html, "switch-probe");
        string switchRoot = ElementWithAttribute(html, "data-ui", "switch");
        Require(toggle.Contains("data-ui=\"switch\"", StringComparison.Ordinal)
            && switchRoot.Contains("data-part=\"track\"", StringComparison.Ordinal)
            && switchRoot.Contains("data-part=\"thumb\"", StringComparison.Ordinal),
            "Switch root or track/thumb parts are missing.");
        string switchInput = Tags(html, "data-part", "input")
            .Single(input => input.Contains("type=\"checkbox\"", StringComparison.Ordinal)
                && !input.Contains("data-slot=\"checkbox-input\"", StringComparison.Ordinal));
        Require(HasBooleanAttribute(switchInput, "checked")
            && switchInput.Contains("aria-disabled=\"false\"", StringComparison.Ordinal)
            && switchInput.Contains("aria-invalid=\"true\"", StringComparison.Ordinal)
            && switchInput.Contains("aria-readonly=\"false\"", StringComparison.Ordinal),
            "Switch checked state or normalized true/false ARIA values are missing.");

        string radioGroup = Element(html, "radio-group-probe");
        string[] radios = Tags(html, "data-ui", "radio");
        Require(radioGroup.Contains("data-ui=\"radio-group\"", StringComparison.Ordinal)
            && radioGroup.Contains("data-slot=\"radio-group\"", StringComparison.Ordinal)
            && radioGroup.Contains("role=\"radiogroup\"", StringComparison.Ordinal),
            "Radio group identity, slot or accessibility role is missing.");
        Require(radios.Length == 2 && radios.All(radio => radio.Contains("data-part=\"item\"", StringComparison.Ordinal)),
            "Radio option roots or CSS part markers are missing.");
        string selectedRadio = Tags(html, "data-part", "input")
            .Single(input => input.Contains("type=\"radio\"", StringComparison.Ordinal));
        Require(HasBooleanAttribute(selectedRadio, "checked"),
            "Selected radio appearance state is not represented by its native checked input.");

        string drawerHost = ElementWithAttribute(html, "data-ui", "drawer");
        string drawerOverlay = ElementWithAttribute(html, "data-slot", "drawer-root");
        Require(drawerHost.Contains("data-part=\"host\"", StringComparison.Ordinal)
            && drawerOverlay.Contains("data-part=\"overlay\"", StringComparison.Ordinal)
            && drawerOverlay.Contains("data-side=\"start\"", StringComparison.Ordinal)
            && Class(drawerOverlay) == "drawer-raw px-2 px-4",
            "Drawer root, fallback placement or raw Class pass-through is incorrect.");
        Require(html.Contains("data-part=\"toggle\"", StringComparison.Ordinal)
            && html.Contains("data-part=\"backdrop\"", StringComparison.Ordinal)
            && html.Contains("data-part=\"panel\"", StringComparison.Ordinal),
            "Drawer toggle, backdrop or panel part is missing.");

        string tooltip = Element(html, "tooltip-probe");
        Require(tooltip.Contains("data-ui=\"tooltip\"", StringComparison.Ordinal)
            && tooltip.Contains("data-align=\"center\"", StringComparison.Ordinal)
            && tooltip.Contains("data-bottom=\"true\"", StringComparison.Ordinal)
            && Class(tooltip) == "tooltip-raw px-2 px-4",
            "Tooltip root, normalized placement or raw Class pass-through is incorrect.");
        Require(ElementWithAttribute(html, "data-slot", "tooltip-bubble").Contains("data-part=\"bubble\"", StringComparison.Ordinal),
            "Tooltip bubble part or compatibility slot is missing.");

        string toast = Element(html, "toast-probe");
        Require(toast.Contains("data-ui=\"alert\"", StringComparison.Ordinal)
            && toast.Contains("data-slot=\"toast-item\"", StringComparison.Ordinal)
            && toast.Contains("data-toast=\"true\"", StringComparison.Ordinal)
            && Class(toast) == "toast-raw px-2 px-4",
            "Toast lost Alert identity, its toast marker/slot, or raw Class pass-through.");

        string toastHost = Element(html, "toast-host-probe");
        Require(toastHost.Contains("data-ui=\"toast-host\"", StringComparison.Ordinal)
            && toastHost.Contains("data-slot=\"toast-host\"", StringComparison.Ordinal)
            && toastHost.Contains("data-placement=\"top-right\"", StringComparison.Ordinal)
            && Class(toastHost) == "toast-host-raw px-2 px-4",
            "Toast host lost its identity, fallback placement or raw Class pass-through.");
        Require(ElementWithAttribute(html, "data-slot", "toast-stack").Contains("data-part=\"stack\"", StringComparison.Ordinal),
            "Toast stack CSS part or compatibility slot is missing.");
    }

    private static string Element(string html, string id)
    {
        Match match = Regex.Match(html, "<[^>]+\\bid=\"" + Regex.Escape(id) + "\"[^>]*>");
        Require(match.Success, $"Missing element {id}.");
        return WebUtility.HtmlDecode(match.Value);
    }

    private static string ElementWithAttribute(string html, string attribute, string value)
    {
        Match match = Regex.Match(html, "<[^>]*\\b" + Regex.Escape(attribute) + "=\"" + Regex.Escape(value) + "\"[^>]*>");
        Require(match.Success, $"Missing element with {attribute}={value}.");
        return WebUtility.HtmlDecode(match.Value);
    }

    private static string[] Tags(string html, string attribute, string value) =>
        Regex.Matches(html, "<[^>]*\\b" + Regex.Escape(attribute) + "=\"" + Regex.Escape(value) + "\"[^>]*>")
            .Select(match => WebUtility.HtmlDecode(match.Value))
            .ToArray();

    private static string Class(string element) =>
        WebUtility.HtmlDecode(Regex.Match(element, "\\bclass=\"([^\"]*)\"").Groups[1].Value);

    private static bool HasBooleanAttribute(string element, string attribute) =>
        Regex.IsMatch(element, "(?:^|\\s)" + Regex.Escape(attribute) + "(?:=\"(?:disabled|checked|true|)\")?(?:\\s|>)", RegexOptions.IgnoreCase);

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
