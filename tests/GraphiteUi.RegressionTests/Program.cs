using System.Globalization;
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
var merger = provider.GetRequiredService<TailwindMerge.TwMerge>();
var styledButton = merger.Merge("text-secondary-foreground text-body-sm", "custom-layout");
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
Console.WriteLine("Passed: SSR forms, accessibility, decimal precision, typed selects, popup slots and debounce lifecycle.");

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
