using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using GraphiteUI.Extensions;
using GraphiteUi.Common;
using GraphiteUi.RegressionTests;
using GraphiteUi.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging();
services.AddGraphiteUi();
await using var provider = services.BuildServiceProvider();
await using var renderer = new HtmlRenderer(provider, provider.GetRequiredService<ILoggerFactory>());
var model = new ProbeModel();
var context = new EditContext(model);
var messages = new ValidationMessageStore(context);
messages.Add(context.Field(nameof(ProbeModel.Text)), "Required");
var html = await renderer.Dispatcher.InvokeAsync(async () =>
{
    var component = await renderer.RenderComponentAsync<Probe>(ParameterView.FromDictionary(new Dictionary<string, object?>
    {
        [nameof(Probe.Model)] = model,
        [nameof(Probe.Context)] = context
    }));
    return component.ToHtmlString();
});
Assert(html.Contains("for=\"tracking\"", StringComparison.Ordinal), "Label is not linked to the input.");
Assert(html.Contains("aria-describedby=\"existing-help tracking-description\"", StringComparison.Ordinal), "Description references were not merged.");
Assert(html.Contains("aria-invalid=\"true\"", StringComparison.Ordinal), "EditContext validation state is missing.");
Assert(html.Contains("id=\"tracking-description\"", StringComparison.Ordinal), "Description was not rendered.");
Assert(html.Contains("step=\"0.01\"", StringComparison.Ordinal), "Caller numeric precision was overridden.");
var phone = Regex.Match(html, "<input[^>]*id=\"phone\"[^>]*>").Value;
Assert(phone.Contains("inputmode=\"tel\"", StringComparison.Ordinal), "Textbox dropped caller inputmode.");
Assert(phone.Contains("aria-label=\"Phone\"", StringComparison.Ordinal), "Textbox dropped caller accessible label.");
Assert(Regex.Matches(html, "id=\"location\"").Count == 1, "Select has a duplicate input ID.");
var selectPopupRoot = Regex.Match(html, "<div[^>]*data-select-root=\"true\"[^>]*>").Value;
Assert(selectPopupRoot.Contains("data-show-backdrop=\"false\"", StringComparison.Ordinal), "Select popup enables a backdrop by default.");
Assert(selectPopupRoot.Contains("data-trigger-mode=\"none\"", StringComparison.Ordinal), "Select popup trigger must defer click handling to the select.");
var defaultPopupRoot = Regex.Match(html, "<div[^>]*id=\"popup-root\"[^>]*>").Value;
Assert(defaultPopupRoot.Contains("data-show-backdrop=\"false\"", StringComparison.Ordinal), "Popup enables a backdrop by default.");
var explicitBackdropRoot = Regex.Match(html, "<div[^>]*id=\"backdrop-popup\"[^>]*>").Value;
Assert(explicitBackdropRoot.Contains("data-show-backdrop=\"true\"", StringComparison.Ordinal), "Popup cannot explicitly enable its backdrop.");
var defaultMenuRoot = Regex.Match(html, "<div[^>]*id=\"menu-root\"[^>]*>").Value;
Assert(defaultMenuRoot.Contains("data-show-backdrop=\"false\"", StringComparison.Ordinal), "Menu enables a backdrop by default.");
Assert(html.Contains("data-trigger-mode=\"open\"", StringComparison.Ordinal), "Popup ignored TriggerMode.");
Assert(html.Contains("trigger-probe", StringComparison.Ordinal) && html.Contains("content-probe", StringComparison.Ordinal)
    && html.Contains("aria-label=\"Popup details\"", StringComparison.Ordinal), "Popup slot properties did not reach their elements.");
Assert(!html.Contains("TriggerClass=", StringComparison.OrdinalIgnoreCase), "A component parameter leaked into HTML.");
Assert(html.Contains("data-value=\"11111111-1111-1111-1111-111111111111\"", StringComparison.Ordinal), "Typed select value was not preserved.");
Assert(ButtonType.Reset.ToHtmlValue() == "reset", "Reset button cannot render.");
Assert(ThemeColor.Inherit.ToAttributeValue() == "primary"
    && ThemeColor.Inherit.ToAttributeValue(preserveInherit: true) == "inherit"
    && ((ThemeColor)999).ToAttributeValue() == "primary",
    "Theme color attribute fallbacks or button inherit handling changed.");
Assert(((Size)999).ToAttributeValue() == "medium"
    && ((Size)999).ToAttributeValue(Size.Small) == "small",
    "Size attribute fallbacks no longer match their component defaults.");
Assert(((Align)999).ToAttributeValue() == "center"
    && ((Align)999).ToDrawerSideAttribute() == "start"
    && ((ToastPlacement)999).ToAttributeValue() == "top-right",
    "Alignment, drawer-side or toast-placement fallbacks changed.");
Assert(html.Contains("field-wrapper-probe px-2 px-4", StringComparison.Ordinal),
    "The input-field wrapper stopped passing consumer classes through unchanged.");

var buttonClasses = ElementClasses(html, "merge-button");
Assert(buttonClasses.SequenceEqual(["px-2", "px-4", "text-danger-subtle-foreground"], StringComparer.Ordinal),
    "Button Class was not passed through unchanged.");
var inputClasses = ElementClasses(html, "merge-input");
Assert(inputClasses.SequenceEqual(["px-2", "px-4"], StringComparer.Ordinal),
    "Checkbox Class was changed before reaching the root.");
var menuContentClasses = ElementClasses(html, "menu-content");
Assert(menuContentClasses.SequenceEqual(["min-w-52", "max-h-64"], StringComparer.Ordinal),
    "Menu content classes were not passed through unchanged.");
var menuTriggerClasses = ElementClasses(html, "menu-trigger");
Assert(menuTriggerClasses.SequenceEqual(["h-8"], StringComparer.Ordinal),
    "Forwarded menu trigger classes were not passed through unchanged.");
var defaultMenuClasses = ElementClasses(html, "default-menu-content");
Assert(defaultMenuClasses.Length == 0,
    "Menu defaults leaked into its consumer-owned content class attribute.");
var toastClasses = ElementClasses(html, "merge-toast");
Assert(toastClasses.SequenceEqual(["px-2", "px-4"], StringComparer.Ordinal),
    "Toast Class was changed before reaching the root.");
var toastRoot = Regex.Match(html, "<[^>]+\\bid=\"merge-toast\"[^>]*>").Value;
Assert(toastRoot.Contains("data-ui=\"alert\"", StringComparison.Ordinal)
    && toastRoot.Contains("data-slot=\"toast-item\"", StringComparison.Ordinal),
    "Toast's data-slot override replaced the stable UiAlert identity.");

var input = new TextProbe();
input.Configure(30);
var first = input.DebounceAsync("first");
var last = input.DebounceAsync("last");
await Task.WhenAll(first, last);
Assert(input.Value == "last", "A superseded debounce overwrote the latest input.");
var pending = input.DebounceAsync("disposed");
input.Release();
await pending;
Assert(input.Value == "last", "A disposed input committed a pending value.");
var immediate = new TextProbe();
immediate.Configure(0);
await immediate.DebounceAsync("immediate");
Assert(immediate.Value == "immediate", "Zero delay did not commit immediately.");
immediate.Release();

var culture = CultureInfo.CurrentCulture;
try
{
    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
    var number = new NumberProbe();
    Assert(number.Parse("1,25", out var result) && result == 1.25m, "Russian decimal input was interpreted as a thousands group.");
}
finally
{
    CultureInfo.CurrentCulture = culture;
}
await CssMigrationChecks.RunAsync(renderer);
await ControlTokenChecks.RunAsync(renderer);
await SkeletonStyleChecks.RunAsync(renderer);
await ButtonLoadingChecks.RunAsync(renderer);
Console.WriteLine("Passed: migrated CSS component states/parts/fallbacks/class pass-through, enum attribute normalization, SSR forms, accessibility, decimal precision, typed selects, popup slots, debounce lifecycle, input tokens, skeleton rendering and button loading.");

static string[] ElementClasses(string html, string id)
{
    var element = Regex.Match(html, "<[^>]+\\bid=\"" + Regex.Escape(id) + "\"[^>]*>").Value;
    Assert(element.Length > 0, $"Missing element: {id}.");
    var classes = Regex.Match(element, "\\bclass=\"([^\"]*)\"").Groups[1].Value;
    return WebUtility.HtmlDecode(classes).Split(' ', StringSplitOptions.RemoveEmptyEntries);
}

static void Assert(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

public sealed class ProbeModel
{
    public string Text { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal Weight { get; set; } = 1m;
    public TestId? Location { get; set; }
    public bool Checked { get; set; }
}

public readonly record struct TestId(Guid Value)
{
    public override string ToString() => Value.ToString("D");
}

internal sealed class TextProbe : UiTextbox
{
    public void Configure(int delay) => DebounceDelay = delay;
    public void Release() => Dispose(true);
}

internal sealed class NumberProbe : UiNumbox<decimal>
{
    public bool Parse(string value, out decimal result) => TryParseValueFromString(value, out result, out _);
}
