namespace TextBasedGame.DamageMechanics.BodyParts;

public class Head : IBodyParts
{
    public string Name => "Head";
    public int PainModifier => 30;
    public int Effectiveness { get; set; } = 100;
    

}