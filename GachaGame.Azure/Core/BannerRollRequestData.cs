using Newtonsoft.Json;

namespace GachaGame_Prototype.Azure;

[method: JsonConstructor]
public struct BannerRollRequestData(string bannerId)
{
    public string BannerId { get; } = bannerId;
}