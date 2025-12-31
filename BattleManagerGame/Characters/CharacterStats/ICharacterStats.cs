namespace TextBasedGame.Characters.Stats;

public interface ICharacterStats
{
    float Melee { get; }
    float Accuracy { get; }
    float Armor { get; }
    float Strength { get; }
}

public enum CharacterStatType
{
    Melee,
    Accuracy,
    Armor,
    Strength
}