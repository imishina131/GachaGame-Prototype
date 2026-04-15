using Newtonsoft.Json;

namespace GachaGame.Azure;

[JsonObject]
public struct BannerRollRequestData(string bannerId)
{
    public string BannerId { get; } = bannerId;
}