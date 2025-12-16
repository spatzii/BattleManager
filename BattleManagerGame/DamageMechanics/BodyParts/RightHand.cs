namespace TextBasedGame.DamageMechanics.BodyParts;

public class RightHand: IArms
{
    public string Name => "Right hand";
    public int PainModifier => 15;
    public int Effectiveness { get; set; } = 100;
}