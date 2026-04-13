using Newtonsoft.Json;

namespace GachaGame.Azure.Core.PlayFabHelpers;
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class TitleAuthenticationContext
{
    [JsonRequired]
    public required string Id { get; set; }
    [JsonRequired]
    public required string EntityToken { get; set; }
}