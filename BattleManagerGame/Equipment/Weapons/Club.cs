using TextBasedGame.Equipment.EquipmentStats.WeaponStats;

namespace TextBasedGame.Equipment.Weapons;

public class Club(IWeaponStats stats) : IWeapons
{
    public string Name => WeaponStats.Name;
    public int Damage => (int)WeaponStats.Damage;
    public IWeaponStats WeaponStats { get; } = stats;

    public static Club Create()
    {
        var stats = WeaponLoader.LoadStatsFromFile(GamePaths.WEAPON_STATS + "Club.json");
        return new Club(stats);
    }
}