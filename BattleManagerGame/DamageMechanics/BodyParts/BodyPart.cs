using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.DamageMechanics.BodyParts;

public class BodyPart(string name, BodyPartType type) : IBodyPart
{
    public string Name { get; } = name;
    public BodyPartType Type { get; } = type;
    public int Effectiveness { get; set; } = 100;
}