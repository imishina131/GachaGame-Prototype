using GachaGame_Prototype.Azure.Core.Data_Types;

namespace GachaGame.Azure.Core.Interfaces;

public class EmptyRollResolver<T> : IRollResolver<T> where T : struct, IRollData
{

    public T ResolveRoll(List<T> possibleRolls, PlayerData playerData, out PlayerData playerDataAfterRoll)
    {
        playerDataAfterRoll = playerData;
        return default;
    }
}