using System;
using Newtonsoft.Json;

[JsonObject]
public struct RollResultData
{
    public RollResultData(Guid characterID, Guid rarityTierID)
    {
        CharacterID = characterID;
        RarityTierID = rarityTierID;
    }
    public Guid CharacterID { get; init; }
    public Guid RarityTierID { get; init; }
}
