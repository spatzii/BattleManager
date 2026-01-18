using System.Collections.Generic;
using System.Linq;
using TextBasedGame.Characters.BaseStats;
using TextBasedGame.Characters.CharacterState;
using TextBasedGame.DamageMechanics.Body;
using TextBasedGame.Equipment.Weapons;

namespace TextBasedGame.Characters;

public class TestCharacter(string name, IBaseStats baseStats, IWeapons? weapon = null) : ICharacter
{
    public string Name { get; } = name;
    public IBody Body { get; } = new Body();
    public IWeapons? Weapon { get; private set; } = weapon;
    public IBaseStats Stats { get; } = baseStats;
    public ICharacterState GameState { get; } = new CharacterState.CharacterState(baseStats);

    public float GetOverallCondition()
    {
        List<int> effectivenessList = [];
        effectivenessList.AddRange(Body.Parts.Values.Select(part => part.Effectiveness));
        float totalEffectiveness = effectivenessList.Sum();
        return (totalEffectiveness / effectivenessList.Count);
    }
}