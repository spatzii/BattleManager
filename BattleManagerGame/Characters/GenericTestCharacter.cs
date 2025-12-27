using TextBasedGame.Characters.Stats;
using TextBasedGame.DamageMechanics.Body;
using TextBasedGame.Equipment.Weapons;

namespace TextBasedGame.Characters;

public class GenericTestCharacter(string name, ICharacterStats characterStats, IWeapons? weapon = null) : ICharacter
{
    public string Name { get; } = name;
    private readonly IBody _body = new Body();
    public IBody Body => _body;
    public IWeapons? Weapon { get; private set; } = weapon;
    public ICharacterStats CharacterStats { get; } = characterStats;
}