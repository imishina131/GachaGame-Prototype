using GachaGame.Azure.Core.DataTypes;
using Microsoft.Extensions.Logging;
namespace GachaGame.Azure.Core.Interfaces;

/// <summary>
/// Represents a resolver for a <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type of object the resolver will handle</typeparam>
public interface IRollResolver<T> where T : struct, IRollData
{
    /// <summary>
    /// Resolves a roll for a <typeparamref name="T"/> through taking in a multiple <see cref="IRollData"/> and generating a single result
    /// </summary>
    /// <param name="possibleRolls">Each possible roll result</param>
    /// <param name="rollContext">The <see cref="RollContext"/> with data for the current roll</param>
    /// <returns>The resulting roll</returns>
    public T ResolveRoll(List<T> possibleRolls, RollContext rollContext, ILogger logger);
}

//public class RollResolver: IRollResolver<RollData>
//{
//    random _random = new random();

//    public RollData ResolveRoll(List<RollData> possibleRolls, playerData playerData)
//    {
//List<RollData> tier5 = new List<RollData>();
//List<RollData> otherTiers = new List<RollData>();

//Guid fiveStarId = Guid.Empty;

//foreach (RollData roll in possibleRolls)
//{
//    if(roll.Rarity==fiveStarId)
//    {
//    tier5.add(roll);
//    }
//else
//{
//otherTiers.add(roll);
//}
//}

//int rollValue = _random.range(1,101);

//if(rollValue<=5)
//{
//    int win5050 = _random.range(0,2);

//    if(win5050==1)
//    {
//        return tier5[0];
//    }

//    else
//    {
//int randomIndex = _random.range(0, tier5.Count)
//return tier5[random];
//    }
//}

//int otherIndex
// =_random.range(0, otherTiers.Count);
//return otherTiers[otherIndex
//];
//    }
//}