namespace TextBasedGame.DamageMechanics.BodyParts;

public class Torso : IBodyParts
{
    public string Name => "Chest";
    public int PainModifier => 20;
    public int Effectiveness { get; set; } = 100;

}  