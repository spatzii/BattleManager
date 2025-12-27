using TextBasedGame.Equipment.EquipmentStats.WeaponStats;

namespace TextBasedGame.Equipment.Weapons;

// public IBodyParts Head { get; } = new Head();

public class Sword (IWeaponStats weaponStats) : IWeapons
{
    public string Name => "Sword";
    public int Damage => 10;
    public IWeaponStats WeaponStats { get; } = weaponStats;
}