using GachaGame_Prototype.Azure;
using GachaGame.Azure.Core.DataTypes;
using GachaGame.Azure.Core.Helpers;
using GachaGame.Azure.Core.PlayfabHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.DataModels;
using PlayFab.ServerModels;

namespace GachaGame.Azure;

public static class BannerRollRequest
{
    [Function("BannerRollRequest")]
    public static async Task<IActionResult> Run
    (
        [HttpTrigger(AuthorizationLevel.Function, "post")]
        HttpRequest req,
        ILogger logger
    )
    {
        FunctionExecutionContext<BannerRollRequestData>? context =
            JsonConvert.DeserializeObject<FunctionExecutionContext<BannerRollRequestData>>(
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
        return Optional<Banner>.OfNullable(
                JsonConvert.DeserializeObject<Banner>(
                    getBannerInfo.Result.Result.Data[context.FunctionArgument.BannerId]))
            .SelectMany(resolvedBanner => Optional<RarityTier>.OfNullable(
                resolvedBanner.RarityTiers is { Count: > 0 }
                    ? resolvedBanner.RarityTierResolver?.ResolveRoll(resolvedBanner.RarityTiers)
                    : null))
            .SelectMany(resolvedRarityTier => Optional<Character>.OfNullable(
                resolvedRarityTier.Characters is { Count: > 0 }
                    ? resolvedRarityTier.CharacterResolver?.ResolveRoll(resolvedRarityTier.Characters)
                    : null))
            .Match(
                onSome: IActionResult (c) => new OkObjectResult(new { c.CharacterID }),
                onNone: () => new BadRequestObjectResult("Error in banner data")
            );
    }
}