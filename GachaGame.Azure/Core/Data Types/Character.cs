using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct Character : IRollData
{
    public Guid CharacterID { get; set; }
    public uint Rarity { get; set; }
}