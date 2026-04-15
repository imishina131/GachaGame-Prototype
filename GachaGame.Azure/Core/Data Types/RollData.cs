using Newtonsoft.Json;
namespace GachaGame.Azure.Core.DataTypes;

[JsonObject]
public struct RollData
{
    public RollData(DateTime rollTime, string bannerRolled, Guid character, Guid rarityTier)
    {
        RollTime = rollTime;
        Character = character;
        BannerRolled = bannerRolled;
        RarityTier = rarityTier;
        Success = true;
    }
    public RollData()
    {
        RollTime = DateTime.Now;
        Character = Guid.Empty;
        BannerRolled = "";
        RarityTier = Guid.Empty;
        Success = false;
    }
    public DateTime RollTime { get; init; }
    public Guid Character { get; init; }
    public string BannerRolled { get; init; }
    public Guid RarityTier { get; init; }
    public bool Success { get; init; }
}
