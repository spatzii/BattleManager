using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.Characters;

public interface ICharacter
{
    public string Name { get; }
    public IBody Body { get; }
}