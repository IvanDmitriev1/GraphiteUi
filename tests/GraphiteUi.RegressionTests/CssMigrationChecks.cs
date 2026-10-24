using System.Net;
using System.Text.RegularExpressions;
using GraphiteUi.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GraphiteUi.RegressionTests;

internal static class CssMigrationChecks
{
    public static async Task RunAsync(HtmlRenderer renderer)
    {
        string html = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var component = await renderer.RenderComponentAsync<CssMigrationProbe>(ParameterView.Empty);
            return component.ToHtmlString();
        });

        CheckButton(html);
        CheckAlertBadgeAvatar(html);
        CheckCardBreadcrumbDivider(html);
        CheckSkeleton(html);
        CheckPopupAndMenu(html);
    }

    private static void CheckButton(string html)
    {
        var button = Element(html, "styled-button");
        Require(button.Contains("data-ui=\"button\"", StringComparison.Ordinal), "Button root identity is missing.");
        Require(button.Contains("data-slot=\"button\"", StringComparison.Ordinal), "Button data-slot was removed.");
        Require(button.Contains("data-color=\"success\"", StringComparison.Ordinal)
            && button.Contains("data-size=\"large\"", StringComparison.Ordinal)
            && button.Contains("data-variant=\"ghost\"", StringComparison.Ordinal)
            && button.Contains("data-icon-only=\"true\"", StringComparison.Ordinal),
            "Button presentation attributes are not normalized.");
        Require(button.Contains("data-disabled=\"false\"", StringComparison.Ordinal)
            && button.Contains("data-loading=\"true\"", StringComparison.Ordinal)
            && button.Contains("aria-busy=\"true\"", StringComparison.Ordinal),
            "Button boolean states were not rendered as true/false.");
        Require(Class(button) == "button-raw px-2 px-4", "Button Class was changed before reaching the root.");
        Require(Part(html, "styled-button", "content").Length > 0
            && Part(html, "styled-button", "loading").Contains("aria-hidden=\"true\"", StringComparison.Ordinal)
            && Part(html, "styled-button", "spinner").Length > 0,
            "Button content and loading parts are incomplete.");

        var fallback = Element(html, "fallback-button");
        Require(fallback.Contains("data-color=\"primary\"", StringComparison.Ordinal)
            && fallback.Contains("data-size=\"medium\"", StringComparison.Ordinal)
            && fallback.Contains("data-variant=\"solid\"", StringComparison.Ordinal),
            "Invalid button enums did not retain their primary/medium/solid fallbacks.");
        Require(Element(html, "inherit-button").Contains("data-color=\"inherit\"", StringComparison.Ordinal),
            "Button Inherit was normalized to a filled color.");
    }

    private static void CheckAlertBadgeAvatar(string html)
    {
        var alert = Element(html, "styled-alert");
        Require(alert.Contains("data-ui=\"alert\"", StringComparison.Ordinal)
            && alert.Contains("data-slot=\"alert\"", StringComparison.Ordinal)
            && alert.Contains("data-color=\"danger\"", StringComparison.Ordinal)
            && alert.Contains("role=\"alert\"", StringComparison.Ordinal),
            "Alert root identity, slot, color or danger role is missing.");
        Require(Class(alert) == "alert-raw px-2 px-4", "Alert Class was merged or rewritten.");
        Require(Part(html, "styled-alert", "start-content").Length > 0
            && Part(html, "styled-alert", "content").Length > 0
            && Part(html, "styled-alert", "title").Length > 0
            && Part(html, "styled-alert", "body").Length > 0
            && Part(html, "styled-alert", "dismiss").Length > 0,
            "Alert part markers are incomplete.");
        Require(Element(html, "fallback-alert").Contains("data-color=\"primary\"", StringComparison.Ordinal),
            "Invalid/Inherited alert colors did not fall back to primary.");

        var badge = Element(html, "styled-badge");
        Require(badge.Contains("data-ui=\"badge\"", StringComparison.Ordinal)
            && badge.Contains("data-color=\"info\"", StringComparison.Ordinal)
            && badge.Contains("data-size=\"large\"", StringComparison.Ordinal)
            && Class(badge) == "badge-raw px-2 px-4",
            "Badge identity, presentation state or raw Class pass-through is incorrect.");
        var fallbackBadge = Element(html, "fallback-badge");
        Require(fallbackBadge.Contains("data-color=\"primary\"", StringComparison.Ordinal)
            && fallbackBadge.Contains("data-size=\"small\"", StringComparison.Ordinal),
            "Badge invalid enums did not retain primary/small fallbacks.");

        var avatar = Element(html, "styled-avatar");
        Require(avatar.Contains("data-ui=\"avatar\"", StringComparison.Ordinal)
            && avatar.Contains("data-size=\"large\"", StringComparison.Ordinal)
            && Class(avatar) == "avatar-raw px-2 px-4"
            && Part(html, "styled-avatar", "image").Contains("src=\"ada.png\"", StringComparison.Ordinal),
            "Avatar root, image part or raw Class pass-through is incorrect.");
        Require(Element(html, "fallback-avatar").Contains("data-size=\"medium\"", StringComparison.Ordinal),
            "Invalid avatar size did not retain its medium fallback.");
    }

    private static void CheckCardBreadcrumbDivider(string html)
    {
        var card = Element(html, "styled-card");
        Require(card.Contains("data-ui=\"card\"", StringComparison.Ordinal)
            && Class(card) == "card-raw px-2 px-4"
            && Part(html, "styled-card", "header").Length > 0
            && Part(html, "styled-card", "title").Length > 0
            && Part(html, "styled-card", "body").Length > 0
            && Part(html, "styled-card", "footer").Length > 0,
            "Card identity, parts or raw Class pass-through is incorrect.");

        var breadcrumb = Element(html, "styled-breadcrumb");
        Require(breadcrumb.Contains("data-ui=\"breadcrumb\"", StringComparison.Ordinal)
            && Class(breadcrumb) == "breadcrumb-raw px-2 px-4"
            && Part(html, "styled-breadcrumb", "list").Length > 0,
            "Breadcrumb root/list markers or raw Class pass-through is incorrect.");
        Require(html.Contains("data-ui=\"breadcrumb-item\"", StringComparison.Ordinal)
            && html.Contains("data-current=\"true\"", StringComparison.Ordinal)
            && Part(html, "styled-breadcrumb", "separator").Length > 0,
            "Breadcrumb item identity/current state/separator markers are incomplete.");

        var vertical = Element(html, "vertical-divider");
        var horizontal = Element(html, "horizontal-divider");
        Require(vertical.Contains("data-ui=\"divider\"", StringComparison.Ordinal)
            && vertical.Contains("data-orientation=\"vertical\"", StringComparison.Ordinal)
            && Class(vertical) == "divider-raw h-6"
            && horizontal.Contains("data-orientation=\"horizontal\"", StringComparison.Ordinal)
            && Class(horizontal) == "divider-raw w-10",
            "Divider orientation or raw Class pass-through is incorrect.");
    }

    private static void CheckSkeleton(string html)
    {
        var root = Element(html, "mimic-root");
        Require(root.Contains("data-ui=\"skeleton\"", StringComparison.Ordinal)
            && root.Contains("data-loading=\"true\"", StringComparison.Ordinal)
            && root.Contains("aria-busy=\"true\"", StringComparison.Ordinal)
            && Part(html, "mimic-root", "content").Contains("data-skeleton-mimic=\"true\"", StringComparison.Ordinal),
            "Skeleton loading or auto-mimic state is missing.");
        Require(!Part(html, "custom-skeleton-root", "content").Contains("data-skeleton-mimic=\"true\"", StringComparison.Ordinal),
            "Custom skeleton content unexpectedly received auto-mimic state.");

        var block = Element(html, "styled-skeleton-block");
        var line = Element(html, "styled-skeleton-line");
        Require(block.Contains("data-ui=\"skeleton-block\"", StringComparison.Ordinal)
            && block.Contains("aria-hidden=\"true\"", StringComparison.Ordinal)
            && Class(block) == "block-raw h-10"
            && line.Contains("data-ui=\"skeleton-line\"", StringComparison.Ordinal)
            && Class(line) == "line-raw h-6",
            "Skeleton parts or raw Class pass-through are incorrect.");
    }

    private static void CheckPopupAndMenu(string html)
    {
        var popup = Element(html, "styled-popup");
        Require(popup.Contains("data-ui=\"popup\"", StringComparison.Ordinal)
            && popup.Contains("data-slot=\"popup-root\"", StringComparison.Ordinal)
            && popup.Contains("data-close-escape=\"false\"", StringComparison.Ordinal)
            && popup.Contains("data-show-backdrop=\"false\"", StringComparison.Ordinal)
            && Class(popup) == "popup-raw px-2 px-4",
            "Popup root identity, boolean state or raw Class pass-through is incorrect.");
        Require(Part(html, "styled-popup", "trigger").Contains("class=\"trigger-raw h-8\"", StringComparison.Ordinal)
            && Part(html, "styled-popup", "backdrop").Contains("class=\"backdrop-raw z-30\"", StringComparison.Ordinal)
            && Part(html, "styled-popup", "content").Contains("class=\"content-raw min-w-52\"", StringComparison.Ordinal),
            "Popup slot classes were merged or changed.");

        var menu = Element(html, "styled-menu");
        Require(menu.Contains("data-ui=\"popup\"", StringComparison.Ordinal)
            && menu.Contains("data-menu=\"true\"", StringComparison.Ordinal)
            && menu.Contains("data-slot=\"popup-root\"", StringComparison.Ordinal)
            && Class(menu) == "menu-raw px-2 px-4",
            "Menu did not preserve its menu identity on the popup root.");
        Require(Part(html, "styled-menu", "trigger").Contains("class=\"menu-trigger-raw h-8\"", StringComparison.Ordinal)
            && Part(html, "styled-menu", "content").Contains("class=\"menu-content-raw min-w-56\"", StringComparison.Ordinal)
            && Part(html, "styled-menu", "items").Length > 0,
            "Menu slot pass-through or item-list part is incorrect.");
        var item = Element(html, "styled-menu-item");
        Require(item.Contains("data-ui=\"menu-item\"", StringComparison.Ordinal)
            && item.Contains("data-disabled=\"true\"", StringComparison.Ordinal)
            && item.Contains("data-slot=\"menu-item\"", StringComparison.Ordinal)
            && Class(item) == "item-raw px-2 px-4",
            "Menu item disabled state or raw Class pass-through is incorrect.");
    }

    private static string Part(string html, string id, string part)
    {
        string segment = ComponentFragment(html, id);
        Match match = Regex.Match(segment, "<[^>]*data-part=\"" + Regex.Escape(part) + "\"[^>]*>");
        return match.Success ? WebUtility.HtmlDecode(match.Value) : string.Empty;
    }

    private static string ComponentFragment(string html, string id)
    {
        Match root = Regex.Match(html, "<([A-Za-z][A-Za-z0-9:-]*)[^>]*\\bid=\"" + Regex.Escape(id) + "\"[^>]*>");
        Require(root.Success, $"Missing component root {id}.");
        string[] voidTags = ["area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr"];
        int depth = 0;
        int end = root.Index;
        foreach (Match tag in Regex.Matches(html[root.Index..], "</?[A-Za-z][^>]*>"))
        {
            string value = tag.Value;
            bool closing = value.StartsWith("</", StringComparison.Ordinal);
            string name = Regex.Match(value, "</?([A-Za-z][A-Za-z0-9:-]*)").Groups[1].Value;
            if (closing)
            {
                depth--;
                if (depth == 0)
                {
                    end = root.Index + tag.Index + tag.Length;
                    break;
                }
            }
            else if (!value.EndsWith("/>", StringComparison.Ordinal) && !voidTags.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                depth++;
            }
            else if (depth == 0)
            {
                end = root.Index + tag.Index + tag.Length;
                break;
            }
        }
        return html[root.Index..end];
    }

    private static string Element(string html, string id)
    {
        Match match = Regex.Match(html, "<[^>]+\\bid=\"" + Regex.Escape(id) + "\"[^>]*>");
        Require(match.Success, $"Missing component root {id}.");
        return WebUtility.HtmlDecode(match.Value);
    }

    private static string Class(string element) =>
        WebUtility.HtmlDecode(Regex.Match(element, "\\bclass=\"([^\"]*)\"").Groups[1].Value);

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
