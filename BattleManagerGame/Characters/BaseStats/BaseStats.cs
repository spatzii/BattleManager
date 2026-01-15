using System.Collections.Generic;

namespace TextBasedGame.Characters.BaseStats;

public class BaseStats(Dictionary<CharacterStatType, float> baseStats) : IBaseStats
{
    public float Melee => baseStats[CharacterStatType.Melee];
    public float Accuracy => baseStats[CharacterStatType.Accuracy];
    public float Evasion => baseStats[CharacterStatType.Evasion];
    public float Strength => baseStats[CharacterStatType.Strength];
    public float Stamina => baseStats[CharacterStatType.Stamina];
    public float Initative => baseStats[CharacterStatType.Initiative];
    public float StartingHealth => baseStats[CharacterStatType.StartingHealth];

}   