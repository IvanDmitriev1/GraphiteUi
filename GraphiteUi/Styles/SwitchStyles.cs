using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class SwitchStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("relative")
        .Add("inline-flex")
        .Add("items-center")
        .Add("gap-2")
        .Add("cursor-pointer")
        .Add("select-none")
        .Add("data-[disabled=true]:opacity-disabled")
        .Add("data-[disabled=true]:cursor-default")
        .ToString();

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("peer")
        .Add("sr-only")
        .ToString();

    public static string TrackClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("relative")
        .Add("inline-flex")
        .Add("h-6")
        .Add("w-11")
        .Add("shrink-0")
        .Add("overflow-hidden")
        .Add("items-center")
        .Add("rounded-full")
        .Add("border")
        .Add("border-primary-15")
        .Add("bg-primary-10")
        .Add("hover:border-primary-20")
        .Add("hover:bg-primary-20")
        .Add("transition-[background-color,border-color,box-shadow]")
        .Add("duration-200")
        .Add("ease-in-out")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("peer-checked:bg-primary-foreground")
        .Add("peer-checked:border-background")
        .Add("peer-checked:hover:bg-primary-foreground")
        .Add("peer-checked:hover:border-background")
        .Add("peer-checked:shadow-[0_0_0_2px_var(--color-background)]")
        .Add("peer-focus-visible:outline")
        .Add("peer-focus-visible:outline-2")
        .Add("peer-focus-visible:outline-primary-5")
        .Add("peer-focus-visible:outline-offset-2")
        .Add("peer-checked:[&>span]:left-[1.375rem]")
        .Add("peer-checked:[&>span]:bg-background")
        .Add("data-[invalid=true]:border-danger-400")
        .Add("peer-disabled:bg-primary-5")
        .Add("peer-disabled:border-primary-10")
        .ToString();

    public static string ThumbClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("pointer-events-none")
        .Add("absolute")
        .Add("left-1")
        .Add("top-1/2")
        .Add("h-4")
        .Add("w-4")
        .Add("rounded-full")
        .Add("-translate-y-1/2")
        .Add("bg-primary-foreground")
        .Add("shadow-xs")
        .Add("transition-[left,background-color]")
        .Add("duration-300")
        .Add("ease-[cubic-bezier(0.22,1,0.36,1)]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("peer-disabled:bg-primary-30")
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("text-regular")
        .Add("text-foreground")
        .ToString();
}
