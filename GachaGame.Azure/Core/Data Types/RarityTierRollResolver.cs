using GachaGame.Azure.Core.Interfaces;
using JetBrains.Annotations;

namespace GachaGame.Azure.Core.DataTypes;
[UsedImplicitly]
public class RarityTierRollResolver : IRollResolver<RarityTier>
{
    public RarityTier? ResolveRoll(List<RarityTier>? possibleRolls)
    {
        if (possibleRolls is null || possibleRolls.Count == 0) return null;
        uint ratioSum = 0;
        foreach (RarityTier roll in possibleRolls) ratioSum += roll.Rarity;
        float numericValue = Random.Shared.Next() * ratioSum;
        foreach (RarityTier roll in possibleRolls)
        {
            numericValue -= roll.Rarity;
            if (numericValue <= 0) return roll;
        }
        return possibleRolls[^1];
    }
}