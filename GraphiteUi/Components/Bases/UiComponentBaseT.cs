using System.Runtime.CompilerServices;

namespace GraphiteUi.Components.Bases;

public abstract class UiComponentBase<TSelf, TComponentStyle> : UiComponentBase
    where TSelf : UiComponentBase<TSelf, TComponentStyle>
    where TComponentStyle : class, IUiComponentStyle<TSelf>
{
    private protected override string RootClass => TwMerge.Merge(TComponentStyle.GetClasses(Unsafe.As<TSelf>(this)));
}