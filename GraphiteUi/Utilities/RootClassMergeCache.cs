using System.Collections.Concurrent;
using TailwindMerge;

namespace GraphiteUi.Utilities;

internal static class RootClassMergeCache
{
    private static readonly ConcurrentDictionary<MergeCacheKey, MergeCacheEntry> Cache = new();

    public static string GetOrAdd(TwMerge twMerge, string coreClasses, string? customClasses)
    {
        string custom = customClasses ?? string.Empty;

        int coreHash = StringComparer.Ordinal.GetHashCode(coreClasses);
        int customHash = StringComparer.Ordinal.GetHashCode(custom);

        var key = new MergeCacheKey(
            HashCode.Combine(coreHash, customHash),
            coreClasses.Length,
            custom.Length);

        if (Cache.TryGetValue(key, out MergeCacheEntry? cachedEntry))
        {
            return cachedEntry.MergedClasses;
        }

        string mergedClasses = custom.Length == 0
            ? twMerge.Merge(coreClasses)
            : twMerge.Merge(coreClasses, custom);

        Cache[key] = new MergeCacheEntry(coreClasses, custom, mergedClasses);
        return mergedClasses;
    }

    private readonly record struct MergeCacheKey(
        int CombinedHash,
        int CoreLength,
        int CustomLength);

    private sealed record MergeCacheEntry(
        string CoreClasses,
        string CustomClasses,
        string MergedClasses);
}
