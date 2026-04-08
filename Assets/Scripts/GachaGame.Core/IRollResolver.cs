using System.Collections.Generic;
/// <summary>
/// Resolves a roll for a <typeparamref name="T"/> through taking in a multiple <see cref="RollData{T}"/> and generating a single result
/// </summary>
/// <typeparam name="T">The type that this resolver should handle</typeparam>
public interface IRollResolver<T>
{
    T ResolveRoll(List<RollData<T>> rolls);
}