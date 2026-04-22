using Newtonsoft.Json;

namespace GachaGame.Azure.Core.PlayFabHelpers;
/// <summary>
/// Represents the context of a function call. Automatically populated by PlayFab when the function call is routed through the PlayFab system
/// </summary>
/// <typeparam name="T">The type of object data the request contains</typeparam>
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class FunctionExecutionContext<T>
{
    /// <summary>
    /// The entity profile of the caller.
    /// </summary>
    [JsonRequired]
    public required PlayFab.ProfilesModels.EntityProfileBody CallerEntityProfile { get; set; }
    /// <summary>
    /// The authentication context of the caller.
    /// </summary>
    [JsonRequired]
    public required TitleAuthenticationContext TitleAuthenticationContext { get; set; }
    /// <summary>
    /// Whether to generate a PlayStream event for this function call.
    /// </summary>
    public bool GeneratePlayStreamEvent { get; set; }
    /// <summary>
    /// The function argument
    /// </summary>
    [JsonRequired]
    public required T FunctionArgument { get; set; }
}
/// <inheritdoc/>
public class FunctionExecutionContext : FunctionExecutionContext<object>
{
    
}