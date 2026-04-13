using GachaGame_Prototype.Azure;
using GachaGame_Prototype.Azure.Core.Data_Types;
using GachaGame.Azure.Core.DataTypes;
using GachaGame.Azure.Core.PlayFabHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.DataModels;
using PlayFab.ServerModels;
using ObjectResult = PlayFab.DataModels.ObjectResult;

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
        PlayFabAuthenticationContext userAuth = new()
        {
            EntityId = context.TitleAuthenticationContext.Id,
            PlayFabId = context.CallerEntityProfile.Lineage.MasterPlayerAccountId,
            EntityType = context.CallerEntityProfile.Entity.Type,
            EntityToken = context.TitleAuthenticationContext.EntityToken
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
        logger.LogInformation("Banner Roll Requested: {bannerId}", context.FunctionArgument.BannerId);
        if (!getBannerInfo.Result.Result.Data.TryGetValue(context.FunctionArgument.BannerId, out string? bannerJson)) 
            return new BadRequestObjectResult($"Banner '{context.FunctionArgument.BannerId}' not found in Title Data");
        Banner banner = JsonConvert.DeserializeObject<Banner>(bannerJson);
        PlayerData playerData = 
            getPlayerObjects.Result.Result.Objects.TryGetValue("PlayerData", out ObjectResult? playFabObject) 
            && playFabObject?.DataObject is not null
            ? (PlayerData)playFabObject.DataObject 
            : new();
        RarityTier tier = banner.RarityTierResolver.ResolveRoll(banner.RarityTiers, playerData, out PlayerData dataAfterRoll);
        logger.LogInformation("Tier: {Characters}", tier);
        logger.LogInformation("Characters: {Characters}", tier.Characters);
        logger.LogInformation("Resolver: {CharacterResolver}", tier.CharacterResolver);
        Character rolledCharacter = tier.CharacterResolver.ResolveRoll(tier.Characters, playerData, out dataAfterRoll);
        playerData = dataAfterRoll;
        if(rolledCharacter.CharacterID == Guid.Empty) return new BadRequestObjectResult("Character not rolled, User lacks currency or there are no characters in the tier");
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