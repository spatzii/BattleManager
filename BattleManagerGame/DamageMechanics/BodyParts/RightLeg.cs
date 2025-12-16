namespace TextBasedGame.DamageMechanics.BodyParts;

public class RightLeg: ILegs
{
    public string Name => "Right leg";
    public int PainModifier => 10;
    public int Effectiveness { get; set; } = 100;
}