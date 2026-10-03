namespace GraphiteUi.Docs.Client.Components;

public partial class PopupCompatibilityDemo
{
    private string _result = "No selection";
    private string _selected = string.Empty;
    private void Choose() => _result = "Selected in " + RendererInfo.Name;
}
