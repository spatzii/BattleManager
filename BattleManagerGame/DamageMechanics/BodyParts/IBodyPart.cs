using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.DamageMechanics.BodyParts;

public interface IBodyPart
{
    string Name { get; }
    
    BodyPartType Type { get; }
    int Effectiveness { get; set; }
    
}