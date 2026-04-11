using GachaGame_Prototype.Azure.Helpers;
using GachaGame_Prototype.Azure.Interfaces;
using Newtonsoft.Json;

namespace GachaGame_Prototype.Azure.Core.Data_Types;

public class Banner
{
    [JsonConverter(typeof(RollResolverConverter<RarityTier>))]
    IRollResolver<RarityTier>? RarityTierResolver { get; set; }
    public List<RarityTier>? RarityTiers { get; set; }
}