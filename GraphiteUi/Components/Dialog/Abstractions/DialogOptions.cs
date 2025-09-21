namespace GraphiteUi.Components;

public sealed class DialogOptions
{
    public bool CloseOnEscape { get; init; } = true;
    public bool DismissOnOverlayClick { get; init; } = true;
    public string Classes { get; set; } = string.Empty;

    public static DialogOptions Default { get; } = new();
}