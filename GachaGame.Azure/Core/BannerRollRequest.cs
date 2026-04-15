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
        FunctionExecutionContext<BannerRollRequestData>? context = 
            JsonConvert.DeserializeObject<FunctionExecutionContext<BannerRollRequestData>>(await new StreamReader(req.Body).ReadToEndAsync(cancellationToken));
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
        await Task.WhenAll(getBannerInfo, getPlayerObjects).WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
        PlayerData playerData =
            getPlayerObjects.Result.Result.Objects.TryGetValue("PlayerData", out ObjectResult? playFabObject)
            && playFabObject?.DataObject is not null
                ? JsonConvert.DeserializeObject<PlayerData>(JsonConvert.SerializeObject(playFabObject.DataObject)) ?? new()
                : new();
        if (!getBannerInfo.Result.Result.Data.TryGetValue(context.FunctionArgument.BannerId, out string? bannerJson)) 
            return new BadRequestObjectResult($"Banner '{context.FunctionArgument.BannerId}' not found in Title Data");
        Banner banner = JsonConvert.DeserializeObject<Banner>(bannerJson);
        RollData result = await TryRollBanner(context, banner, playerData, userAuth);
        await UpdatePlayerDataAsync(result, playerData, userAuth);
        return new OkObjectResult(result);
    }

    async Task<RollData> TryRollBanner(FunctionExecutionContext<BannerRollRequestData> context, Banner banner, PlayerData playerData, PlayFabAuthenticationContext userAuth)
    {
        //This is for the currency system which needs to be setup on the client api so we can still test rolls
        // PlayFabResult<GetUserInventoryResult> userInventory = await PlayFabServerAPI.GetUserInventoryAsync(new()
        // {
        //     AuthenticationContext = userAuth,
        //     PlayFabId = userAuth.PlayFabId
        // });
        // if(!userInventory.Result.VirtualCurrency.TryGetValue(banner.Currency, out int currentAmount) || currentAmount < banner.Cost)
        //     return new();
        // await PlayFabServerAPI.SubtractUserVirtualCurrencyAsync(new()
        // {
        //     VirtualCurrency = banner.Currency,
        //     Amount = banner.Cost,
        //     PlayFabId = userAuth.PlayFabId,
        //     AuthenticationContext = userAuth
        // });
        RarityTier tier = banner.RarityTierResolver.ResolveRoll(banner.RarityTiers, playerData);
        Character rolledCharacter = tier.CharacterResolver.ResolveRoll(tier.Characters, playerData);
        return new(DateTime.Now, context.FunctionArgument.BannerId, rolledCharacter.CharacterID, tier.TierID);
    }
    
    async Task UpdatePlayerDataAsync(RollData result, PlayerData playerData, PlayFabAuthenticationContext userAuth)
    {
        if(!result.Success) return;
        playerData.PlayerRollData.Add(result);
        await PlayFabDataAPI.SetObjectsAsync(new()
        {
            AuthenticationContext = userAuth,
            Entity = new()
            {
                Type = userAuth.EntityType,
                Id = userAuth.EntityId
            },
            Objects = [new()
            {
                ObjectName = "PlayerData",
                 DataObject = playerData,
            }]
        });

    }
}