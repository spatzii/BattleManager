namespace TextBasedGame.DamageMechanics.BodyParts;

public class LeftLeg: ILegs
{
    public string Name => "Left leg";
    public int PainModifier => 10;
    public int Effectiveness { get; set; } = 100;
}