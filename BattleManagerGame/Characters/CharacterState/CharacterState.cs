using TextBasedGame.Characters.BaseStats;
using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.Characters.CharacterState;

public class CharacterState(IBaseStats baseStats) : ICharacterState
{
    private readonly IBaseStats _baseStats = baseStats;
    public float CurrentStamina { get; set; } = baseStats.Stamina;
    public CharacterHealthState HealthState { get; private set; } = CharacterHealthState.Healthy;

    public void ConsumeStamina(float number)
    {
        CurrentStamina = Math.Max(0, CurrentStamina - number);
    }
    public void UpdateHealthState(IBody body)
    {
        // Calculate average effectiveness across all body parts
        var avgEffectiveness = body.Parts.Values
            .Average(part => part.Effectiveness);
        
        HealthState = avgEffectiveness switch
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