using Newtonsoft.Json;
using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.Interfaces;
namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// Represents a tier of characters that can be rolled. Stored as a JSON object on PlayFab
/// </summary>
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class RarityTier : IRollData
{
    [JsonConstructor]
    RarityTier(Guid tierID, uint rarity, IRollResolver<Character> characterResolver, List<Character> characters)
    {
        TierID = tierID;
        Rarity = rarity;
        CharacterResolver = characterResolver;
        Characters = characters;
    }
    /// <summary>
    /// The ID of the rarity tier. This is used for lookups on the client side.
    /// </summary>
    [JsonRequired]
    public required Guid TierID { get; init; }
    /// <summary>
    /// The rarity of rolling this tier.
    /// </summary>
    [JsonRequired]
    public required uint Rarity { get; set; }
    /// <summary>
    /// The <see cref="IRollResolver{T}"/> for each <see cref="Character"/> in this tier.
    /// </summary>
    [JsonConverter(typeof(RollResolverConverter<Character>)), JsonRequired]
    public required IRollResolver<Character> CharacterResolver { get; init; }
    /// <summary>
    /// Each <see cref="Character"/> in this tier.
    /// </summary>
    [JsonRequired]
    public required List<Character> Characters { get; init; }
}