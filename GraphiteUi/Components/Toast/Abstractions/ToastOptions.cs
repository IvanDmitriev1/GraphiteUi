using GraphiteUi.Common;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public sealed class ToastOptions
{
    public ThemeColor Color { get; init; } = ThemeColor.Primary;
    public string? Title { get; init; }
    public string? Message { get; init; }
    public RenderFragment? Content { get; init; }
    public int? DurationMs { get; init; }
    public bool? Dismissible { get; init; }
    public bool? PauseOnHover { get; init; }
    public string? Class { get; init; }

    public static ToastOptions Default { get; } = new();
}
