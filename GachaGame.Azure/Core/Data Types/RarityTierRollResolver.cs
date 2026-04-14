using GachaGame.Azure.Core.Interfaces;
using JetBrains.Annotations;

namespace GachaGame.Azure.Core.DataTypes;
[UsedImplicitly]
public struct RarityTierRollResolver : IRollResolver<RarityTier>
{
    public  RarityTier ResolveRoll(List<RarityTier> possibleRolls, PlayerData playerData, out PlayerData playerDataAfterRoll)
    {
        playerDataAfterRoll = playerData;
        if (possibleRolls.Count == 0) return default;
        uint ratioSum = 0;
        foreach (RarityTier roll in possibleRolls) ratioSum += roll.Rarity;
        float numericValue = Random.Shared.NextSingle() * ratioSum;
        foreach (RarityTier roll in possibleRolls)
        {
            numericValue -= roll.Rarity;
            if (numericValue <= 0) return roll;
        }
        return possibleRolls[^1];
    }
}