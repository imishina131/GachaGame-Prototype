using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject]
public class PlayerData
{
    public HashSet<RollData> PlayerRollData { get; init;} = [];
    public Dictionary<string, uint> Pity {get; init;} = new();
}