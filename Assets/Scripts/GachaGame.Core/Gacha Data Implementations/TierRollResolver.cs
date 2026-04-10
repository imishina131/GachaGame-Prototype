using System.Collections.Generic;
using UnityEngine;

public class TierRollResolver : IRollResolver<TierRoll>
{
    public TierRoll ResolveRoll(List<TierRoll> possibleRolls)
    {
        if (possibleRolls is null || possibleRolls.Count == 0) return null;
        uint ratioSum = 0;
        foreach (TierRoll roll in possibleRolls) ratioSum += roll.Rarity;
        float numericValue = Random.value * ratioSum;
        foreach (TierRoll roll in possibleRolls)
        {
            numericValue -= roll.Rarity;
            if (numericValue <= 0) return roll;
        }
        return possibleRolls[^1];
    }
}
