using TextBasedGame.Characters.Stats;

namespace TextBasedGame.Characters.CharacterStats;

public class BaseStats : IBaseStats
{
    private readonly Dictionary<CharacterStatType, float> _baseStats;
    
    public BaseStats(Dictionary<CharacterStatType, float> baseStats)
    {
        _baseStats = baseStats;
    }

    public float Melee => _baseStats[CharacterStatType.Melee];
    public float Accuracy => _baseStats[CharacterStatType.Accuracy];
    public float Evasion => _baseStats[CharacterStatType.Evasion];
    public float Strength => _baseStats[CharacterStatType.Strength];
    public float Stamina => _baseStats[CharacterStatType.Stamina];

}