using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class RadioStyles
{
    public static string GroupClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("flex")
        .Add("flex-col")
        .Add("gap-2")
        .ToString();

    public static string ItemClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("inline-flex")
        .Add("items-center")
        .Add("gap-0")
        .Add("cursor-pointer")
        .Add("select-none")
        .Add("data-[disabled=true]:opacity-disabled")
        .Add("data-[disabled=true]:cursor-default")
        .ToString();

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("peer")
        .Add("sr-only")
        .ToString();

    public static string ControlClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("relative")
        .Add("inline-flex")
        .Add("h-5")
        .Add("w-5")
        .Add("shrink-0")
        .Add("items-center")
        .Add("justify-center")
        .Add("rounded-full")
        .Add("border")
        .Add("border-primary-20")
        .Add("bg-surface2")
        .Add("transition-[background-color,border-color,border-width,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("group-hover:border-primary-20")
        .Add("group-hover:bg-surface2")
        .Add("peer-focus-visible:ring")
        .Add("peer-focus-visible:ring-primary-5")
        .Add("peer-checked:border-primary-foreground")
        .Add("peer-checked:bg-surface2")
        .Add("peer-checked:border-[4px]")
        .Add("data-[invalid=true]:border-danger-400")
        .Add("peer-disabled:border-primary-10")
        .Add("peer-disabled:bg-primary-5")
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("ml-4")
        .Add("text-regular")
        .Add("text-foreground")
        .ToString();
}
