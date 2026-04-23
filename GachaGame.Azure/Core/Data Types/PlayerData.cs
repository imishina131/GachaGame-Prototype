using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// Represents the data of a player stored in PlayFab as a JSON object
/// </summary>
[JsonObject]
public class PlayerData
{
    public PlayerData(){}
    public PlayerData(PlayerData playerData)
    {
        BannerData = new(playerData.BannerData);
    }
    public Dictionary<string, PlayerBannerData> BannerData {get; init;} = new();
}

/// <summary>
/// Represents the data of a player relating to a specific <see cref="Banner"/> stored in PlayFab as a JSON object
/// </summary>
[JsonObject]
public class PlayerBannerData
{
    public bool IsGuaranteedFeatured {get; set;}
    public uint CurrentPity {get; set;}
}