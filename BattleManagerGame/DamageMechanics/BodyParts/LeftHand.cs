namespace TextBasedGame.DamageMechanics.BodyParts;

public class LeftHand: IArms
{
    public string Name => "Left hand";
    public int PainModifier => 15;
    public int Effectiveness { get; set; } = 100;
}