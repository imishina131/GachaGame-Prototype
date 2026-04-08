/// <summary>
/// Represents a result of <typeparamref name="T"/> rolled with a given <see cref="Rarity"/>
/// </summary>
/// <typeparam name="T">The type of the result of the roll</typeparam>
public struct RollData<T>
{
    /// <summary>
    /// The rarity of the roll
    /// </summary>
    public uint Rarity { get; }
    /// <summary>
    /// The result of the roll
    /// </summary>
    public T Result { get; }
    
    public RollData(uint rarity, T result)
    {
        Rarity = rarity;
        Result = result;
    }
}
