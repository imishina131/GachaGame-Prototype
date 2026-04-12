namespace GachaGame.Azure.Core.Interfaces;


public interface IRollResolver<T> where T : struct, IRollData
{
    /// <summary>
    /// Resolves a roll for a <typeparamref name="T"/> through taking in a multiple <see cref="IRollData"/> and generating a single result
    /// </summary>
    /// <param name="possibleRolls">Each possible roll result</param>
    /// <returns>The resulting roll</returns>
    public T ResolveRoll(List<T> possibleRolls);
}