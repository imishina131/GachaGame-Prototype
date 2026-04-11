using GachaGame.Azure.Core.Interfaces;
using JetBrains.Annotations;

namespace GachaGame.Azure.Core.DataTypes;
[UsedImplicitly]
public class CharacterRollResolver: IRollResolver<Character>
{
    public Character ResolveRoll(List<Character>? possibleRolls)
    {
        throw new NotImplementedException();
    }
}