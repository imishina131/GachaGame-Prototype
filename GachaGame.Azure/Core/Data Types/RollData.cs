using Newtonsoft.Json;
namespace GachaGame.Azure.Core.DataTypes;
/// <summary>
/// Represents the resulting data from roll on a <see cref="Banner"/> sent to the client to display to the user
/// </summary>
[JsonObject]
public struct RollData : IEquatable<RollData>
{
    /// <summary>
    /// Creates a new <see cref="RollData"/>
    /// </summary>
    /// <param name="rollTime">The time at which the banner was rolled</param>
    /// <param name="bannerRolled">The <see cref="Banner"/> that was rolled</param>
    /// <param name="character">The resulting <see cref="Character"/> from the roll</param>
    /// <param name="rarityTier">The resulting <see cref="RarityTier"/> of the roll</param>
    public RollData(DateTime rollTime, string bannerRolled, Guid character, Guid rarityTier)
    {
        RollTime = rollTime;
        Character = character;
        BannerRolled = bannerRolled;
        RarityTier = rarityTier;
        Success = true;
    }
    /// <summary>
    /// Creates a new <see cref="RollData"/> with default values.
    /// </summary>
    public RollData()
    {
        RollTime = DateTime.Now;
        Character = Guid.Empty;
        BannerRolled = "";
        RarityTier = Guid.Empty;
        Success = false;
    }
    /// <summary>
    /// The time at which the banner was rolled
    /// </summary>
    public DateTime RollTime { get; init; }
    /// <summary>
    /// The resulting <see cref="Character"/> from the roll
    /// </summary>
    public Guid Character { get; init; }
    /// <summary>
    /// The <see cref="Banner"/> that was rolled
    /// </summary>
    public string BannerRolled { get; init; }
    /// <summary>
    /// The resulting <see cref="RarityTier"/> of the roll
    /// </summary>
    public Guid RarityTier { get; init; }
    /// <summary>
    /// Whether the roll was successful
    /// </summary>
    public bool Success { get; init; }
    /// <inheritdoc/>
    public bool Equals(RollData other)
    {
        return RollTime.Equals(other.RollTime) && Character.Equals(other.Character) && BannerRolled == other.BannerRolled && RarityTier.Equals(other.RarityTier) && Success == other.Success;
    }
    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is RollData other && Equals(other);
    }
    /// <inheritdoc/>
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
