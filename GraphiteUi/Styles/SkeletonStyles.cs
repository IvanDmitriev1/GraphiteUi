using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class SkeletonStyles
{
    private const string ShimmerClass = "bg-[image:var(--skeleton-shimmer-image)] bg-[length:var(--skeleton-shimmer-size)] animate-[var(--skeleton-shimmer-animation)] motion-reduce:animate-none";

    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[512])
        .Add("relative transition-opacity duration-150 ease-out")
        .Add(Utils.MotionReduceTransitionNone)
        .Add("data-[loading=true]:pointer-events-none data-[loading=true]:select-none data-[loading=true]:cursor-wait data-[loading=true]:overflow-hidden")
        .ToString();

    public static string ContentClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add("relative transition-[opacity,color,background-color,border-color] duration-150 ease-out")
        .Add(Utils.MotionReduceTransitionNone)
        .ToString();

    public static string AutoMimicClass { get; } = new CssClassBuilder(stackalloc char[256])
        .Add(ContentClass)
        .Add("graphite-skeleton-mimic")
        .ToString();

    public static string BlockClass { get; } = new CssClassBuilder(stackalloc char[1024])
        .Add("relative w-full min-h-4 overflow-hidden rounded-md bg-skeleton text-transparent pointer-events-none")
        .Add(ShimmerClass)
        .ToString();

    public static string LineClass { get; } = new CssClassBuilder(stackalloc char[1024])
        .Add("relative h-4 w-full overflow-hidden rounded-md bg-skeleton text-transparent pointer-events-none")
        .Add(ShimmerClass)
        .ToString();
}
