using GachaGame.Azure.Core.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// Resolves the <see cref="Character"/> from a roll. It will guarantee a featured pull if the last pull was not featured. Otherwise, pulling a featured <see cref="Character"/> is a coinflip
/// </summary>
[UsedImplicitly]
public struct CharacterRollResolver : IRollResolver<Character>
{
    /// <inheritdoc/>
    public Character ResolveRoll(List<Character> possibleRolls, RollContext rollContext, ILogger logger)
    {
        if (possibleRolls.Count == 0)
        {
            logger.LogWarning("ResolveRoll called with an empty character list.");
            return default;
        }
        List<Character> featuredPool = possibleRolls.Where(c => c.IsFeatured).ToList();
        List<Character> standardPool = possibleRolls.Where(c => !c.IsFeatured).ToList();
        if(featuredPool.Count == 0) return GetRandomFromList(standardPool);
        if (!rollContext.PlayerData.BannerData.TryGetValue(rollContext.BannerName, out PlayerBannerData? bannerEntry)) bannerEntry = new();
        if (bannerEntry.IsGuaranteedFeatured)
        {
            SetGuaranteedFeatured(rollContext, false);
            return GetRandomFromList(featuredPool);
        }
        bool won5050 = Random.Shared.NextSingle() > 0.5f;
        if(won5050)
        {
            SetGuaranteedFeatured(rollContext, false);
            return GetRandomFromList(featuredPool);
        }
        SetGuaranteedFeatured(rollContext, true);
        return GetRandomFromList(standardPool);
    }

    void SetGuaranteedFeatured(RollContext rollContext, bool guaranteedFeaturedStatus)
    {
        rollContext.TryAddMutation(data =>
        {
            if (!data.BannerData.TryGetValue(rollContext.BannerName, out PlayerBannerData? bannerEntry)) bannerEntry = new();
            bannerEntry.IsGuaranteedFeatured = guaranteedFeaturedStatus;
            data.BannerData[rollContext.BannerName] = bannerEntry;
        });
    }
    Character GetRandomFromList(List<Character> characters)
    {
        uint ratioSum = 0;
        foreach (Character roll in characters)
        {
            ratioSum += roll.Rarity;
        }
        float numericValue = Random.Shared.NextSingle() * ratioSum;
        foreach (Character roll in characters)
        {
            numericValue -= roll.Rarity;
            if (numericValue <= 0)
            {
                return roll;
            }
        }
        return characters[Random.Shared.Next(characters.Count)];
    }
}