using Newtonsoft.Json;
namespace GachaGame.Azure.Core.DataTypes;

[JsonObject]
public struct RollResultData(Guid characterID, Guid rarityTierID)
{
    public Guid CharacterID { get; init; } = characterID;
    public Guid RarityTierID { get; init; } = rarityTierID;
}