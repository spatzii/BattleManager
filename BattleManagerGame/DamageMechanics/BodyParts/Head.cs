namespace TextBasedGame.DamageMechanics.BodyParts;

public class Head : IBodyPart
{
    public string Name => "Head";
    public int PainModifier => 30;
    public int Effectiveness { get; set; } = 100;
    

}