using Newtonsoft.Json;
using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.Interfaces;
namespace GachaGame.Azure.Core.DataTypes;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public readonly struct RarityTier : IRollData
{
    public Guid TierID { get; }
    public uint Rarity { get; }
    [JsonConverter(typeof(RollResolverConverter<Character>))]
    public IRollResolver<Character> CharacterResolver { get; }
    public List<Character> Characters { get; }
}