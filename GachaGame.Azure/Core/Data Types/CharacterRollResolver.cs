using GachaGame.Azure.Core.Interfaces;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GachaGame.Azure.Core.DataTypes;

[UsedImplicitly]
public struct CharacterRollResolver : IRollResolver<Character>
{
    public Character ResolveRoll(List<Character> possibleRolls, PlayerData playerData)
    {
        if (possibleRolls == null || possibleRolls.Count == 0) return default;

        uint ratioSum = 0;
        foreach (var roll in possibleRolls) ratioSum += roll.Rarity;

        float numericValue = (float)Random.Shared.NextDouble() * ratioSum;
        uint targetRarity = 0;

        foreach (var roll in possibleRolls)
        {
            numericValue -= roll.Rarity;
            if (numericValue <= 0)
            {
                targetRarity = roll.Rarity;
                break;
            }
        }

        var tierPool = possibleRolls.Where(character => character.Rarity == targetRarity).ToList();

        return tierPool[Random.Shared.Next(tierPool.Count)];
    }
}