using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

internal static class ColorVariants
{
    public static readonly string PageColors = new CssClassBuilder(stackalloc char[128])
        .Add("bg-background")
        .Add("text-foreground")
        .ToString();

    public static class Surface
    {
        /// <summary>Resting surface: cards, input fields, triggers.</summary>
        public static readonly string Background = new CssClassBuilder(stackalloc char[128])
            .Add("bg-surface1")
            .Add("text-surface1-foreground")
            .ToString();

        /// <summary>Raised surface: popups, menus, drawers, tooltips.</summary>
        public static readonly string PopupBackground = new CssClassBuilder(stackalloc char[128])
            .Add("bg-surface3")
            .Add("text-surface3-foreground")
            .ToString();

        /// <summary>
        /// Hover tint for rows and triggers. Its own role, not <c>--color-focus</c>:
        /// focus is the accent color, so hovering would otherwise tint every row
        /// brand-colored.
        /// </summary>
        public static readonly string HoverBackground = new CssClassBuilder(stackalloc char[128])
            .Add("hover:bg-surface-hover")
            .Add("text-surface1-foreground")
            .ToString();
    }

    public static class Border
    {
        /// <summary>Default hairline. Was a 5%-alpha border, too faint to read on either theme.</summary>
        public static readonly string Default = new CssClassBuilder(stackalloc char[128])
            .Add("border-border")
            .ToString();

        /// <summary>Emphasised border for modal surfaces.</summary>
        public static readonly string Strong = new CssClassBuilder(stackalloc char[128])
            .Add("border-border-strong")
            .ToString();

        /// <summary>
        /// Boundary for interactive controls (inputs, checkboxes, radios, switches,
        /// triggers). Holds 3:1 per WCAG 1.4.11, unlike <see cref="Default"/>, which is
        /// a decorative hairline and measures 1.19:1 against the page.
        /// </summary>
        public static readonly string Control = new CssClassBuilder(stackalloc char[128])
            .Add("border-control")
            .ToString();

        /// <summary>Hover edge for interactive controls, paired with <see cref="Control"/>.</summary>
        public const string ControlHover = "hover:border-control-hover";

        /// <summary>Same, driven by a hover on the wrapping <c>group</c>.</summary>
        public const string GroupControlHover = "group-hover:border-control-hover";
    }

    public static class Ring
    {
        /// <summary>
        /// One focus indicator for the whole library. The old per-ThemeColor maps
        /// produced a 6px band of 5%-white (~1.1:1), failing WCAG 2.4.11.
        /// </summary>
        public const string Focus = "focus-visible:ring-2 focus-visible:ring-focus";

        public const string FocusWithin = "focus-within:ring-2 focus-within:ring-focus";

        /// <summary>Invalid-state ring, applied via the <c>data-invalid</c> attribute.</summary>
        public const string Invalid = "data-invalid:focus-within:ring-focus-invalid";
    }

    public static class Disabled
    {
        public const string Background = "disabled:bg-disabled-surface data-disabled:bg-disabled-surface";
        public const string Foreground = "disabled:text-disabled-foreground data-disabled:text-disabled-foreground";

        public const string GroupBackground = "group-data-disabled:bg-disabled-surface";
        public const string GroupForeground = "group-data-disabled:text-disabled-foreground";

        /// <summary>Disabled variants driven by a sibling <c>peer</c> input.</summary>
        public const string PeerBackground = "peer-disabled:bg-disabled-surface";
        public const string PeerForeground = "peer-disabled:text-disabled-foreground";
        public const string PeerBorder = "peer-disabled:border-disabled";
    }

    public static class Placeholder
    {
        /// <summary>
        /// Was the 30%-alpha step, which measured 2.66:1 against surface1. The 50%
        /// step clears 4.5:1 on dark but only reaches 3.35:1 on light, so secondary
        /// text sits at 60%.
        /// </summary>
        public const string Foreground = "placeholder:text-muted-foreground";
    }
}
