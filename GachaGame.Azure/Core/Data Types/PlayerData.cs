using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject]
public class PlayerData
{
    public Dictionary<string, PlayerBannerData> BannerData {get; set;} = new();
}

[JsonObject]
public class PlayerBannerData
{
    public HashSet<RollData> RollData {get; set;} = [];
    public uint CurrentPity {get; set;}
}