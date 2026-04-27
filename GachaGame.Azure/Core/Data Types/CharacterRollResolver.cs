using System;
using System.Collections.Generic;
using System.Linq;
using GachaGame.Azure.Core.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace GachaGame.Azure.Core.DataTypes;

[UsedImplicitly]
public struct CharacterRollResolver : IRollResolver<Character>
{
    private readonly ILogger _logger;

    public CharacterRollResolver(ILogger<CharacterRollResolver> logger)
    {
        _logger = logger;
    }

    public Character ResolveRoll(List<Character> possibleRolls, RollContext rollContext)
    {
        if (possibleRolls == null || possibleRolls.Count == 0)
        {
            _logger?.LogWarning("ResolveRoll called with an empty character list.");
            return default;
        }

        uint targetRarity = possibleRolls[0].Rarity;

        if (targetRarity == 5)
        {
            int half = possibleRolls.Count / 2;
            List<Character> featuredPool = possibleRolls.Take(half).ToList();
            List<Character> standardPool = possibleRolls.Skip(half).ToList();

            Character selected;
            bool won5050 = Random.Shared.NextSingle() > 0.5f;

            if (won5050 && featuredPool.Count > 0)
            {
                _logger?.LogInformation("50/50 WON: Select from event Gacha pool.");
                selected = featuredPool[Random.Shared.Next(featuredPool.Count)];
            }
            else
            {
                _logger?.LogInformation("50/50 LOST: select from default pool.");
                List<Character> fallbackPool = standardPool.Count > 0 ? standardPool : possibleRolls;
                selected = fallbackPool[Random.Shared.Next(fallbackPool.Count)];
            }


            var currentBanner = rollContext.Banner;

            rollContext.TryAddMutation(data =>
            {

                string bannerKey = currentBanner.GetHashCode().ToString();

                if (data.BannerData.TryGetValue(bannerKey, out var bannerEntry))
                {
                    bannerEntry.CurrentPity = 0;
                }
            });

            return selected;
        }

        uint ratioSum = 0;
        foreach (Character roll in possibleRolls) ratioSum += roll.Rarity;

        float numericValue = Random.Shared.NextSingle() * ratioSum;
        foreach (Character roll in possibleRolls)
        {
            numericValue -= roll.Rarity;
            if (numericValue <= 0) return roll;
        }

        return possibleRolls[Random.Shared.Next(possibleRolls.Count)];
    }
}