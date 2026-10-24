using System.Reflection;
using System.Text.RegularExpressions;
using GraphiteUi.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GraphiteUi.RegressionTests;

internal static class ButtonLoadingChecks
{
    public static async Task RunAsync(HtmlRenderer renderer)
    {
        string html = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var component = await renderer.RenderComponentAsync<ButtonLoadingProbe>(ParameterView.Empty);
            return component.ToHtmlString();
        });

        string native = Element(html, "loading-native");
        Require(native.Contains("type=\"submit\"", StringComparison.Ordinal), "Loading changed native Submit type.");
        Require(HasBooleanAttribute(native, "disabled"), "Loading did not disable a native button.");
        Require(native.Contains("aria-busy=\"true\"", StringComparison.Ordinal), "Loading did not expose aria-busy.");
        Require(native.Contains("data-disabled=\"false\"", StringComparison.Ordinal)
            && native.Contains("data-loading=\"true\"", StringComparison.Ordinal),
            "Loading and explicit disabled states were not emitted separately.");
        string classes = Regex.Match(native, "\\bclass=\"([^\"]*)\"").Groups[1].Value;
        Require(classes.Contains("data-[disabled=true]:bg-disabled-surface", StringComparison.Ordinal)
            && !classes.Contains("disabled:bg-disabled-surface", StringComparison.Ordinal)
            && !classes.Contains("data-disabled:bg-disabled-surface", StringComparison.Ordinal),
            "Loading or a false data-disabled flag matched generic disabled styling.");
        Require(html.Contains("Save document", StringComparison.Ordinal) && Count(html, "Save document") == 1,
            "Loading removed or duplicated the original button label.");
        Require(html.Contains("opacity-0", StringComparison.Ordinal), "Loading did not visually conceal its retained content slot.");
        Require(html.Contains("aria-hidden=\"true\"", StringComparison.Ordinal)
            && html.Contains("motion-reduce:animate-none", StringComparison.Ordinal),
            "Loading spinner is not hidden from assistive technology or lacks a reduced-motion state.");

        string icon = Element(html, "loading-icon");
        Require(icon.Contains("aria-label=\"Refresh list\"", StringComparison.Ordinal)
            && html.Contains("<span>R</span>", StringComparison.Ordinal),
            "Loading removed the icon-only button's label or accessible name.");

        string anchor = Element(html, "loading-anchor");
        Require(anchor.Contains("href=\"/caller-owned\"", StringComparison.Ordinal)
            && anchor.Contains("role=\"link\"", StringComparison.Ordinal)
            && anchor.Contains("tabindex=\"-1\"", StringComparison.Ordinal)
            && anchor.Contains("aria-disabled=\"false\"", StringComparison.Ordinal),
            "Loading changed caller-owned anchor or accessibility attributes.");

        string reset = Element(html, "idle-reset");
        Require(reset.Contains("type=\"reset\"", StringComparison.Ordinal)
            && !HasBooleanAttribute(reset, "disabled"),
            "Idle Reset lost its native reset type or became disabled.");

        await CheckCallbackGuardAsync();
    }

    private static async Task CheckCallbackGuardAsync()
    {
        var button = new UiButton();
        int invocations = 0;
        SetParameters(button, new Dictionary<string, object?>
        {
            [nameof(UiButton.IsLoading)] = true,
            [nameof(UiButton.OnClick)] = EventCallback.Factory.Create<MouseEventArgs>(new object(), _ => invocations++)
        });
        MethodInfo onClick = typeof(UiButton).GetMethod("OnClickAsync", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Button click handler was not found.");

        await (Task)onClick.Invoke(button, [new MouseEventArgs()])!;
        Require(invocations == 0, "Loading allowed a managed click callback.");

        SetParameters(button, new Dictionary<string, object?> { [nameof(UiButton.IsLoading)] = false });
        await (Task)onClick.Invoke(button, [new MouseEventArgs()])!;
        Require(invocations == 1, "An idle button did not invoke its managed click callback.");

        SetParameters(button, new Dictionary<string, object?> { [nameof(UiButton.Disabled)] = true });
        await (Task)onClick.Invoke(button, [new MouseEventArgs()])!;
        Require(invocations == 1, "An explicitly disabled button invoked its managed click callback.");
    }

    private static void SetParameters(IComponent component, Dictionary<string, object?> parameters) =>
        ParameterView.FromDictionary(parameters).SetParameterProperties(component);

    private static string Element(string html, string id)
    {
        Match match = Regex.Match(html, "<[^>]+\\bid=\"" + Regex.Escape(id) + "\"[^>]*>");
        Require(match.Success, $"Missing button element {id}.");
        return match.Value;
    }

    private static bool HasBooleanAttribute(string element, string attribute) =>
        Regex.IsMatch(element, "(?:^|\\s)" + Regex.Escape(attribute) + "(?:=\"(?:disabled|true|)\")?(?:\\s|>)", RegexOptions.IgnoreCase);

    private static int Count(string value, string token) =>
        Regex.Matches(value, Regex.Escape(token)).Count;

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
