using GachaGame_Prototype.Azure;
using GachaGame_Prototype.Azure.Core.Data_Types;
using GachaGame.Azure.Core.DataTypes;
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
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        CancellationToken cancellationToken
    )
    {
        FunctionExecutionContext<BannerRollRequestData>? context = JsonConvert.DeserializeObject<FunctionExecutionContext<BannerRollRequestData>>(await new StreamReader(req.Body).ReadToEndAsync(cancellationToken));
        if (context is null) return new BadRequestObjectResult("Invalid request");
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
        }, cancellationToken);
        Task<PlayFabResult<GetObjectsResponse>> getPlayerObjects = PlayFabDataAPI.GetObjectsAsync(new()
        {
            AuthenticationContext = userAuth,
            Entity = new()
            {
                Id = context.CallerEntityProfile.Entity.Id,
                Type = context.CallerEntityProfile.Entity.Type
            }
        }, cancellationToken);
        await Task.WhenAll(getBannerInfo, getPlayerObjects).WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
        Banner banner = JsonConvert.DeserializeObject<Banner>(getBannerInfo.Result.Result.Data[context.FunctionArgument.BannerId]);
        PlayerData playerData = (PlayerData)getPlayerObjects.Result.Result.Objects["PlayerData"].DataObject ?? new PlayerData();
        RarityTier tier = banner.RarityTierResolver.ResolveRoll(banner.RarityTiers, playerData, out PlayerData dataAfterRoll);
        Character rolledCharacter = tier.CharacterResolver.ResolveRoll(tier.Characters, playerData, out dataAfterRoll);
        playerData = dataAfterRoll;
        if(rolledCharacter.CharacterID == Guid.Empty) return new BadRequestObjectResult("No characters available for this tier");
        await PlayFabDataAPI.SetObjectsAsync(new()
        {
            AuthenticationContext = userAuth,
            Entity = new()
            {
                Id = context.CallerEntityProfile.Entity.Id,
                Type = context.CallerEntityProfile.Entity.Type
            },
            Objects =
            [
                new()
                {
                    ObjectName = "PlayerData",
                    DataObject = playerData
                }
            ]
        });
        logger.LogInformation("Rolled Character: {character}", rolledCharacter.CharacterID);
        return new OkObjectResult(rolledCharacter.CharacterID);
    }
}