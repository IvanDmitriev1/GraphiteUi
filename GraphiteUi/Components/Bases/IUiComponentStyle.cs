namespace GraphiteUi.Components.Bases;

public interface IUiComponentStyle<in TComponent>
    where TComponent : UiComponentBase
{
    public static abstract string GetClasses(TComponent component);
}