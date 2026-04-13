/// <summary>
/// Represents a result of a random rollable value with a given rarity
/// </summary>
public interface IRollData
{
    /// <summary>
    /// The rarity of the roll
    /// </summary>
    public int Weight { get; }
}
