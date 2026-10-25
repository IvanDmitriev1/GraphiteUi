using GraphiteUi.Common;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Docs.Client.Pages;

public partial class ThemeLab
{
    [SupplyParameterFromQuery] public bool Sample { get; set; }
    [SupplyParameterFromQuery] public string? Theme { get; set; }
    [SupplyParameterFromQuery] public string? Palette { get; set; }

    private bool IsLight => Theme == "light";
    private string PreviewPalette => Palette is "blue" or "emerald" ? Palette : "graphite";
    private string PaletteTitle => PreviewPalette switch
    {
        "blue" => "Vivid blue",
        "emerald" => "Emerald green",
        _ => "Graphite"
    };
    private string PaletteDetailsUrl => SampleUrl(IsLight ? "light" : "dark") + "#palette-details";
    private string ProjectName { get; set; } = "Graphite workspace";
    private string Search { get; set; } = string.Empty;
    private bool Notifications { get; set; } = true;
    private bool AutoSave { get; set; } = true;
    private static readonly ThemeColor[] StatusColors =
        [ThemeColor.Success, ThemeColor.Warning, ThemeColor.Danger, ThemeColor.Info];

    private string SampleUrl(string theme) =>
        $"/theme-lab?sample=true&theme={theme}&palette={PreviewPalette}";

    private string ComparisonUrl(string anchor) =>
        PaletteUrl(PreviewPalette) + $"#{anchor}";

    private string PaletteUrl(string palette) =>
        $"/theme-lab?theme={(IsLight ? "light" : "dark")}&palette={palette}";

    private string ThemeUrl(string theme) =>
        $"/theme-lab?theme={theme}&palette={PreviewPalette}";

    private static string StatusTitle(ThemeColor color) => color switch
    {
        ThemeColor.Success => "Changes saved",
        ThemeColor.Warning => "Review before continuing",
        ThemeColor.Danger => "Something needs attention",
        _ => "An update is available"
    };
}
