using GachaGame.Azure.Core.Interfaces;
using JetBrains.Annotations;

namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// A <see cref="IRollResolver{T}"/> that rolls a <see cref="Character"/> based on the rarity of the <see cref="Character"/> and the pity in the <see cref="PlayerData"/> from the <see cref="RollContext"/>
/// </summary>
[UsedImplicitly]
public struct CharacterRollResolver: IRollResolver<Character>
{
    /// <inheritdoc/>
    public Character ResolveRoll(List<Character> possibleRolls, RollContext rollContext)
    {
        if (possibleRolls.Count == 0) return default;
        uint ratioSum = 0;
        foreach (Character roll in possibleRolls) ratioSum += roll.Rarity;
        float numericValue = Random.Shared.NextSingle() * ratioSum;
        foreach (Character roll in possibleRolls)
        {
            numericValue -= roll.Rarity;
            if (numericValue <= 0) return roll;
        }


        var tierPool = possibleRolls.Where(character => character.Rarity == targetRarity).ToList();

        return tierPool[Random.Shared.Next(tierPool.Count)];

    }
}