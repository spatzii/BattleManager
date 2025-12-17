namespace TextBasedGame.Equipment.Weapons;

// public IBodyParts Head { get; } = new Head();

public class Sword : IWeapons
{
    public string Name => "Sword";
    public int Damage => 10;
}