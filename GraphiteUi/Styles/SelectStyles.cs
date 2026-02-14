using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class SelectStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[384])
        .Add("group")
        .Add("relative")
        .Add("inline-flex")
        .Add("w-full")
        .Add("flex-col")
        .Add("data-[disabled=true]:opacity-disabled")
        .Add("data-[disabled=true]:pointer-events-none")
        .ToString();
    public static string TriggerInputClass { get; } = new CssClassBuilder(stackalloc char[384])
        .Add("w-full")
        .Add("bg-transparent")
        .Add("outline-none")
        .Add("appearance-none")
        .Add("text-regular")
        .Add("text-surface1-foreground")
        .Add(ColorVariants.Placeholder.Foreground)
        .ToString();

    public static string SearchIconClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("size-5")
        .Add("shrink-0")
        .Add("text-primary-30")
        .ToString();

    public static string ListClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("flex")
        .Add("flex-col")
        .Add("gap-1")
        .Add("max-h-60")
        .Add("overflow-auto")
        .ToString();

    public static string EmptyClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("px-4")
        .Add("py-3")
        .Add("text-small")
        .Add("text-primary-30")
        .ToString();

    public static string DividerClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("my-1")
        .Add("h-px")
        .Add("w-full")
        .Add("bg-divider")
        .ToString();

    public static string ClearButtonClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("inline-flex")
        .Add("h-10")
        .Add("w-full")
        .Add("items-center")
        .Add("gap-2")
        .Add("rounded-md")
        .Add("px-2")
        .Add("text-left")
        .Add("text-regular")
        .Add("outline-hidden")
        .Add("cursor-pointer")
        .Add("transition-[color,background-color]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add(ColorVariants.Surface.HoverBackground)
        .Add("disabled:text-primary-30")
        .Add("disabled:hover:bg-transparent")
        .Add(Utils.DisabledCursorDefault)
        .ToString();

    public static string ClearIconClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("size-5")
        .ToString();

    public static string ItemClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("inline-flex")
        .Add("w-full")
        .Add("h-10")
        .Add("items-center")
        .Add("rounded-md")
        .Add("px-2.5")
        .Add("text-left")
        .Add("text-regular")
        .Add("outline-hidden")
        .Add("cursor-pointer")
        .Add("transition-[background-color,color]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add(ColorVariants.Surface.HoverBackground)
        .Add("focus-visible:ring")
        .Add("focus-visible:ring-primary-5")
        .Add("data-[selected=true]:bg-white")
        .Add("data-[selected=true]:text-secondary-foreground")
        .Add("data-[active=true]:bg-white")
        .Add("data-[active=true]:text-secondary-foreground")
        .Add("data-[disabled=true]:text-primary-30")
        .Add("data-[disabled=true]:hover:bg-transparent")
        .Add("data-[disabled=true]:pointer-events-none")
        .Add("data-[disabled=true]:cursor-default")
        .ToString();
}
