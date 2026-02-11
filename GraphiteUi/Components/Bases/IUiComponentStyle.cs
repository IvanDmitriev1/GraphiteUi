namespace GraphiteUi.Components.Bases;

public interface IUiComponentStyle<in TComponent>
    where TComponent : IUiComponent
{
    public static abstract string GetCoreClasses(TComponent component);
}
