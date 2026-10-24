using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class SelectStyles
{
    public static string RootClass { get; } = "group w-full min-w-0";

    public const string TriggerClass = "w-full min-w-0";
    internal const string FieldClass = "flex w-full min-w-0 flex-col";
    public const string LabelClass = "mb-1 block text-body-sm text-muted-foreground";

    private static readonly string TriggerStateClass =
        "focus-within:ring-2 focus-within:ring-focus focus-within:ring-offset-0 focus-within:border-transparent focus-within:hover:border-transparent aria-[invalid=true]:border-invalid aria-[invalid=true]:focus-within:border-transparent aria-[invalid=true]:focus-within:ring-focus-invalid";

    private static readonly string[] TriggerClasses =
    [
        new CssClassBuilder(stackalloc char[448]).Add(TriggerClass).Add("h-[var(--control-height-sm)]").Add(TriggerStateClass).ToString(),
        new CssClassBuilder(stackalloc char[448]).Add(TriggerClass).Add("h-[var(--control-height-md)]").Add(TriggerStateClass).ToString(),
        new CssClassBuilder(stackalloc char[448]).Add(TriggerClass).Add("h-[var(--control-height-lg)]").Add(TriggerStateClass).ToString()
    ];

    public static string GetTriggerClass(GraphiteUi.Common.Size size)
    {
        int sizeIndex = (int)size;
        return (uint)sizeIndex < (uint)TriggerClasses.Length
            ? TriggerClasses[sizeIndex]
            : TriggerClasses[(int)GraphiteUi.Common.Size.Medium];
    }

    public static string TriggerInputClass { get; } = new CssClassBuilder(stackalloc char[384])
        .Add("w-full min-w-0 flex-1 bg-transparent outline-none appearance-none text-body text-surface1-foreground")
        .Add(ColorVariants.Placeholder.Foreground)
        .ToString();

    public static string SearchIconClass { get; } = "size-5 shrink-0 text-muted-foreground";

    public static string ListClass { get; } = "flex flex-col gap-1 max-h-60 overflow-auto";

    public static string EmptyClass { get; } = "px-4 py-3 text-body-sm text-muted-foreground";

    public static string DividerClass { get; } = "my-1 h-px w-full bg-divider";

    public static string ClearButtonClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("inline-flex h-10 w-full items-center gap-2 rounded-md px-2 text-left text-body cursor-pointer transition-[color,background-color]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add(ColorVariants.Surface.HoverSurfaceClass)
        .Add(Utils.FocusVisible)
        .Add("disabled:text-disabled-foreground disabled:hover:bg-transparent")
        .Add(Utils.DisabledCursorDefault)
        .ToString();

    public static string ClearIconClass { get; } = "size-5";

    public static string ItemClass { get; } = new CssClassBuilder(stackalloc char[768])
        .Add("inline-flex w-full h-10 items-center rounded-md px-2.5 text-left text-body cursor-pointer transition-[background-color,color]")
        .Add(Utils.MotionReduceTransitionNone)
        .Add(ColorVariants.Surface.HoverSurfaceClass)
        .Add(Utils.FocusVisible)
        // Was a solid white row, a jarring jump from a transparent resting state.
        .Add("data-[selected=true]:bg-selected data-[selected=true]:text-selected-foreground data-[active=true]:bg-selected data-[active=true]:text-selected-foreground data-[disabled=true]:text-disabled-foreground data-[disabled=true]:hover:bg-transparent data-[disabled=true]:pointer-events-none data-[disabled=true]:cursor-default")
        .ToString();
}
