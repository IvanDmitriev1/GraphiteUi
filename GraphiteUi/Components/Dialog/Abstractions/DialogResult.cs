using System.Diagnostics.CodeAnalysis;

namespace GraphiteUi.Components;

public readonly struct DialogResult<TResult>(TResult? value, bool isCancelled)
{
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsCancelled { get; } = isCancelled;

    public TResult? Value { get; } = value;

    public static DialogResult<TResult> FromValue(TResult value) => new(value, false);
    public static DialogResult<TResult> Canceled => new(default, true);
}