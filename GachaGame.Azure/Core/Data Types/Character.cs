using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public record struct Character : IRollData
{
    [method: JsonConstructor]
    Character(Guid characterID, uint rarity)
    {
        CharacterID = characterID;
        Rarity = rarity;
    }
    [JsonRequired]
    public required Guid CharacterID { get; init; }
    [JsonRequired]
    public required uint Rarity { get; init; }
}