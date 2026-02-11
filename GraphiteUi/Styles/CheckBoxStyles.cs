using GraphiteUi.Components;
using GraphiteUi.Components.Checkbox;
using GraphiteUi.Common;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public sealed class CheckBoxStyles : ICheckBoxStyles
{
    private static CssClassBuilder Base() => CssClassBuilder.Empty()
        .Add("p-1")
        .Add("-m-1")
        .Add("group")
        .Add("max-w-fit")
        .Add("inline-flex")
        .Add("items-center")
        .Add("justify-start")
        .Add("outline-hidden")
        .Add("cursor-pointer")
        .Add("data-[disabled=true]:cursor-default")
        .Add("data-[readonly=true]:cursor-default");

    public static string GetClasses(UiCheckbox component)
    {
        CssClassBuilder builder = new CssClassBuilder(stackalloc char[256]);
        builder.Add(Base());
        builder.Add(component.Class);
        return builder.ToString();
    }

    public static string WrapperClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("relative")
        .Add("inline-flex")
        .Add("items-center")
        .Add("justify-start")
        .Add("gap-1.5")
        .Add("flex-shrink-0")
        .Add("transition-transform")
        .Add("motion-reduce:transition-none")
        .Add("group-active:scale-95")
        .Add("group-data-[disabled=true]:cursor-default")
        .Add("group-data-[readonly=true]:cursor-default")
        .ToString();

    public static string InputClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add(StyleUtils.VisuallyHidden)
        .Add("peer")
        .ToString();

    public static string ControlClass { get; } = new CssClassBuilder(stackalloc char[1024])
        .Add("relative")
        .Add("inline-flex")
        .Add("items-center")
        .Add("justify-center")
        .Add("size-8")
        .Add("rounded-md")
        .Add("border")
        .Add("border-primary-15")
        .Add("bg-primary-10")
        .Add("text-transparent")
        .Add("select-none")
        .Add("transition-[background-color,border-color,color,box-shadow]")
        .Add("motion-reduce:transition-none")
        // static -> hovered
        .Add("group-hover:bg-primary-15")
        .Add("group-hover:border-primary-20")
        // checked state (driven by native input)
        .Add("peer-checked:bg-primary-foreground")
        .Add("peer-checked:border-primary-foreground")
        .Add("peer-checked:text-secondary-foreground")
        // checked state (like screenshot: white box, dark check)
        .Add("group-data-[checked=true]:bg-primary-foreground")
        .Add("group-data-[checked=true]:border-primary-foreground")
        .Add("group-data-[checked=true]:text-secondary-foreground")
        // focus state
        .Add("group-focus-within:ring")
        .Add("peer-focus-visible:ring")
        .Add(ColorVariants.Ring.Focus[ThemeColor.Primary])
        // disabled state
        .Add("peer-disabled:bg-primary-5")
        .Add("peer-disabled:border-primary-10")
        .Add("peer-disabled:text-primary-20")
        .Add("group-data-[disabled=true]:bg-primary-5")
        .Add("group-data-[disabled=true]:border-primary-10")
        .Add("group-data-[disabled=true]:text-primary-20")
        .ToString();

    public static string IconClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("size-4")
        .ToString();

    public static string LabelClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("text-regular")
        .Add("text-foreground")
        .Add("leading-none")
        .Add("peer-disabled:text-primary-30")
        .Add("group-data-[disabled=true]:text-primary-30")
        .ToString();
}

