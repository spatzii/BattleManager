using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame.DamageMechanics.Body;

public class Body : IBody
{
    public IBodyPart Head { get; } = new Head();
    public IBodyPart Torso { get; } = new Torso();
    public IBodyPart LeftHand { get; } = new LeftHand();
    public IBodyPart RightHand { get; } = new RightHand();
    public IBodyPart LeftLeg { get; } = new LeftLeg();
    public IBodyPart RightLeg { get; } = new RightLeg();
    
}