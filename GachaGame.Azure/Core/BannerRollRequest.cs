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
        // Deserialize the request body into a BannerRollRequestData object
        FunctionExecutionContext<BannerRollRequestData>? context = 
            JsonConvert.DeserializeObject<FunctionExecutionContext<BannerRollRequestData>>(await new StreamReader(req.Body).ReadToEndAsync(cancellationToken));
        if (context is null) return new BadRequestObjectResult("Invalid request");
        //Create a PlayFabAuthenticationContext for the user using data from the execution context
        PlayFabAuthenticationContext userAuth = new()
        {
            EntityId = context.CallerEntityProfile.Entity.Id,
            PlayFabId = context.CallerEntityProfile.Lineage.MasterPlayerAccountId,
            EntityType = context.CallerEntityProfile.Entity.Type,
            EntityToken = context.TitleAuthenticationContext.EntityToken
        };
        //Get the TitleData from PlayFab for the user to use for the roll
        Task<PlayFabResult<GetTitleDataResult>> getBannerInfo = PlayFabServerAPI.GetTitleDataAsync(new()
        {
            Keys = [context.FunctionArgument.BannerId]
        });
        //Get the PlayerData from PlayFab for the user to use for the roll
        Task<PlayFabResult<GetObjectsResponse>> getPlayerObjects = PlayFabDataAPI.GetObjectsAsync(new()
        {
            AuthenticationContext = userAuth,
            Entity = new()
            {
                Id = context.CallerEntityProfile.Entity.Id,
                Type = context.CallerEntityProfile.Entity.Type
            }
        });
        //Wait for the PlayFab API to return the results
        await Task.WhenAll(getBannerInfo, getPlayerObjects);
        //Try and deserialize the PlayerData and if we fail, create some new data to store our results and pity
        PlayerData playerData =
            getPlayerObjects.Result.Result.Objects.TryGetValue("PlayerData", out ObjectResult? playFabObject)
            && playFabObject?.DataObject is not null
                ? JsonConvert.DeserializeObject<PlayerData>(JsonConvert.SerializeObject(playFabObject.DataObject)) ?? new()
                : new();
        //Try and get the banner the user is requesting from the title data and if it exists, deserialize it
        if (!getBannerInfo.Result.Result.Data.TryGetValue(context.FunctionArgument.BannerId, out string? bannerJson)) 
            return new BadRequestObjectResult($"Banner '{context.FunctionArgument.BannerId}' not found in Title Data");
        Banner banner = JsonConvert.DeserializeObject<Banner>(bannerJson);
        //Create roll context to pass into the resolvers to safely make changes to the player data
        RollContext rollContext = new(playerData, banner, context.FunctionArgument.BannerId);
        //Try and roll the banner
        RollData result = await TryRollBanner(context, rollContext, userAuth);
        await UpdatePlayerDataAsync(result, rollContext, userAuth);
        return new OkObjectResult(result);
    }
    async Task<RollData> TryRollBanner(FunctionExecutionContext<BannerRollRequestData> context, RollContext rollContext, PlayFabAuthenticationContext userAuth)
    {
        //Get the user's inventory from PlayFab
         PlayFabResult<GetUserInventoryResult> userInventory = await PlayFabServerAPI.GetUserInventoryAsync(new()
         {
             AuthenticationContext = userAuth,
             PlayFabId = userAuth.PlayFabId
         });
         //Try to get the currency from the user's inventory and return empty if the currency is not found
         if(!userInventory.Result.VirtualCurrency.TryGetValue(rollContext.Banner.Currency, out int currentAmount) || currentAmount < rollContext.Banner.Cost) return new();
         await PlayFabServerAPI.SubtractUserVirtualCurrencyAsync(new()
         {
             VirtualCurrency = rollContext.Banner.Currency,
             Amount = rollContext.Banner.Cost,
             PlayFabId = userAuth.PlayFabId,
             AuthenticationContext = userAuth
        });
        //Roll the tier on the banner
        RarityTier tier = rollContext.Banner.RarityTierResolver.ResolveRoll(rollContext.Banner.RarityTiers, rollContext);
        //Roll the character on the tier
        Character rolledCharacter = tier.CharacterResolver.ResolveRoll(tier.Characters, rollContext);
        //Return the roll data using the results of the roll
        return new(DateTime.Now, context.FunctionArgument.BannerId, rolledCharacter.CharacterID, tier.TierID);
    }
    
    async Task UpdatePlayerDataAsync(RollData result, RollContext rollContext, PlayFabAuthenticationContext userAuth)
    {
        if(!result.Success) return;
        //Apply the player data mutations to the player data
        rollContext.ApplyPlayerDataMutations();
        //If banner data doesn't exist, create it for the player
        if (!rollContext.PlayerData.BannerData.TryGetValue(result.BannerRolled, out PlayerBannerData? bannerData))
        {
            bannerData = new();
            rollContext.PlayerData.BannerData[result.BannerRolled] = bannerData;
        }
        //Add our roll data to the player data
        bannerData.RollData.Add(result);
        //Set the player data to the updated player data
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
                 DataObject = rollContext.PlayerData,
            }]
        });
    }
}