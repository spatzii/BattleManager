using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame.DamageMechanics.Body;

public class Body : IBody
{
    private readonly Dictionary<BodyPartType, IBodyPart> _parts;
    // Cached array for fast random selection of the defender's *actual* parts.
    private readonly IBodyPart[] _partList;
    public IReadOnlyDictionary<BodyPartType, IBodyPart> Parts => _parts;
    public IBodyPart GetPart(BodyPartType type)
    {
        return _parts[type]; // Or handle missing parts gracefully
    }
    
    public IBodyPart GetRandomPart(Random? rng = null)
    {
        rng ??= Random.Shared;
        return _partList[rng.Next(_partList.Length)];
    }
    
  
    public Body()
    {
        _parts = new Dictionary<BodyPartType, IBodyPart>
        {
            { BodyPartType.Head, new BodyPart("Head", BodyPartType.Head) },
            { BodyPartType.Torso, new BodyPart("Torso", BodyPartType.Torso) },
            { BodyPartType.LeftArm, new BodyPart("Left Arm", BodyPartType.LeftArm) },
            { BodyPartType.RightArm, new BodyPart("Right Arm", BodyPartType.RightArm) },
            { BodyPartType.LeftLeg, new BodyPart("Left Leg", BodyPartType.LeftLeg) },
            { BodyPartType.RightLeg, new BodyPart("Right Leg", BodyPartType.RightLeg) }
        };
        
        _partList = _parts.Values.ToArray();

    }
}