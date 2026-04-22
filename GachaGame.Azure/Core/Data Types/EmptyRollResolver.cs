using GachaGame.Azure.Core.Interfaces;
namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// A <see cref="IRollResolver{T}"/> that always returns the default value of <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type of object to roll</typeparam>
public class EmptyRollResolver<T> : IRollResolver<T> where T : struct, IRollData
{
    /// <inheritdoc/>
    public T ResolveRoll(List<T> possibleRolls, RollContext rollContext) => default;
}