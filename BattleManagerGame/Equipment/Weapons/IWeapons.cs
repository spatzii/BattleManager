using TextBasedGame.Equipment.EquipmentStats.WeaponStats;

namespace TextBasedGame.Equipment.Weapons;

public interface IWeapons
{
    public IWeaponStats Stats { get; }
    
}