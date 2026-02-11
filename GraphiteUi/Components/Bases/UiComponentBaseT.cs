using GraphiteUi.Utilities;
using System.Runtime.CompilerServices;

namespace GraphiteUi.Components.Bases;

public abstract class UiComponentBase<TSelf, TComponentStyle> : UiComponentBase
    where TSelf : UiComponentBase<TSelf, TComponentStyle>
    where TComponentStyle : class, IUiComponentStyle<TSelf>
{
    private protected override string RootClass => RootClassMergeCache.GetOrAdd(
        TwMerge,
        TComponentStyle.GetCoreClasses(Unsafe.As<TSelf>(this)),
        Unsafe.As<TSelf>(this).Class);
}
