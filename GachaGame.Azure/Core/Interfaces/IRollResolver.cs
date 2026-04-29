using GachaGame.Azure.Core.DataTypes;
using Microsoft.Extensions.Logging;
namespace GachaGame.Azure.Core.Interfaces;

/// <summary>
/// Represents a resolver for a <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type of object the resolver will handle</typeparam>
public interface IRollResolver<T> where T : IRollData
{
    /// <summary>
    /// Resolves a roll for a <typeparamref name="T"/> through taking in a multiple <see cref="IRollData"/> and generating a single result
    /// </summary>
    /// <param name="possibleRolls">Each possible roll result</param>
    /// <param name="rollContext">The <see cref="RollContext"/> with data for the current roll</param>
    /// <returns>The resulting roll</returns>
    public T ResolveRoll(List<T> possibleRolls, RollContext rollContext, ILogger logger);
}