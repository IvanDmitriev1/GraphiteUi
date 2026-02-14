using GraphiteUi.Common;
using GraphiteUi.Components.Bases;
using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

public partial class UiAvatar : UiComponentBase
{
    [Parameter] public string? Src { get; set; }
    [Parameter] public string Alt { get; set; } = "Avatar";
    [Parameter] public string? Name { get; set; }
    [Parameter] public Size Size { get; set; } = Size.Medium;

    private string RootClass => MergeRootClass(AvatarStyles.GetRootClasses(this));

    private string Initials
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return "?";
            }

            string[] parts = Name.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length == 0)
            {
                return "?";
            }

            if (parts.Length == 1)
            {
                return parts[0][0].ToString().ToUpperInvariant();
            }

            return string.Concat(parts[0][0], parts[^1][0]).ToUpperInvariant();
        }
    }

    public UiAvatar()
    {
        As = "span";
    }
}
