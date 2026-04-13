using System.Collections.Generic;

public interface IRollResolver<T> where T : IRollData
{
    T ResolveRoll(List<T> possibleRolls);
}