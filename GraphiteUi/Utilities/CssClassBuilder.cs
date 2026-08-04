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
        var value = _stringBuilder.ToString();
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
        value = value.Trim();

        if (value.IsEmpty)
            return;

        if (_stringBuilder.Length > 0)
            _stringBuilder.Append(' ');

        _stringBuilder.Append(value);
    }
}