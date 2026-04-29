using GachaGame.Azure.Core.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// Resolves the rolled <see cref="RarityTier"/> from the options provided
/// </summary>
[UsedImplicitly]
public struct RarityTierRollResolver : IRollResolver<RarityTier>
{
    /// <inheritdoc/>
    public RarityTier ResolveRoll(List<RarityTier> possibleRolls, RollContext rollContext, ILogger logger)
{
    uint pity = rollContext.PlayerData.BannerData.TryGetValue(rollContext.BannerName, out PlayerBannerData? bannerData) ? bannerData.CurrentPity : 0;

    logger.LogInformation("Pity BEFORE: " + pity);
    pity++;

    uint rarityValue3Star;
    uint rarityValue4Star;
    uint rarityValue5Star;
    switch (pity)
    {
        case >= 90:
            rarityValue5Star = 100;
            rarityValue4Star = 0;
            rarityValue3Star = 0;
            pity = 0;
            break;
        case >= 74:
            rarityValue5Star = 25;
            rarityValue4Star = 18;
            rarityValue3Star = 57;
            break;
        case >= 55:
            rarityValue5Star = 15; 
            rarityValue4Star = 20; 
            rarityValue3Star = 65;
            break;
        case >= 30:
            rarityValue5Star = 10;
            rarityValue4Star = 23;
            rarityValue3Star = 67;
            break;
        default:
            rarityValue5Star = 5; 
            rarityValue4Star = 25; 
            rarityValue3Star = 70;
            break;
    }

    rollContext.Banner.RarityTiers[0].Rarity = rarityValue3Star;
    rollContext.Banner.RarityTiers[1].Rarity = rarityValue4Star;
    rollContext.Banner.RarityTiers[2].Rarity = rarityValue5Star;

    logger.LogInformation("Rarity Value 5 Star: " + rarityValue5Star);
    logger.LogInformation("Rarity Value 4 Star: " + rarityValue4Star);
    logger.LogInformation("Rarity Value 3 Star: " + rarityValue3Star);
    logger.LogInformation("Pity: " + pity);
    rollContext.TryAddMutation(playerData =>
    {
        if (!playerData.BannerData.ContainsKey(rollContext.BannerName))
            playerData.BannerData[rollContext.BannerName] = new();
        playerData.BannerData[rollContext.BannerName].CurrentPity = pity;
    });

    if (possibleRolls.Count == 0) return default;
    uint ratioSum = 0;
    foreach (RarityTier roll in possibleRolls) ratioSum += roll.Rarity;
    float numericValue = Random.Shared.NextSingle() * ratioSum;
    foreach (RarityTier roll in possibleRolls)
    {
        numericValue -= roll.Rarity;
        if (!(numericValue <= 0)) continue;
        if (roll != possibleRolls[^1]) return roll;
        rollContext.TryAddMutation(playerData =>
        {
            if (!playerData.BannerData.ContainsKey(rollContext.BannerName))
                playerData.BannerData[rollContext.BannerName] = new();
            playerData.BannerData[rollContext.BannerName].CurrentPity = 0;
        });
    }
    return possibleRolls[^1];
}
}