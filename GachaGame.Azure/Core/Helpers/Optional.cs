using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace GachaGame.Azure.Core.Helpers;
public readonly struct Optional<T>
{
    readonly T? m_value;
    Optional(T value) => m_value = value;
    [MemberNotNullWhen(true, nameof(m_value))]
    public bool HasValue => m_value is not null;

    /// <summary>
    /// Creates an Optional with a value
    /// </summary>
    /// <param name="value">The value wrapped as an optional</param>
    /// <returns>The new optional wrapped value</returns>
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> Some(T value) => new(value);
    /// <summary>
    /// Creates an Optional with no value
    /// </summary>
    public static Optional<T> None => default;
    /// <summary>
    /// Creates an Optional from a nullable value
    /// </summary>
    /// <param name="value">The nullable type to convert to an optional</param>
    /// <returns>The new Optional</returns>
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> OfNullable(T? value) => value is null ? None : new(value);
    /// <summary>
    /// Flattens multiple Optional values into a single optional
    /// </summary>
    /// <param name="bind">The optional type to flatten into this optional</param>
    /// <typeparam name="TResult">The resulting type from the flatten operation</typeparam>
    /// <returns>The composed optional</returns>
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Optional<TResult> Flatten<TResult>(Func<T, Optional<TResult>> bind) =>
        HasValue ? bind(m_value) : Optional<TResult>.None;
    /// <summary>
    /// Executes a different function based on whether the optional has a value
    /// </summary>
    /// <param name="onSome">The function executed when there is a value</param>
    /// <param name="onNone">The function executed when there is no value</param>
    /// <typeparam name="TResult">Th</typeparam>
    /// <returns></returns>
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        HasValue ? onSome(m_value) : onNone();
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Optional<T> Where(Func<T, bool> predicate) =>
        HasValue && predicate(m_value) ? this : None;
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Optional<TResult> Select<TResult>(Func<T, TResult?> map) =>
        HasValue ? Optional<TResult>.OfNullable(map(m_value)) : Optional<TResult>.None;
    /// <summary>
    /// Maps the value using a selector that may return null, wrapping the result in an Optional
    /// </summary>
    /// <param name="selector">A function that maps the current value to a nullable result</param>
    /// <typeparam name="TResult">The type of the mapped result</typeparam>
    /// <returns>Some if the selector returns a value, None if it returns null</returns>
    [System.Diagnostics.Contracts.Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Optional<TResult> Bind<TResult>(Func<T, TResult?> selector) =>
        HasValue
            ? Optional<TResult>.OfNullable(selector(m_value))
            : Optional<TResult>.None;
}