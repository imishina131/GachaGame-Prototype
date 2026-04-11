using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
FunctionsApplicationBuilder builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

PlayFab.PlayFabSettings.staticSettings.TitleId = 
    Environment.GetEnvironmentVariable("PLAYFAB_TITLE_ID");
PlayFab.PlayFabSettings.staticSettings.DeveloperSecretKey = 
    Environment.GetEnvironmentVariable("PLAYFAB_SECRET_KEY");

builder.Build().Run();