using System.Collections.Generic;
using UnityEngine;

public class CharacterRollResolver : IRollResolver<CharacterRoll, float>
{
    public CharacterRoll ResolveRoll(List<CharacterRoll> possibleRolls)
    {
        if (possibleRolls is null || possibleRolls.Count == 0) return null;
        uint ratioSum = 0;
        foreach (CharacterRoll roll in possibleRolls) ratioSum += roll.Rarity;
        float numericValue = Random.value * ratioSum;
        foreach (CharacterRoll roll in possibleRolls)
        {
            numericValue -= roll.Rarity;
            if (numericValue <= 0) return roll;
        }
        return possibleRolls[^1];
    }
}
