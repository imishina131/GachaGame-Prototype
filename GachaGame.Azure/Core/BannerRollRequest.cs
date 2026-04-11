using GachaGame_Prototype.Azure;
using GachaGame.Azure.Core.DataTypes;
using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.PlayfabHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.DataModels;
using PlayFab.ServerModels;

namespace GachaGame.Azure;

public class BannerRollRequest(ILogger<BannerRollRequest> logger)
{
    [Function("BannerRollRequest")]
    public async Task<IActionResult> Run
    (
        [HttpTrigger(AuthorizationLevel.Function, "post")]
        HttpRequest req
    )
    {
        FunctionExecutionContext<BannerRollRequestData>? context;
        try
        {
            context =
                JsonConvert.DeserializeObject<FunctionExecutionContext<BannerRollRequestData>>(
                    await new StreamReader(req.Body).ReadToEndAsync());
        }
        catch(Exception e)
        {
            logger.LogError(e, "Failed to deserialize request");
            return new BadRequestObjectResult("Failed to deserialize request");
        }

        if (context?.FunctionArgument.BannerId is null)
        {
            logger.LogError("Failed to deserialize request");
            return new BadRequestObjectResult("Failed to deserialize request");
        }
        if (!req.Headers.TryGetValue("X-EntityToken", out StringValues entityToken)) return new BadRequestObjectResult("Missing Entity Token header");
        PlayFabAuthenticationContext userAuth = new()
        {
            EntityId = context.CallerEntityProfile.Entity.Id,
            PlayFabId = context.CallerEntityProfile.Lineage.MasterPlayerAccountId,
            EntityType = context.CallerEntityProfile.Entity.Type,
            EntityToken = entityToken
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
        return Optional<Banner>.OfNullable(
                JsonConvert.DeserializeObject<Banner>(
                    getBannerInfo.Result.Result.Data[context.FunctionArgument.BannerId]))
            .Bind(b => b.RarityTierResolver?.ResolveRoll(b.RarityTiers))
            .Bind(t => t.CharacterResolver?.ResolveRoll(t.Characters))
            .Match(
                onSome: IActionResult (c) => new OkObjectResult(new { c.CharacterID }),
                onNone: () =>
                {
                    logger.LogError("Roll failed");
                    return new BadRequestObjectResult("Roll failed");
                });
    }
}