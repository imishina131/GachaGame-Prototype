using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject]
public class PlayerData
{
    public HashSet<RollData> PlayerRollData = [];
    public Dictionary<string, uint> Pity = new();
}