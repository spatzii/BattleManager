using TextBasedGame.Equipment.EquipmentStats.WeaponStats;

namespace TextBasedGame.Equipment.EquipmentStats.WeaponStats;

public class WeaponStats: IWeaponStats
{
    private readonly Dictionary<WeaponStatType, float> _weaponStats;
    public string Name { get; }
    public WeaponStats(string name, Dictionary<WeaponStatType, float> weaponStats)
    {
        Name = name;
        _weaponStats = weaponStats;
    }
    public float Damage => _weaponStats[WeaponStatType.Damage];
    public float MinStrengthRequired => _weaponStats[WeaponStatType.MinStrengthRequired];
}