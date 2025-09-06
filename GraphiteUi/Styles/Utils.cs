using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class Utils
{
    public static readonly string VisuallyHidden = new CssClassBuilder(stackalloc char[25])
        .Add("sr-only")
        .ToString();

    public static readonly string ReduceMotion = new CssClassBuilder(stackalloc char[128])
        .Add("reduce-motion:transition-none")
        .ToString();

    public static readonly string Disabled = new CssClassBuilder(stackalloc char[128])
        .Add("opacity-disabled")
        .Add("pointer-events-none")
        .ToString();

    public static readonly string FocusVisible = new CssClassBuilder(stackalloc char[128])
        .Add("outline-hidden")
        .Add("focus-visible:z-10")
        .Add("focus-visible:outline-2")
        .Add("focus-visible:outline-focus")
        .Add("focus-visible:outline-offset-2")
        .ToString();

    public static readonly string FocusWithin = new CssClassBuilder(stackalloc char[128])
        .Add("outline-hidden")
        .Add("focus-within:z-10")
        .Add("focus-within:outline-2")
        .Add("focus-within:outline-focus")
        .Add("focus-within:outline-offset-2")
        .ToString();
}