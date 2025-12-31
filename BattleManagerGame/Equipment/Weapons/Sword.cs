using TextBasedGame.Equipment.EquipmentStats.WeaponStats;

namespace TextBasedGame.Equipment.Weapons;

public class Sword(IWeaponStats stats) : IWeapons
{
    public string Name => Stats.Name;
    public int Damage => (int)Stats.Damage;
    public IWeaponStats Stats { get; } = stats;

    public static Sword Create()
    {
        var stats = WeaponLoader.LoadStatsFromFile(GamePaths.WEAPON_STATS + "BasicSword.json");
        return new Sword(stats);
    }
}
