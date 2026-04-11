using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace GachaGame.Azure.Core.Helpers;
public readonly struct Optional<T>
{
    readonly T? m_value;

    Optional(T value) => m_value = value;

    [MemberNotNullWhen(true, nameof(m_value))]
    public bool HasValue => m_value is not null;
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> Some(T value) => new(value);
    public static Optional<T> None => default;
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> OfNullable(T? value) => value is null ? None : new(value);
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Optional<TResult> SelectMany<TResult>(Func<T, Optional<TResult>> bind) =>
        HasValue ? bind(m_value) : Optional<TResult>.None;
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        HasValue ? onSome(m_value) : onNone();
}