using GachaGame.Azure.Core.DataTypes;
using Newtonsoft.Json;

namespace GachaGame.Azure;
/// <summary>
/// Represents the data of a banner roll request sent from the client
/// </summary>
/// <param name="bannerId">The ID of the <see cref="Banner"/> to try and roll</param>
[JsonObject]
public struct BannerRollRequestData(string bannerId)
{
    /// <summary>
    /// The ID of the <see cref="Banner"/> to try and roll
    /// </summary>
    public string BannerId { get; } = bannerId;
}