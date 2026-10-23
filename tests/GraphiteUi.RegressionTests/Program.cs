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
using TailwindMerge.Models;

var services = new ServiceCollection();
services.AddLogging();
services.AddGraphiteUi();
await using var provider = services.BuildServiceProvider();
var merger = provider.GetRequiredService<TailwindMerge.TwMerge>();
var standardMerge = merger.Merge("p-2", "p-4");
Assert(standardMerge == "p-4", "Standard spacing conflicts did not keep the last utility.");
var responsiveMerge = merger.Merge("sm:hover:p-2", "sm:hover:p-4");
Assert(responsiveMerge == "sm:hover:p-4", "Responsive and state modifiers did not merge within the same variant.");
var modifierMerge = merger.Merge("hover:bg-red-500 focus:bg-red-500", "hover:bg-blue-500") ?? string.Empty;
Assert(modifierMerge.Contains("focus:bg-red-500", StringComparison.Ordinal)
    && modifierMerge.Contains("hover:bg-blue-500", StringComparison.Ordinal)
    && !modifierMerge.Contains("hover:bg-red-500", StringComparison.Ordinal),
    "State conflicts removed a class from another state.");
var arbitraryMerge = merger.Merge("w-[13px]", "w-[14px]");
Assert(arbitraryMerge == "w-[14px]", "Arbitrary width utilities did not conflict.");

var configuredServices = new ServiceCollection();
configuredServices.AddGraphiteUi(config =>
{
    config.CacheSize = 32;
    config.Extend(new ExtendedConfig
    {
        ConflictingClassGroups = new Dictionary<string, string[]>
        {
            ["font-size"] = ["text-color"]
        }
    });
});
await using var configuredProvider = configuredServices.BuildServiceProvider();
var configuredMerger = configuredProvider.GetRequiredService<TailwindMerge.TwMerge>();
Assert(configuredMerger.Merge("text-red-500", "text-body-sm") == "text-body-sm",
    "The consumer merge callback did not apply its custom conflict-group override.");

var styledButton = merger.Merge("text-secondary-foreground text-body-sm", "custom-layout") ?? string.Empty;
Assert(styledButton.Split(' ').Contains("text-secondary-foreground") && styledButton.Split(' ').Contains("text-body-sm"),
    "Consumer layout classes caused Graphite typography to remove its semantic foreground color.");
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
Assert(html.Contains("data-trigger-mode=\"open\"", StringComparison.Ordinal), "Popup ignored TriggerMode.");
Assert(html.Contains("trigger-probe", StringComparison.Ordinal) && html.Contains("content-probe", StringComparison.Ordinal)
    && html.Contains("aria-label=\"Popup details\"", StringComparison.Ordinal), "Popup slot properties did not reach their elements.");
Assert(!html.Contains("TriggerClass=", StringComparison.OrdinalIgnoreCase), "A component parameter leaked into HTML.");
Assert(html.Contains("data-value=\"11111111-1111-1111-1111-111111111111\"", StringComparison.Ordinal), "Typed select value was not preserved.");
Assert(ButtonType.Reset.ToHtmlValue() == "reset", "Reset button cannot render.");
Assert(html.Contains("field-wrapper-probe px-2 px-4", StringComparison.Ordinal),
    "The input-field wrapper stopped passing consumer classes through unchanged.");

var buttonClasses = ElementClasses(html, "merge-button");
Assert(buttonClasses.Contains("px-4") && !buttonClasses.Contains("px-2"),
    "Button consumer spacing did not override built-in classes.");
var inputClasses = ElementClasses(html, "merge-input");
Assert(inputClasses.Contains("px-4") && !inputClasses.Contains("px-2"),
    "Input consumer spacing did not use the configured merger.");
var menuContentClasses = ElementClasses(html, "menu-content");
Assert(menuContentClasses.Contains("min-w-52") && !menuContentClasses.Contains("min-w-44")
    && !menuContentClasses.Contains("min-w-40"), "Menu content overrides did not replace menu and popup defaults.");
var menuTriggerClasses = ElementClasses(html, "menu-trigger");
Assert(menuTriggerClasses.Contains("h-8") && !menuTriggerClasses.Contains("h-[var(--control-height-md)]"),
    "Forwarded menu trigger classes did not override popup defaults.");
var defaultMenuClasses = ElementClasses(html, "default-menu-content");
Assert(defaultMenuClasses.Contains("min-w-44") && !defaultMenuClasses.Contains("min-w-40"),
    "The menu content delta did not replace the popup minimum width.");
var toastClasses = ElementClasses(html, "merge-toast");
Assert(toastClasses.Contains("px-4") && !toastClasses.Contains("px-2"),
    "Toast consumer spacing did not override the composed alert classes.");

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
StyleVariantChecks.Run();
StyleCompositionChecks.Run();
StyleOwnershipChecks.Run();
await ControlTokenChecks.RunAsync(renderer, merger);
await SkeletonStyleChecks.RunAsync(renderer, merger);
Console.WriteLine("Passed: Tailwind merging, menu composition, SSR forms, accessibility, decimal precision, typed selects, popup slots, debounce lifecycle, style variants, style composition, card ownership, control tokens and skeleton rendering/overrides.");

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
