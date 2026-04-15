using System;
using Newtonsoft.Json;

[JsonObject]
public struct RollResultData
{
    [JsonConstructor]
    public RollResultData(DateTime rollTime, string bannerRolled, Guid characterID, Guid rarityTierID, bool success)
    {
        CharacterIDHex = characterID.ToString("N").ToUpper();
        RarityTierIDHex = rarityTierID.ToString("N").ToUpper();
        RollTime = rollTime;
        BannerRolled = bannerRolled;
        Success = success;
    }
    DateTime RollTime{ get; init;}
    
    [JsonProperty("CharacterID")]
    string CharacterIDHex { get; init; }

    string BannerRolled { get; init; }
    [JsonProperty("RarityTierID")]
    string RarityTierIDHex { get; init; }
    bool Success { get; init; }
    
    [JsonIgnore]
    public SerializableGuid CharacterID => SerializableGuid.FromHexString(CharacterIDHex);

    [JsonIgnore]
    public SerializableGuid RarityTierID => SerializableGuid.FromHexString(RarityTierIDHex);
}
