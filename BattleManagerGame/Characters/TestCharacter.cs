using TextBasedGame.Characters.CharacterState;
using TextBasedGame.Characters.Stats;
using TextBasedGame.DamageMechanics.Body;
using TextBasedGame.Equipment.Weapons;

namespace TextBasedGame.Characters;

public class TestCharacter(string name, IBaseStats baseStats, IWeapons? weapon = null) : ICharacter
{
    public string Name { get; } = name;
    public IBody Body { get; } = new Body();
    public IWeapons? Weapon { get; private set; } = weapon;
    public IBaseStats Stats { get; } = baseStats;
    public ICharacterState State { get; } = new CharacterState.CharacterState();
}