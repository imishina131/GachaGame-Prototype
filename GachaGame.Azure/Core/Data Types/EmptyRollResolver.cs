using GachaGame.Azure.Core.Interfaces;
namespace GachaGame.Azure.Core.DataTypes;

public class EmptyRollResolver<T> : IRollResolver<T> where T : struct, IRollData
{
    public T ResolveRoll(List<T> possibleRolls, PlayerData playerData) => default;
}