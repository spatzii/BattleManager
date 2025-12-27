using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.DamageMechanics.BodyParts;

public class BodyPart : IBodyPart
{
    public string Name { get; }
    public BodyPartType Type { get; }
    public int Effectiveness { get; set; }
    
    public BodyPart(string name, BodyPartType type)
    {
        Name = name;
        Type = type;
        Effectiveness = 100;
    }
    
}