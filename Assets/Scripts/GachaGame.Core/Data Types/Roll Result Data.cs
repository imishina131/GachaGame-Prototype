using System;
using Newtonsoft.Json;

[JsonObject]
public struct RollResultData
{
    [JsonConstructor]
    public RollResultData(Guid characterID, Guid rarityTierID)
    {
        CharacterIDHex = characterID.ToString("N").ToUpper();
        RarityTierIDHex = rarityTierID.ToString("N").ToUpper();
    }

    [JsonProperty("CharacterID")]
    private string CharacterIDHex { get; init; }

    [JsonProperty("RarityTierID")]
    private string RarityTierIDHex { get; init; }

    [JsonIgnore]
    public SerializableGuid CharacterID => SerializableGuid.FromHexString(CharacterIDHex);

    [JsonIgnore]
    public SerializableGuid RarityTierID => SerializableGuid.FromHexString(RarityTierIDHex);
}
