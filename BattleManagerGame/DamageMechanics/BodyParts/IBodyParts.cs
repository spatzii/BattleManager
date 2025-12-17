namespace TextBasedGame.DamageMechanics.BodyParts;

public interface IBodyPart
{
    string Name { get; }
    int PainModifier { get; }
    int Effectiveness { get; set; }
    
}