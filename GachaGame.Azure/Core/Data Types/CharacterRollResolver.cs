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

        if (targetRarity == 5)
        {
            var featuredPool = tierPool.Where(character => character.IsFeatured).ToList();
            var standardPool = tierPool.Where(character => !character.IsFeatured).ToList();

            bool winFiftyFifty = playerData.HasPityCounter || Random.Shared.NextDouble() < 0.5;

            if (winFiftyFifty && featuredPool.Count > 0)
            {
                return featuredPool[Random.Shared.Next(featuredPool.Count)];
            }

            if (standardPool.Count > 0)
            {
                return standardPool[Random.Shared.Next(standardPool.Count)];
            }
        }

        return tierPool[Random.Shared.Next(tierPool.Count)];
    }
}