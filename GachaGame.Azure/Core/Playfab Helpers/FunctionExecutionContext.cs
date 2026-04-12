using Newtonsoft.Json;

namespace GachaGame.Azure.Core.PlayfabHelpers;
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class FunctionExecutionContext<T>
{
    [JsonRequired]
    public required PlayFab.ProfilesModels.EntityProfileBody CallerEntityProfile { get; set; }
    [JsonRequired]
    public required TitleAuthenticationContext TitleAuthenticationContext { get; set; }
    public bool GeneratePlayStreamEvent { get; set; }
    [JsonRequired]
    public required T FunctionArgument { get; set; }
}

public class FunctionExecutionContext : FunctionExecutionContext<object>
{
    
}