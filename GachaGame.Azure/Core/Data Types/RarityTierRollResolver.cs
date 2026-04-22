using GachaGame.Azure.Core.Interfaces;
using JetBrains.Annotations;

namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// Resolves the rolled <see cref="RarityTier"/> from the options provided
/// </summary>
[UsedImplicitly]
public struct RarityTierRollResolver : IRollResolver<RarityTier>
{
    /// <inheritdoc/>
    public  RarityTier ResolveRoll(List<RarityTier> possibleRolls, RollContext rollContext)
    {
        var playerBannerData = rollContext.PlayerData.BannerData[rollContext.BannerName];
        uint pity = playerBannerData.CurrentPity;
        pity++;

        if(playerBannerData != null)
        {
            rollContext.TryAddMutation(PlayerData =>
            {
                if (pity >= 30)
                {
                    uint rarityValue = rollContext.Banner.RarityTiers[0].Rarity;
                    rarityValue = 75;
                }
                else if (pity >= 55)
                {
                    uint rarityValue = rollContext.Banner.RarityTiers[0].Rarity;
                    rarityValue = 85;
                }
                else if (pity >= 76)
                {
                    uint rarityValue = rollContext.Banner.RarityTiers[0].Rarity;
                    rarityValue = 90;
                }
                else if(pity >= 90)
                {
                    uint rarityValue = rollContext.Banner.RarityTiers[0].Rarity;
                    rarityValue = 100;
                }
                else
                {
                }
            });
        }

        //ideas of how code should work
        //uint pity = PlayerBannerData.CurrentPity;
        //pity++;
        //PlayerBannerData.CurrentPity = pity;
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