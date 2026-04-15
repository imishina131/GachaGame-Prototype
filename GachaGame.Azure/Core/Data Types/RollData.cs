using Newtonsoft.Json;
namespace GachaGame.Azure.Core.DataTypes;

[JsonObject]
public struct RollData : IEquatable<RollData>
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
    public bool Equals(RollData other)
    {
        return RollTime.Equals(other.RollTime) && Character.Equals(other.Character) && BannerRolled == other.BannerRolled && RarityTier.Equals(other.RarityTier) && Success == other.Success;
    }
    public override bool Equals(object? obj)
    {
        return obj is RollData other && Equals(other);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(RollTime, Character, BannerRolled, RarityTier, Success);
    }

    public static bool operator ==(RollData left, RollData right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RollData left, RollData right)
    {
        return !(left == right);
    }
}
