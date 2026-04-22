using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;
namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// Represents a banner that can be rolled. Stored as a JSON object on PlayFab
/// </summary>
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
    /// <summary>
    /// The <see cref="IRollResolver{T}"/> for the rarity tiers of this banner.
    /// </summary>
    [JsonConverter(typeof(RollResolverConverter<RarityTier>)), JsonRequired]
    public required IRollResolver<RarityTier> RarityTierResolver { get; init; }
    /// <summary>
    /// Each <see cref="RarityTier"/> in this banner.
    /// </summary>
    [JsonRequired]
    public required List<RarityTier> RarityTiers { get; init; }
    /// <summary>
    /// The currency used to roll this banner.
    /// </summary>
    [JsonRequired]
    public required string Currency { get; init; }
    /// <summary>
    /// The cost of rolling this banner.
    /// </summary>
    [JsonRequired]
    public required int Cost{ get; init;}
}