namespace TextBasedGame.Characters.Stats;

public interface ICharacterStats
{
    float Melee { get; }
    float Accuracy { get; }
}

public enum CharacterStatType
{
    Melee,
    Accuracy
}