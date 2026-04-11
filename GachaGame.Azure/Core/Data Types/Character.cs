using GachaGame.Azure.Core.Interfaces;

namespace GachaGame.Azure.Core.DataTypes;

public class Character : IRollData
{
    public Guid CharacterID { get; set; }
    public uint Rarity { get; set; }
}