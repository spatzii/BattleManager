namespace TextBasedGame.DamageMechanics.BodyParts;

public interface IBodyParts
{
    string Name { get; }
    int PainModifier { get; }
    int Effectiveness { get; set; }
    
}