using GachaGame_Prototype.Azure.Interfaces;

namespace GachaGame_Prototype.Azure.Core.Data_Types;

public class Character : IRollData
{
    public Guid CharacterID { get; set; }
    public uint Rarity { get; set; }
}