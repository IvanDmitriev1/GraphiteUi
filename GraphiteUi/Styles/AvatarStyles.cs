using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class AvatarStyles
{
    private static readonly string BaseClass = new CssClassBuilder(stackalloc char[256])
        .Add("inline-flex")
        .Add("items-center")
        .Add("justify-center")
        .Add("overflow-hidden")
        .Add("rounded-full")
        .Add("bg-surface2")
        .Add("text-surface2-foreground")
        .Add("border")
        .Add(ColorVariants.Border.Default)
        .ToString();

    private static readonly string[] SizeClasses =
    [
        "size-8 text-small",
        "size-10 text-regular",
        "size-14 text-medium"
    ];

    private static readonly string[] RootClassesBySize = BuildRootClassesBySize();

    public static string ImageClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("size-full")
        .Add("object-cover")
        .ToString();

    public static string GetRootClasses(UiAvatar component)
    {
        int sizeIndex = (int)component.Size;
        if ((uint)sizeIndex >= (uint)RootClassesBySize.Length)
        {
            sizeIndex = (int)Size.Medium;
        }

        return RootClassesBySize[sizeIndex];
    }

    private static string[] BuildRootClassesBySize()
    {
        int sizeCount = SizeClasses.Length;
        var rootClasses = new string[sizeCount];

        for (int i = 0; i < sizeCount; i++)
        {
            rootClasses[i] = CssClassBuilder.Empty()
                .Add(BaseClass)
                .Add(SizeClasses[i])
                .ToString();
        }

        return rootClasses;
    }
}
