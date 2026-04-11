using Newtonsoft.Json;
using GachaGame_Prototype.Azure.Helpers;
using GachaGame_Prototype.Azure.Interfaces;

namespace GachaGame_Prototype.Azure.Core.Data_Types;

public class RarityTier : IRollData
{
    public Guid TierID { get; set; }
    public uint Rarity { get; set; }
    [JsonConverter(typeof(RollResolverConverter<Character>))]
    public IRollResolver<Character>? CharacterResolver { get; set; }
    public List<Character>? Characters { get; set; }
}