namespace TextBasedGame.Characters.BaseStats;

public interface IBaseStats
{
    float Melee { get; }
    float Accuracy { get; }
    float Evasion { get; }
    float Strength { get; }
    float Stamina { get; }
}

public enum CharacterStatType
{
    Melee,
    Accuracy,
    Evasion,
    Strength,
    Stamina
}