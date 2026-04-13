using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;

namespace GachaGame.Azure.Core.DataTypes;
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public readonly record struct Character : IRollData
{
    public Guid CharacterID { get; }
    public uint Rarity { get; }
}