using Newtonsoft.Json;
using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.Interfaces;
namespace GachaGame.Azure.Core.DataTypes;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct RarityTier : IRollData
{
    [JsonConstructor]
    RarityTier(Guid tierID, uint rarity, IRollResolver<Character> characterResolver, List<Character> characters)
    {
        TierID = tierID;
        Rarity = rarity;
        CharacterResolver = characterResolver;
        Characters = characters;
    }
    [JsonRequired]
    public required Guid TierID { get; init; }
    [JsonRequired]
    public required uint Rarity { get; init; }

    [JsonConverter(typeof(RollResolverConverter<Character>)), JsonRequired]
    public required IRollResolver<Character> CharacterResolver { get; init; }
    [JsonRequired]
    public required List<Character> Characters { get; init; }
}