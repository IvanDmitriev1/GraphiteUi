using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class FileInputStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[384])
        .Add("flex")
        .Add("w-full")
        .Add("flex-col")
        .Add("gap-1")
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-body-sm")
        .Add("text-neutral-60")
        .ToString();

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add(CommonStyles.ControlFrameRegularClass)
        .Add("w-full")
        .Add("px-3")
        .Add("py-2")
        .Add("file:mr-3")
        .Add("file:rounded-md")
        .Add("file:border")
        .Add("file:border-neutral-10")
        .Add("file:bg-surface2")
        .Add("file:px-2")
        .Add("file:py-1")
        .Add("file:text-body-sm")
        .Add("file:text-surface2-foreground")
        .Add("file:transition-colors")
        .Add("hover:file:bg-neutral-15")
        .Add("file:cursor-pointer")
        .Add("aria-[invalid=true]:border-danger")
        .Add(Utils.FocusVisible)
        .Add(ColorVariants.Disabled.Background)
        .Add(ColorVariants.Disabled.Foreground)
        .Add(Utils.DisabledCursorDefault)
        .ToString();

    public static string InfoClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-body-sm")
        .Add("text-neutral-60")
        .ToString();
}
