using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;

public class Banner
{
    [JsonConverter(typeof(RollResolverConverter<RarityTier>))]
    IRollResolver<RarityTier>? RarityTierResolver { get; set; }
    public List<RarityTier>? RarityTiers { get; set; }
}