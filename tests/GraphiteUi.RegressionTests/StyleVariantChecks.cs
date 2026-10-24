using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.RegressionTests;

internal static class StyleVariantChecks
{
    private static readonly string[] ButtonToneTokens =
    [
        "border-transparent",
        "bg-accent",
        "bg-secondary",
        "bg-success",
        "bg-warning",
        "bg-danger",
        "bg-info"
    ];

    private static readonly string[] SubtleToneTokens =
    [
        "bg-secondary-subtle",
        "bg-accent-subtle",
        "bg-secondary-subtle",
        "bg-success-subtle",
        "bg-warning-subtle",
        "bg-danger-subtle",
        "bg-info-subtle"
    ];

    private static readonly string[] IndicatorTokens =
    [
        "bg-secondary-indicator",
        "bg-accent",
        "bg-secondary-indicator",
        "bg-success",
        "bg-warning",
        "bg-danger",
        "bg-info"
    ];

    private static readonly string[] ButtonSizeTokens =
    [
        "h-[var(--control-height-sm)]",
        "h-[var(--control-height-md)]",
        "h-[var(--control-height-lg)]"
    ];
    private static readonly string[] ButtonIconSizeTokens =
    [
        "size-[var(--control-height-sm)]",
        "size-[var(--control-height-md)]",
        "size-[var(--control-height-lg)]"
    ];
    private static readonly string[] ButtonSubtleToneTokens =
    [
        "secondary",
        "accent",
        "secondary",
        "success",
        "warning",
        "danger",
        "info"
    ];
    private static readonly string[] ButtonOutlineBorderTokens =
    [
        "border-control",
        "border-control",
        "border-control",
        "border-success-subtle-foreground",
        "border-warning-subtle-foreground",
        "border-danger-subtle-foreground",
        "border-info-subtle-foreground"
    ];
    private static readonly string[] ButtonNonSolidForegroundTokens =
    [
        "",
        "text-foreground",
        "text-foreground",
        "text-success-subtle-foreground",
        "text-warning-subtle-foreground",
        "text-danger-subtle-foreground",
        "text-info-subtle-foreground"
    ];
    private static readonly string[] BadgeSizeTokens = ["px-2", "px-2.5", "px-3"];
    private static readonly string[] AvatarSizeTokens = ["size-8", "size-10", "size-14"];

    internal static void Run()
    {
        CheckButtons();
        CheckBadges();
        CheckAvatars();
        CheckAlerts();
        CheckDrawers();
        CheckTooltips();
        CheckToasts();
        CheckCheckboxes();
    }

    private static void CheckButtons()
    {
        var component = new UiButton();
        var variants = Enum.GetValues<ButtonVariant>();

        foreach (ThemeColor color in Enum.GetValues<ThemeColor>())
        {
            for (int sizeIndex = 0; sizeIndex < ButtonSizeTokens.Length; sizeIndex++)
            {
                var size = (Size)sizeIndex;
                foreach (ButtonVariant variant in variants)
                {
                    foreach (bool iconOnly in new[] { false, true })
                    {
                        SetParameters(component, new Dictionary<string, object?>
                        {
                            [nameof(UiButton.Color)] = color,
                            [nameof(UiButton.Size)] = size,
                            [nameof(UiButton.Variant)] = variant,
                            [nameof(UiButton.IconOnly)] = iconOnly
                        });

                        string classes = ButtonStyles.GetRootClasses(color, size, variant, iconOnly);
                        AssertEqual(classes, ButtonStyles.GetRootClasses(component), "Button component overload diverged.");
                        AssertToken(classes, "border", $"Button {variant} lost its allocated border geometry.");
                        AssertToken(classes, "rounded-md", $"Button {variant} lost its radius.");
                        AssertToken(classes, "focus-visible:ring-focus", $"Button {variant} lost its focus ring.");
                        AssertToken(classes, "motion-reduce:transition-none", $"Button {variant} lost reduced motion support.");
                        Assert(!classes.Contains("data-disabled:bg-", StringComparison.Ordinal),
                            "Button disabled styling uses a presence-only data selector.");

                        if (iconOnly)
                        {
                            AssertToken(classes, ButtonIconSizeTokens[sizeIndex], $"Icon-only button size missing for {size}.");
                            AssertToken(classes, "p-0", "Icon-only button has horizontal padding.");
                            AssertNoPrefix(classes, "min-w-", "Icon-only button inherited a minimum width.");
                            AssertNoPrefix(classes, "px-", "Icon-only button inherited horizontal padding.");
                            AssertNoPrefix(classes, "h-[var(--control-height-", "Icon-only button has a second height utility.");
                        }
                        else
                        {
                            AssertToken(classes, ButtonSizeTokens[sizeIndex], $"Button size missing for {size}.");
                            if (variant == ButtonVariant.Link)
                            {
                                AssertNoPrefix(classes, "min-w-", "Link button has a labeled minimum width.");
                                AssertNoPrefix(classes, "px-", "Link button has horizontal padding.");
                            }
                            else
                            {
                                AssertAnyPrefix(classes, "min-w-", $"Labeled {variant} button has no minimum width.");
                                AssertAnyPrefix(classes, "px-", $"Labeled {variant} button has no horizontal padding.");
                            }
                        }

                        if (variant == ButtonVariant.Solid)
                        {
                            AssertToken(classes, ButtonToneTokens[(int)color], $"Solid tone missing for {color}: {classes}.");
                            AssertToken(classes, "shadow-xs", "Solid button lost its existing shadow.");
                            Assert(!HasToken(classes, "underline"), "Solid button became underlined.");
                        }
                        else
                        {
                            AssertToken(classes, "bg-transparent", $"{variant} button gained a resting fill.");
                            string foreground = ButtonNonSolidForegroundTokens[(int)color];
                            if (foreground.Length > 0)
                            {
                                AssertToken(classes, foreground, $"{variant} button foreground is not readable for {color}.");
                            }
                            else
                            {
                                Assert(!HasToken(classes, "text-foreground"), "Inherit button stopped inheriting its text color.");
                            }
                            Assert(!HasToken(classes, "shadow-xs"), $"{variant} button retained a solid shadow.");

                            if (variant is ButtonVariant.Outline or ButtonVariant.Ghost)
                            {
                                string tone = ButtonSubtleToneTokens[(int)color];
                                AssertToken(classes, $"hover:bg-{tone}-subtle-hover", $"{variant} {color} hover state missing.");
                                AssertToken(classes, $"active:bg-{tone}-subtle-active", $"{variant} {color} pressed state missing.");
                            }

                            if (variant == ButtonVariant.Outline)
                            {
                                AssertToken(classes, ButtonOutlineBorderTokens[(int)color], $"Outline border missing for {color}.");
                                Assert(!HasToken(classes, "underline"), "Outline button became underlined.");
                            }
                            else if (variant == ButtonVariant.Ghost)
                            {
                                AssertToken(classes, "border-transparent", "Ghost button is not visually borderless.");
                                Assert(!HasToken(classes, "underline"), "Ghost button became underlined.");
                            }
                            else
                            {
                                AssertToken(classes, "border-transparent", "Link button is not visually borderless.");
                                AssertToken(classes, "underline", "Link button lost its underline.");
                                Assert(!classes.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                    .Any(token => token.StartsWith("hover:bg-", StringComparison.Ordinal)
                                        || token.StartsWith("active:bg-", StringComparison.Ordinal)),
                                    "Link button gained an unrequested interaction fill.");
                            }
                        }
                    }
                }

                string oldOverload = ButtonStyles.GetRootClasses(color, size);
                AssertEqual(oldOverload, ButtonStyles.GetRootClasses(color, size, ButtonVariant.Solid, false),
                    "The two-argument button style overload stopped forwarding to labeled Solid.");
            }
        }

        string inheritClasses = ButtonStyles.GetRootClasses(ThemeColor.Inherit, Size.Small);
        Assert(!HasToken(inheritClasses, "bg-accent") && !HasToken(inheritClasses, "bg-secondary"),
            "Inherit button gained a filled tone.");
        AssertEqual(ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium),
            ButtonStyles.GetRootClasses((ThemeColor)(-1), Size.Medium), "Negative button color did not fall back to primary.");
        AssertEqual(ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium),
            ButtonStyles.GetRootClasses((ThemeColor)99, Size.Medium), "Out-of-range button color did not fall back to primary.");
        AssertEqual(ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium),
            ButtonStyles.GetRootClasses(ThemeColor.Primary, (Size)(-1)), "Negative button size did not fall back to medium.");
        AssertEqual(ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium),
            ButtonStyles.GetRootClasses(ThemeColor.Primary, (Size)99), "Out-of-range button size did not fall back to medium.");
        AssertEqual(ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium),
            ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium, (ButtonVariant)(-1), false),
            "Negative button variant did not fall back to Solid.");
        AssertEqual(ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium),
            ButtonStyles.GetRootClasses(ThemeColor.Primary, Size.Medium, (ButtonVariant)99, false),
            "Out-of-range button variant did not fall back to Solid.");
    }

    private static void AssertAnyPrefix(string classes, string prefix, string message) =>
        Assert(classes.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Any(token => token.StartsWith(prefix, StringComparison.Ordinal)), message);

    private static void AssertNoPrefix(string classes, string prefix, string message) =>
        Assert(!classes.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Any(token => token.StartsWith(prefix, StringComparison.Ordinal)), message);

    private static void CheckBadges()
    {
        var component = new UiBadge();

        foreach (ThemeColor color in Enum.GetValues<ThemeColor>())
        {
            for (int sizeIndex = 0; sizeIndex < BadgeSizeTokens.Length; sizeIndex++)
            {
                var size = (Size)sizeIndex;
                SetParameters(component, new Dictionary<string, object?>
                {
                    [nameof(UiBadge.Color)] = color,
                    [nameof(UiBadge.Size)] = size
                });

                string classes = BadgeStyles.GetRootClasses(color, size);
                int toneIndex = color == ThemeColor.Inherit ? (int)ThemeColor.Primary : (int)color;
                AssertToken(classes, SubtleToneTokens[toneIndex], $"Badge tone missing for {color}.");
                AssertToken(classes, BadgeSizeTokens[sizeIndex], $"Badge size missing for {size}.");
                AssertEqual(classes, BadgeStyles.GetRootClasses(component), "Badge component overload diverged.");
            }
        }

        AssertEqual(BadgeStyles.GetRootClasses(ThemeColor.Primary, Size.Small),
            BadgeStyles.GetRootClasses(ThemeColor.Inherit, Size.Small), "Inherit badge did not use primary tone.");
        AssertEqual(BadgeStyles.GetRootClasses(ThemeColor.Primary, Size.Small),
            BadgeStyles.GetRootClasses((ThemeColor)(-1), Size.Small), "Negative badge color did not fall back to primary.");
        AssertEqual(BadgeStyles.GetRootClasses(ThemeColor.Primary, Size.Small),
            BadgeStyles.GetRootClasses((ThemeColor)99, Size.Small), "Out-of-range badge color did not fall back to primary.");
        AssertEqual(BadgeStyles.GetRootClasses(ThemeColor.Primary, Size.Small),
            BadgeStyles.GetRootClasses(ThemeColor.Primary, (Size)(-1)), "Negative badge size did not fall back to small.");
        AssertEqual(BadgeStyles.GetRootClasses(ThemeColor.Primary, Size.Small),
            BadgeStyles.GetRootClasses(ThemeColor.Primary, (Size)99), "Out-of-range badge size did not fall back to small.");
    }

    private static void CheckAvatars()
    {
        var component = new UiAvatar();

        for (int sizeIndex = 0; sizeIndex < AvatarSizeTokens.Length; sizeIndex++)
        {
            var size = (Size)sizeIndex;
            SetParameters(component, new Dictionary<string, object?>
            {
                [nameof(UiAvatar.Size)] = size
            });

            string classes = AvatarStyles.GetRootClasses(size);
            AssertToken(classes, AvatarSizeTokens[sizeIndex], $"Avatar size missing for {size}.");
            AssertEqual(classes, AvatarStyles.GetRootClasses(component), "Avatar component overload diverged.");
        }

        AssertEqual(AvatarStyles.GetRootClasses(Size.Medium), AvatarStyles.GetRootClasses((Size)(-1)),
            "Negative avatar size did not fall back to medium.");
        AssertEqual(AvatarStyles.GetRootClasses(Size.Medium), AvatarStyles.GetRootClasses((Size)99),
            "Out-of-range avatar size did not fall back to medium.");
    }

    private static void CheckAlerts()
    {
        var component = new UiAlert();

        foreach (ThemeColor color in Enum.GetValues<ThemeColor>())
        {
            SetParameters(component, new Dictionary<string, object?>
            {
                [nameof(UiAlert.Color)] = color
            });
            string rootClasses = AlertStyles.GetRootClasses(color);
            string indicatorClass = AlertStyles.GetIndicatorClass(color);

            int toneIndex = color == ThemeColor.Inherit ? (int)ThemeColor.Primary : (int)color;
            AssertToken(rootClasses, SubtleToneTokens[toneIndex], $"Alert tone missing for {color}.");
            AssertToken(indicatorClass, IndicatorTokens[toneIndex], $"Alert indicator missing for {color}.");
            AssertEqual(rootClasses, AlertStyles.GetRootClasses(component), "Alert component root overload diverged.");
            AssertEqual(indicatorClass, AlertStyles.GetIndicatorClass(component), "Alert component indicator overload diverged.");
        }

        AssertEqual(AlertStyles.GetRootClasses(ThemeColor.Primary), AlertStyles.GetRootClasses(ThemeColor.Inherit),
            "Inherit alert did not use primary tone.");
        AssertEqual(AlertStyles.GetIndicatorClass(ThemeColor.Primary), AlertStyles.GetIndicatorClass(ThemeColor.Inherit),
            "Inherit alert indicator did not use primary tone.");
        AssertEqual(AlertStyles.GetRootClasses(ThemeColor.Primary), AlertStyles.GetRootClasses((ThemeColor)(-1)),
            "Negative alert color did not fall back to primary.");
        AssertEqual(AlertStyles.GetRootClasses(ThemeColor.Primary), AlertStyles.GetRootClasses((ThemeColor)99),
            "Out-of-range alert color did not fall back to primary.");
        AssertEqual(AlertStyles.GetIndicatorClass(ThemeColor.Primary), AlertStyles.GetIndicatorClass((ThemeColor)(-1)),
            "Negative alert indicator color did not fall back to primary.");
        AssertEqual(AlertStyles.GetIndicatorClass(ThemeColor.Primary), AlertStyles.GetIndicatorClass((ThemeColor)99),
            "Out-of-range alert indicator color did not fall back to primary.");
    }

    private static void CheckDrawers()
    {
        var component = new UiDrawer();

        foreach (Align side in Enum.GetValues<Align>())
        {
            SetParameters(component, new Dictionary<string, object?>
            {
                [nameof(UiDrawer.Side)] = side
            });
            string classes = DrawerStyles.GetPanelClass(side);
            AssertToken(classes, side == Align.End ? "right-0" : "left-0", $"Drawer placement missing for {side}.");
            AssertToken(classes, side == Align.End ? "translate-x-full" : "-translate-x-full",
                $"Drawer transition missing for {side}.");
            Assert(!HasToken(classes, side == Align.End ? "left-0" : "right-0"),
                $"Drawer placement includes the opposite edge for {side}.");
            Assert(!HasToken(classes, side == Align.End ? "-translate-x-full" : "translate-x-full"),
                $"Drawer transition includes the opposite direction for {side}.");
            AssertEqual(classes, DrawerStyles.GetPanelClass(component), "Drawer component overload diverged.");
        }

        AssertEqual(DrawerStyles.GetPanelClass(Align.Start), DrawerStyles.GetPanelClass((Align)(-1)),
            "Negative drawer side did not fall back to start.");
        AssertEqual(DrawerStyles.GetPanelClass(Align.Start), DrawerStyles.GetPanelClass((Align)99),
            "Out-of-range drawer side did not fall back to start.");
    }

    private static void CheckTooltips()
    {
        var component = new UiTooltip();

        foreach (Align align in Enum.GetValues<Align>())
        {
            foreach (bool bottom in new[] { false, true })
            {
                SetParameters(component, new Dictionary<string, object?>
                {
                    [nameof(UiTooltip.Align)] = align,
                    [nameof(UiTooltip.Bottom)] = bottom
                });
                string classes = TooltipStyles.GetBubbleClass(align, bottom);

                AssertToken(classes, bottom ? "top-full" : "bottom-full", "Tooltip vertical placement was incorrect.");
                Assert(!HasToken(classes, bottom ? "bottom-full" : "top-full"), "Tooltip includes the opposite vertical placement.");
                AssertToken(classes, align switch
                {
                    Align.Start => "left-0",
                    Align.End => "right-0",
                    _ => "left-1/2"
                }, $"Tooltip horizontal placement was incorrect for {align}.");
                Assert(!HasToken(classes, align switch
                {
                    Align.Start => "right-0",
                    Align.End => "left-0",
                    _ => "right-0"
                }), $"Tooltip includes a conflicting horizontal edge for {align}.");
                if (align != Align.Center)
                {
                    Assert(!HasToken(classes, "left-1/2"), $"Non-centered tooltip includes centered placement for {align}.");
                }
                if (align == Align.Center)
                {
                    AssertToken(classes, "-translate-x-1/2", "Centered tooltip did not offset by half its width.");
                }

                AssertEqual(classes, TooltipStyles.GetBubbleClass(component), "Tooltip component overload diverged.");
            }
        }

        AssertEqual(TooltipStyles.GetBubbleClass(Align.Center, false), TooltipStyles.GetBubbleClass((Align)(-1), false),
            "Negative tooltip alignment did not fall back to center.");
        AssertEqual(TooltipStyles.GetBubbleClass(Align.Center, true), TooltipStyles.GetBubbleClass((Align)99, true),
            "Out-of-range tooltip alignment did not fall back to center.");
    }

    private static void CheckToasts()
    {
        var component = new UiToastHost();
        string[] topRightTokens = ["top-0", "left-0", "items-start", "justify-center", "sm:left-auto", "sm:right-0", "sm:justify-end"];
        string[] topCenterTokens = ["top-0", "left-0", "items-start", "justify-center"];
        string[] bottomRightTokens = ["bottom-0", "left-0", "items-end", "justify-center", "sm:left-auto", "sm:right-0", "sm:justify-end"];

        foreach (ToastPlacement placement in Enum.GetValues<ToastPlacement>())
        {
            SetParameters(component, new Dictionary<string, object?>
            {
                [nameof(UiToastHost.Placement)] = placement
            });
            string classes = ToastStyles.GetHostRootClasses(placement);
            string[] expectedTokens = placement switch
            {
                ToastPlacement.TopCenter => topCenterTokens,
                ToastPlacement.BottomRight => bottomRightTokens,
                _ => topRightTokens
            };

            foreach (string token in expectedTokens)
            {
                AssertToken(classes, token, $"Toast placement {placement} is missing {token}.");
            }

            Assert(!HasToken(classes, placement == ToastPlacement.BottomRight ? "top-0" : "bottom-0"),
                $"Toast placement {placement} includes the opposite vertical edge.");
            if (placement == ToastPlacement.TopCenter)
            {
                Assert(!HasToken(classes, "sm:left-auto") && !HasToken(classes, "sm:right-0")
                    && !HasToken(classes, "sm:justify-end"), "Centered toast host includes right-aligned responsive placement.");
            }

            AssertEqual(classes, ToastStyles.GetHostRootClasses(component), "Toast component overload diverged.");
        }

        AssertEqual(ToastStyles.GetHostRootClasses(ToastPlacement.TopRight), ToastStyles.GetHostRootClasses((ToastPlacement)(-1)),
            "Negative toast placement did not fall back to top right.");
        AssertEqual(ToastStyles.GetHostRootClasses(ToastPlacement.TopRight), ToastStyles.GetHostRootClasses((ToastPlacement)99),
            "Out-of-range toast placement did not fall back to top right.");
    }

    private static void CheckCheckboxes()
    {
        var component = new UiCheckbox();
        string classes = CheckBoxStyles.GetRootClasses();

        AssertToken(classes, "relative", "Checkbox root is missing its positioning class.");
        AssertToken(classes, "group", "Checkbox root is missing its group class.");
        AssertEqual(classes, CheckBoxStyles.GetRootClasses(component), "Checkbox component overload diverged.");
        AssertEqual(classes, CheckBoxStyles.GetRootClasses(null!), "Checkbox component overload stopped accepting null.");
    }

    private static void SetParameters(IComponent component, Dictionary<string, object?> parameters) =>
        ParameterView.FromDictionary(parameters).SetParameterProperties(component);

    private static bool HasToken(string classes, string token) =>
        classes.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(token, StringComparer.Ordinal);

    private static void AssertToken(string classes, string token, string message) =>
        Assert(HasToken(classes, token), message);

    private static void AssertEqual(string expected, string actual, string message) =>
        Assert(string.Equals(expected, actual, StringComparison.Ordinal), message);

    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
