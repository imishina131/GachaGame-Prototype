namespace GachaGame.Azure.Core.Interfaces;

public class EmptyRollResolver<T> : IRollResolver<T> where T : struct, IRollData
{
    public T ResolveRoll(List<T> possibleRolls)
    {
        return default;
    }
}