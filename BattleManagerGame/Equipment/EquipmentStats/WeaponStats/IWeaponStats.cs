namespace TextBasedGame.Equipment.EquipmentStats.WeaponStats;

public interface IWeaponStats
{
    string Name { get; }
    float Damage { get; }
}

public enum WeaponStatType
{
    Damage
}