using System;
using System.Linq;
using TextBasedGame.Characters.BaseStats;
using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.Characters.CharacterState;

public class CharacterState(IBaseStats baseStats) : ICharacterState
{
    private readonly IBaseStats _baseStats = baseStats;
    public float CurrentStamina { get; set; } = baseStats.Stamina;
    public float CurrentHealth { get; set; }
    public CharacterHealthState HealthState { get; private set; } = CharacterHealthState.Healthy;

    public void ConsumeStamina(float number)
    {
        CurrentStamina = Math.Max(0, CurrentStamina - number);
    }
    public void UpdateHealthState(IBody body)
    {
        // Step 1: Calculate average effectiveness across all body parts
        var avgEffectiveness = body.Parts.Values
            .Average(part => part.Effectiveness); // todo: this should be tied to @GetOverallCondition in ICharacter

        // Step 2: Set CurrentHealth (couples to effectiveness)
        CurrentHealth = (float)(avgEffectiveness / 100) * _baseStats.StartingHealth;

        // Step 3: Derive HealthState from CurrentHealth percentage
        var healthPercent = (CurrentHealth / _baseStats.StartingHealth) * 100;

        HealthState = healthPercent switch
        {
            >= 90 => CharacterHealthState.Healthy,
            >= 70 => CharacterHealthState.Hurt,
            >= 50 => CharacterHealthState.Injured,
            >= 30 => CharacterHealthState.BadlyInjured,
            >= 10 => CharacterHealthState.Critical,
            _ => CharacterHealthState.Dying
        };
    }
}