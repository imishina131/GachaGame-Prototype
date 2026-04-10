using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ServerModels;

namespace GachaGame_Prototype.Azure;

public static class BannerRoller
{
    [Function("BannerRoller")]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        FunctionExecutionContext<dynamic> context =
            JsonConvert.DeserializeObject<dynamic>(await new StreamReader(req.Body).ReadToEndAsync()) ??
            throw new InvalidOperationException();
        dynamic args = context.FunctionArgument;
        PlayFabResult<GetTitleDataResult>? titleData = await PlayFabServerAPI.GetTitleDataAsync(new()
        {
            Keys = new List<string>()
        });
        return new OkResult();
    }
}