using TextBasedGame.DamageMechanics;
using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.Characters.CharacterState;

public interface ICharacterState
{
    CharacterHealthState HealthState { get; }
    float CurrentStamina { get; set; }
    float CurrentHealth { get; set; }
    void ConsumeStamina(float number);
    void UpdateHealthState(IBody body);
}