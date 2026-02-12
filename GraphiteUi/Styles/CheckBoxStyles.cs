using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class CheckBoxStyles
{
    private static readonly string BaseClass = new CssClassBuilder(stackalloc char[256])
        .Add("p-1 -m-1")
        .Add("group")
        .Add("max-w-fit")
        .Add(Utils.InlineFlexStart)
        .Add("outline-hidden")
        .Add("cursor-pointer")
        .Add(Utils.DataDisabledCursorDefault)
        .Add(Utils.DataReadonlyCursorDefault)
        .ToString();

    public static string GetRootClasses(UiCheckbox component) => BaseClass;

    public static string WrapperClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("relative")
        .Add(Utils.InlineFlexStart)
        .Add("gap-1.5")
        .Add("flex-shrink-0")
        .Add("transition-transform")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("group-active:scale-95")
        .Add(Utils.GroupDataDisabledCursorDefault)
        .Add(Utils.GroupDataReadonlyCursorDefault)
        .ToString();

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add(Utils.VisuallyHidden)
        .Add("peer")
        .ToString();

    public static string ControlClass { get; } = new CssClassBuilder(stackalloc char[1024])
        .Add("relative")
        .Add(Utils.InlineFlexCentered)
        .Add("size-8")
        .Add("rounded-md")
        .Add("border")
        .Add("border-primary-15")
        .Add("bg-primary-10")
        .Add("text-transparent")
        .Add("select-none")
        .Add("transition-[background-color,border-color,color,box-shadow]")
        .Add(Utils.MotionReduceTransitionNone)
        // static -> hovered
        .Add("group-hover:bg-primary-15")
        .Add("group-hover:border-primary-20")
        // checked state (driven by native input)
        .Add("peer-checked:bg-primary-foreground")
        .Add("peer-checked:border-primary-foreground")
        .Add("peer-checked:text-secondary-foreground")
        // validation state
        .Add("data-[invalid=true]:border-danger-400")
        // focus state
        .Add("group-focus-within:ring")
        .Add("peer-focus-visible:ring")
        .Add("group-focus-within:ring-primary-5")
        .Add("peer-focus-visible:ring-primary-5")
        // disabled state
        .Add("peer-disabled:bg-primary-5")
        .Add("peer-disabled:border-primary-10")
        .Add("peer-disabled:text-primary-20")
        .ToString();

    public static string IconClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("size-4")
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("text-regular")
        .Add("text-foreground")
        .Add("leading-none")
        .Add("peer-disabled:text-primary-30")
        .ToString();
}

