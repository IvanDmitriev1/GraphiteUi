using GraphiteUi.Common;

namespace GraphiteUi.Components;

public interface IUiInputFieldStyles : IComponentStyles
{
    public string InputWrapperClass { get; }
    public string LabelClass { get; }
    public string InputClass { get; }
}