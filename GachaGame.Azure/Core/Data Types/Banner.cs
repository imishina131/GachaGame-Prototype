using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct Banner
{
    [JsonConstructor]
    Banner(Guid bannerID, IRollResolver<RarityTier> rarityTierResolver, List<RarityTier> rarityTiers, Guid currencyID)
    {
        RarityTierResolver = rarityTierResolver;
        RarityTiers = rarityTiers;
        CurrencyID = currencyID;
    }
    [JsonConverter(typeof(RollResolverConverter<RarityTier>)), JsonRequired]
    public required IRollResolver<RarityTier> RarityTierResolver { get; init; }
    [JsonRequired]
    public required List<RarityTier> RarityTiers { get; init; }
    [JsonRequired]
    public required Guid CurrencyID { get; init; }
}