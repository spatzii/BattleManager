using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame.DamageMechanics.Body;

public interface IBody
{
    public IBodyParts Head { get; }
    public IBodyParts Torso { get; }
    public IBodyParts LeftHand { get; }
    public IBodyParts RightHand { get; }
    public IBodyParts RightLeg { get; }
    public IBodyParts LeftLeg { get; }
    
}