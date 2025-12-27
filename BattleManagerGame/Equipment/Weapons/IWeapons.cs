using TextBasedGame.Equipment.EquipmentStats.WeaponStats;

namespace TextBasedGame.Equipment.Weapons;

public interface IWeapons
{
    public string Name { get; }
    public int Damage { get; }
    public IWeaponStats WeaponStats { get; }
    
}