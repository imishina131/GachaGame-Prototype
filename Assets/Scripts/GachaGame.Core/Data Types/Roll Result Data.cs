using System;
using Newtonsoft.Json;
/// <summary>
/// The data returned from the remote playfab server upon rolling a banner. Deserialized from JSON in the http request
/// </summary>
[JsonObject]
public struct RollData
{
    /// <summary>
    /// The date and time a roll occured
    /// </summary>
    public DateTime RollTime { get; init; }
    /// <summary>
    /// The <see cref="Guid"/> of the character rolled
    /// </summary>
    public Guid Character { get; init; }
    /// <summary>
    /// The id of the rolled banner
    /// </summary>
    public string BannerRolled { get; init; }
    /// <summary>
    /// The <see cref="Guid"/> of the resulting roll's rarity tier
    /// </summary>
    public Guid RarityTier { get; init; }
    /// <summary>
    /// Whether the roll succeeded or failed
    /// </summary>
    public bool Success { get; init; }
}
