using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject(MemberSerialization.OptIn)]
public class PlayerData
{
    public HashSet<RollData> PlayerRollData = [];
    [JsonIgnore]
    public Dictionary<Character, uint> CharacterInventory = new();
    [JsonProperty]
    List<KeyValuePair<Character, uint>> SerializedCharacterInventory
    {
        get => CharacterInventory.ToList();
        set { CharacterInventory = value.ToDictionary(x => x.Key, x => x.Value); }
    }
    [JsonIgnore]
    public Dictionary<Guid, uint> CurrencyInventory = new();
    [JsonProperty]
    List<KeyValuePair<Guid, uint>> SerializedCurrencyInventory
    {
        get => CurrencyInventory.ToList();
        set { CurrencyInventory = value.ToDictionary(x => x.Key, x => x.Value); }
    }
}
[JsonObject]
public struct RollData(DateTime rollTime, Banner bannerRolled, Character character)
{
    public DateTime RollTime { get; init; } = rollTime;
    public Character Character { get; init; }= character;
    public Banner BannerRolled { get; init; } = bannerRolled;
}