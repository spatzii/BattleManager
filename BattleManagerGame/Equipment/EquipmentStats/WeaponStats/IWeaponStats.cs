namespace TextBasedGame.Equipment.EquipmentStats.WeaponStats;

public interface IWeaponStats
{
    float damage { get; }
}

public enum WeaponStatType
{
    Damage
}