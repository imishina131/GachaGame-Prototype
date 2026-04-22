using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// Represents a character that can be rolled. Stored as a JSON object on PlayFab inside a <see cref="RarityTier"/> in the title data
/// </summary>
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public record struct Character : IRollData
{
    [method: JsonConstructor]
    Character(Guid characterID, uint rarity)
    {
        CharacterID = characterID;
        Rarity = rarity;
    }
    /// <summary>
    /// The ID of the character. This is used for lookups on the client side.
    /// </summary>
    [JsonRequired]
    public required Guid CharacterID { get; init; }
    /// <summary>
    /// The rarity of the character.
    /// </summary>
    [JsonRequired]
    public required uint Rarity { get; init; }
}