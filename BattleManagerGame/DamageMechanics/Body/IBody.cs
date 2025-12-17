using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame.DamageMechanics.Body;

public interface IBody
{
    public IBodyPart Head { get; }
    public IBodyPart Torso { get; }
    public IBodyPart LeftHand { get; }
    public IBodyPart RightHand { get; }
    public IBodyPart RightLeg { get; }
    public IBodyPart LeftLeg { get; }
    
}