using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class FileInputStyles
{
    public static string RootClass { get; } = "flex w-full flex-col gap-1";

    public static string LabelClass { get; } = "text-body-sm text-muted-foreground";

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add(CommonStyles.ControlFrameRegularClass)
        .Add("w-full px-3 py-2 file:mr-3 file:rounded-md file:border file:border-border file:bg-surface2 file:px-2 file:py-1 file:text-body-sm file:text-surface2-foreground file:transition-colors hover:file:bg-surface-active file:cursor-pointer aria-[invalid=true]:border-invalid")
        .Add(Utils.FocusVisible)
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add(Utils.DisabledCursorDefault)
        .ToString();

    public static string InfoClass { get; } = "text-body-sm text-muted-foreground";
}
