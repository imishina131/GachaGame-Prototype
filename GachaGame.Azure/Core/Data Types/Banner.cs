using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public readonly struct Banner
{
    [JsonConverter(typeof(RollResolverConverter<RarityTier>))]
    public IRollResolver<RarityTier> RarityTierResolver { get; }
    public List<RarityTier> RarityTiers { get; }
    public Guid CurrencyID { get; }
}