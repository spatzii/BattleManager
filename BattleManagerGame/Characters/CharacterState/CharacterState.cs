using System;
using System.Linq;
using TextBasedGame.Characters.BaseStats;
using TextBasedGame.DamageMechanics;
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
        
        // Step 1

        // foreach (var part in body.Parts)
        // {
        //     HealthPenaltyTable.GetPenalty(part.Value.Type, BodyPartProfile.DetermineState(part.Value.Effectiveness));
        // }
        var totalPenalty = body.Parts.Values.Sum(part => 
            HealthPenaltyTable.GetPenalty(part.Type, BodyPartProfile.DetermineState(part.Effectiveness)));

        // Step 2
        var healthPercent = 100 - totalPenalty;
        
        // Step 3: Set CurrentHealth (couples to effectiveness)
        CurrentHealth = (healthPercent/ 100) * _baseStats.StartingHealth;
        
        // Step 4: ???
        HealthState = healthPercent switch // todo: does this belong here? Feels like it should be in Profiles.cs
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