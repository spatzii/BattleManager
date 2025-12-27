using TextBasedGame.Equipment.EquipmentStats.WeaponStats;

namespace TextBasedGame.Equipment.EquipmentStats.WeaponStats;

public class WeaponStats: IWeaponStats
{
    private readonly Dictionary<WeaponStatType, float> _weaponStats;
    
    public WeaponStats(Dictionary<WeaponStatType, float> weaponStats)
    {
        _weaponStats = weaponStats;
    }

    public float damage => _weaponStats[WeaponStatType.Damage];
}