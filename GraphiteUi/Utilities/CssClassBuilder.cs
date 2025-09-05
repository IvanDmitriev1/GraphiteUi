using CommunityToolkit.HighPerformance.Buffers;
using LinkDotNet.StringBuilder;

namespace GraphiteUi.Utilities;

public ref struct CssClassBuilder
{
    private ValueStringBuilder _stringBuilder;

    public CssClassBuilder(Span<char> initialBuffer)
    {
        _stringBuilder = new ValueStringBuilder(initialBuffer);
    }

    public CssClassBuilder()
    {
        _stringBuilder = new ValueStringBuilder();
    }

    public static CssClassBuilder Empty() => new();

    public override string ToString()
    {
        var value = StringPool.Shared.GetOrAdd(_stringBuilder.AsSpan());
        _stringBuilder.Dispose();
        return value;
    }

    public CssClassBuilder Add(scoped CssClassBuilder builder)
    {
        if (builder._stringBuilder.Length <= 0)
            return this;

        AppendWithLeadingSpace(builder._stringBuilder.AsSpan());
        builder._stringBuilder.Dispose();
        return this;
    }

    public CssClassBuilder Add(scoped ReadOnlySpan<char> value)
    {
        AppendWithLeadingSpace(value);
        return this;
    }

    public CssClassBuilder Add(scoped ReadOnlySpan<char> value, bool when)
    {
        if (when)
            AppendWithLeadingSpace(value);

        return this;
    }

    public readonly bool Contains(ReadOnlySpan<char> value) => _stringBuilder.Contains(value);


    private void AppendWithLeadingSpace(scoped ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
            return;

        if (value[^1] != ' ')
            _stringBuilder.Append(' ');

        _stringBuilder.Append(value);
    }
}