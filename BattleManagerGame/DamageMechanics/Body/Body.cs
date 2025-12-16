using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame.DamageMechanics.Body;

public class Body : IBody
{
    public IBodyParts Head { get; } = new Head();
    public IBodyParts Torso { get; } = new Torso();
    public IBodyParts LeftHand { get; } = new LeftHand();
    public IBodyParts RightHand { get; } = new RightHand();
    public IBodyParts LeftLeg { get; } = new LeftLeg();
    public IBodyParts RightLeg { get; } = new RightLeg();
    
}