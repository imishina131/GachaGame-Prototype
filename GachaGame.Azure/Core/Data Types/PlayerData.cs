using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject]
public class PlayerData
{
    public PlayerData()
    {
        PlayerRollData = [];
        CharacterInventory = new();
        CurrencyInventory = new();
    }
    [JsonConstructor]
    PlayerData(HashSet<RollData> playerRollData, Dictionary<Character, uint> characterInventory, Dictionary<Guid, uint> currencyInventory)
    {
        PlayerRollData = playerRollData;
        CharacterInventory = characterInventory;
        CurrencyInventory = currencyInventory;
    }
    public HashSet<RollData> PlayerRollData;
    public Dictionary<Character, uint> CharacterInventory;
    public Dictionary<Guid, uint> CurrencyInventory;
}
[JsonObject]
public struct RollData(DateTime rollTime, Banner bannerRolled, Character character)
{
    public DateTime RollTime { get; init; } = rollTime;
    public Character Character { get; init; }= character;
    public Banner BannerRolled { get; init; } = bannerRolled;
}