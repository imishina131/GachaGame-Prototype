using System.Collections.Generic;
using UnityEngine;
public class LocalRollResolver<T> : IRollResolver<T> where T : IRollData
{
    public T ResolveRoll(List<T> possibleRolls)
    {
        if (possibleRolls == null || possibleRolls.Count == 0) return default;
        int totalWeight = 0;
        foreach (T roll in possibleRolls)
        {
            totalWeight += roll.Weight;
        }

        int randomPoint = Random.Range(0, totalWeight);
        int currentSum = 0;

        foreach (T roll in possibleRolls)
        {
            currentSum += roll.Weight;
            if (randomPoint <= currentSum)
            {
                return roll;
            }
        }

        return possibleRolls[^1];
    }
}
