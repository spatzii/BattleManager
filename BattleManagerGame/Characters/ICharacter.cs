using TextBasedGame.Characters.CharacterState;
using TextBasedGame.Characters.Stats;
using TextBasedGame.DamageMechanics.Body;
using TextBasedGame.Equipment.Weapons;

namespace TextBasedGame.Characters;

public interface ICharacter
{
    public string Name { get; }
    public IBody Body { get; }
    public IWeapons? Weapon {get;}
    public IBaseStats Stats { get; }
    public ICharacterState State { get; }
    
    
}