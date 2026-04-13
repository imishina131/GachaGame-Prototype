using GachaGame.Azure.Core.DataTypes;
using Newtonsoft.Json;

namespace GachaGame_Prototype.Azure.Core.Data_Types;
[JsonObject]
public class PlayerData
{
    public HashSet<RollData> PlayerRollData = [];
    public Dictionary<Character, uint> CharacterInventory = new();
    public Dictionary<Guid, uint> CurrencyInventory = new();
}

public readonly record struct RollData(DateTime RollTime, Banner BannerRolled, Character Character)
{
    public readonly DateTime RollTime = RollTime;
    public readonly Character Character = Character;
    public readonly Banner BannerRolled = BannerRolled;
}