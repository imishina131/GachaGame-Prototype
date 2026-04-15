using System;
using Newtonsoft.Json;

[JsonObject]
public struct RollData
{
    public DateTime RollTime { get; init; }
    public Guid Character { get; init; }
    public string BannerRolled { get; init; }
    public Guid RarityTier { get; init; }
    public bool Success { get; init; }
}
