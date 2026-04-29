//Irina Mishina
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
    /// <summary>
    /// Gets current pity of the player and adds 1 each time it goes through this function
    /// </summary>
    uint pity = rollContext.PlayerData.BannerData.TryGetValue(rollContext.BannerName, out PlayerBannerData? bannerData) ? bannerData.CurrentPity : 0;

    logger.LogInformation("Pity BEFORE: " + pity);
    pity++;

    uint rarityValue3Star;
    uint rarityValue4Star;
    uint rarityValue5Star;

        /// <summary>
        /// Checks pity value to determine whether the chances of getting a 5 star will go higher and changes all other odds to keep 100
        /// </summary>
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

    /// <summary>
    /// Updates all rarities for the rolls
    /// </summary>
    rollContext.Banner.RarityTiers[0].Rarity = rarityValue3Star;
    rollContext.Banner.RarityTiers[1].Rarity = rarityValue4Star;
    rollContext.Banner.RarityTiers[2].Rarity = rarityValue5Star;

    logger.LogInformation("Rarity Value 5 Star: " + rarityValue5Star);
    logger.LogInformation("Rarity Value 4 Star: " + rarityValue4Star);
    logger.LogInformation("Rarity Value 3 Star: " + rarityValue3Star);
    logger.LogInformation("Pity: " + pity);

    /// <summary>
    /// Updates the pity within the current banner and selects a reward to return it and check whether its the 5 star or not (will reset pity to 0)
    /// </summary>
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