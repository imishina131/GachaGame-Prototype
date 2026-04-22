using Newtonsoft.Json;

namespace GachaGame.Azure.Core.PlayFabHelpers;
/// <summary>
/// Represents the context of a title authentication call. Automatically populated by PlayFab when the title authentication call is routed through the PlayFab system
/// </summary>
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class TitleAuthenticationContext
{
    /// <summary>
    /// The ID of the entity that is making the call.
    /// </summary>
    [JsonRequired]
    public required string Id { get; set; }
    /// <summary>
    /// The entity token of the entity that is making the call.
    /// </summary>
    [JsonRequired]
    public required string EntityToken { get; set; }
}