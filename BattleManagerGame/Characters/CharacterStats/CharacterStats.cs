using TextBasedGame.Characters.Stats;

namespace TextBasedGame.Characters.CharacterStats;

public class CharacterStats : ICharacterStats
{
    private readonly Dictionary<CharacterStatType, float> _baseStats;
    
    public CharacterStats(Dictionary<CharacterStatType, float> baseStats)
    {
        _baseStats = baseStats;
    }

    public float Melee => _baseStats[CharacterStatType.Melee];
    public float Accuracy => _baseStats[CharacterStatType.Accuracy];
    public float Armor => _baseStats[CharacterStatType.Armor];
    public float Strength => _baseStats[CharacterStatType.Strength];

}