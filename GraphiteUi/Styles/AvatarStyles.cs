using GraphiteUi.Common;
using GraphiteUi.Components;
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class AvatarStyles
{
    private static readonly string BaseClass = new CssClassBuilder(stackalloc char[256])
        .Add("inline-flex items-center justify-center overflow-hidden rounded-full bg-surface2 text-surface2-foreground border")
        .Add(ColorVariants.Border.Default)
        .ToString();

    private static readonly string[] SizeClasses =
    [
        "size-8 text-caption",
        "size-10 text-body-sm",
        "size-14 text-body"
    ];

    private static readonly string[] RootClassesBySize = BuildRootClassesBySize();

    public static string ImageClass { get; } = "size-full object-cover";

    public static string GetRootClasses(UiAvatar component) => GetRootClasses(component.Size);

    public static string GetRootClasses(Size size)
    {
        int sizeIndex = (int)size;
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
