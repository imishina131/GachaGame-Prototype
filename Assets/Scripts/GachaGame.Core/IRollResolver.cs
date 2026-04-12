using System.Collections.Generic;
using UnityEngine;


public interface IRollData<W>
{
    W Weight { get; }
}

public interface IRollResolver<T, W> where T : IRollData<W>
{
    T ResolveRoll(List<T> possibleRolls);
}

public class GachaResolver<T> : IRollResolver<T, int> where T : IRollData<int>
{
    public T ResolveRoll(List<T> possibleRolls)
    {
        if (possibleRolls == null || possibleRolls.Count == 0) return default;

        int totalWeight = 0;
        foreach (T roll in possibleRolls)
        {
            totalWeight += roll.Weight;
        }

        int randomPoint = UnityEngine.Random.Range(0, totalWeight);
        int currentSum = 0;

        foreach (T roll in possibleRolls)
        {
            currentSum += roll.Weight;
            if (randomPoint <= currentSum)
            {
                return roll;
            }
        }

        return possibleRolls[possibleRolls.Count - 1];
    }
}