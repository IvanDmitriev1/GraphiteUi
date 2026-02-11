using GraphiteUi.Components.Bases;

namespace GraphiteUi.Components.Checkbox;

internal interface ICheckBoxStyles : IUiComponentStyle<UiCheckbox>
{
    public static abstract string WrapperClass { get; }
    public static abstract string InputClass { get; }
    public static abstract string ControlClass { get; }
    public static abstract string IconClass { get; }
    public static abstract string LabelClass { get; }
}
