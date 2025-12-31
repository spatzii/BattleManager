namespace TextBasedGame.Characters.Stats;

public interface IBaseStats
{
    float Melee { get; }
    float Accuracy { get; }
    float Evasion { get; }
    float Strength { get; }
}

public enum CharacterStatType
{
    Melee,
    Accuracy,
    Evasion,
    Strength,
    Stamina
}