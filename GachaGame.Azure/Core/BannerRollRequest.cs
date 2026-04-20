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
/// <summary>
/// Represents a request to roll a banner
/// </summary>
/// <param name="logger">A <see cref="ILogger"/> to log details</param>
public class BannerRollRequest(ILogger<BannerRollRequest> logger)
{
    /// <summary>
    /// Executes the banner roll request received from the client
    /// </summary>
    /// <param name="req">The <see cref="HttpRequest"/> to with the <see cref="BannerRollRequestData"/> from the client</param>
    /// <param name="cancellationToken">The token to cancel this roll</param>
    /// <returns>The resulting <see cref="IActionResult"/> with the <see cref="RollData"/> result from this roll</returns>
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
            EntityId = context.CallerEntityProfile.Entity.Id,
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
         PlayFabResult<GetUserInventoryResult> userInventory = await PlayFabServerAPI.GetUserInventoryAsync(new()
         {
             AuthenticationContext = userAuth,
             PlayFabId = userAuth.PlayFabId
         });
         if(!userInventory.Result.VirtualCurrency.TryGetValue(banner.Currency, out int currentAmount) || currentAmount < banner.Cost)
             return new();
         await PlayFabServerAPI.SubtractUserVirtualCurrencyAsync(new()
         {
             VirtualCurrency = banner.Currency,
             Amount = banner.Cost,
             PlayFabId = userAuth.PlayFabId,
             AuthenticationContext = userAuth
        });
        RarityTier tier = banner.RarityTierResolver.ResolveRoll(banner.RarityTiers, playerData);
        Character rolledCharacter = tier.CharacterResolver.ResolveRoll(tier.Characters, playerData);
        return new(DateTime.Now, context.FunctionArgument.BannerId, rolledCharacter.CharacterID, tier.TierID);
    }
    
    async Task UpdatePlayerDataAsync(RollData result, PlayerData playerData, PlayFabAuthenticationContext userAuth)
    {
        if(!result.Success) return;
        if (!playerData.BannerData.TryGetValue(result.BannerRolled, out PlayerBannerData? bannerData))
        {
            bannerData = new();
            playerData.BannerData[result.BannerRolled] = bannerData;
        }
        bannerData.RollData.Add(result);
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