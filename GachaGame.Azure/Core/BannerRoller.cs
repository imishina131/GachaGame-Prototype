using GachaGame_Prototype.Azure.Core.Data_Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.DataModels;
using PlayFab.ServerModels;
namespace GachaGame_Prototype.Azure;

public static class BannerRoller
{
    [Function("BannerRoller")]
    public static async Task<IActionResult> Run
    (
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        ILogger logger
    )
    {
        FunctionExecutionContext<RollRequestData>? context =
            JsonConvert.DeserializeObject<FunctionExecutionContext<RollRequestData>>(
                await new StreamReader(req.Body).ReadToEndAsync());
        if (context?.FunctionArgument.BannerId is null) return new BadRequestResult();
        PlayFabAuthenticationContext userAuth = new()
        {
            EntityId = context.CallerEntityProfile.Entity.Id,
            PlayFabId = context.CallerEntityProfile.Lineage.MasterPlayerAccountId,
            EntityType = context.CallerEntityProfile.Entity.Type
        };
        Task<PlayFabResult<GetTitleDataResult>> getBannerInfo = PlayFabServerAPI.GetTitleDataAsync(new()
        {
            Keys = [context.FunctionArgument.BannerId]
        });
        Task<PlayFabResult<GetObjectsResponse>> getPlayerObjects = PlayFabDataAPI.GetObjectsAsync(new()
        {
            AuthenticationContext = userAuth,
            Entity = new()
            {
                Id = context.CallerEntityProfile.Entity.Id,
                Type = context.CallerEntityProfile.Entity.Type
            }
        });
        await Task.WhenAll(getBannerInfo, getPlayerObjects);
        if (getBannerInfo.Result.Error != null) 
            return new BadRequestObjectResult(getBannerInfo.Result.Error.ErrorMessage);
        if (getPlayerObjects.Result.Error != null)
            return new BadRequestObjectResult(getPlayerObjects.Result.Error.ErrorMessage);
        JsonConvert.DeserializeObject<Banner>(getBannerInfo.Result.Result.Data[context.FunctionArgument.BannerId]);
        
        return new OkResult();
    }
}