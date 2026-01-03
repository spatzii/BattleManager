using TextBasedGame.Characters.BaseStats;

namespace TextBasedGame.Characters.CharacterState;

public class CharacterState (IBaseStats baseStats): ICharacterState
{
    // private readonly IBaseStats _baseStats = baseStats;
    public float CurrentStamina { get; set; } = baseStats.Stamina;

    public void ConsumeStamina(float number)
    {
        CurrentStamina = Math.Max(0, CurrentStamina - number);
    }
}