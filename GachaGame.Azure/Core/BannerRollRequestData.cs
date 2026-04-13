using Newtonsoft.Json;

namespace GachaGame.Azure;

[method: JsonConstructor]
public struct BannerRollRequestData(string bannerId)
{
    public string BannerId { get; } = bannerId;
}