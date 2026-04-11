using Newtonsoft.Json;
using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.Interfaces;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class RarityTier : IRollData
{
    public Guid TierID { get; set; }
    public uint Rarity { get; set; }
    [JsonConverter(typeof(RollResolverConverter<Character>))]
    public IRollResolver<Character>? CharacterResolver { get; set; }
    public List<Character>? Characters { get; set; }
}