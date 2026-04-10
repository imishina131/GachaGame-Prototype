namespace GachaGame_Prototype.Azure;

public class FunctionExecutionContext<T>
{
    public required PlayFab.ProfilesModels.EntityProfileBody CallerEntityProfile { get; set; }
    public  TitleAuthenticationContext? TitleAuthenticationContext { get; set; }
    public bool? GeneratePlayStreamEvent { get; set; }
    public required T FunctionArgument { get; set; }
}

public class FunctionExecutionContext : FunctionExecutionContext<object>
{
    
}