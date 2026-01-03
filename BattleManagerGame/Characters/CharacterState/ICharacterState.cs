namespace TextBasedGame.Characters.CharacterState;

public interface ICharacterState
{
    public float CurrentStamina { get; set; }
    public void ConsumeStamina(float number);
}