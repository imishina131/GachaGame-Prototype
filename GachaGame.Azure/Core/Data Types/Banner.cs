using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct Banner
{
    [JsonConstructor]
    Banner(IRollResolver<RarityTier> rarityTierResolver, List<RarityTier> rarityTiers, string currency, int cost)
    {
        RarityTierResolver = rarityTierResolver;
        RarityTiers = rarityTiers;
        Currency = currency;
        Cost = cost;
    }
    [JsonConverter(typeof(RollResolverConverter<RarityTier>)), JsonRequired]
    public required IRollResolver<RarityTier> RarityTierResolver { get; init; }
    [JsonRequired]
    public required List<RarityTier> RarityTiers { get; init; }
    [JsonRequired]
    public required string Currency { get; init; }
    [JsonRequired]
    public required int Cost{ get; init;}
}