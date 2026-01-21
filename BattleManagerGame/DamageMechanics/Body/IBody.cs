using System;
using System.Collections.Generic;
using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame.DamageMechanics.Body;

public interface IBody
{
    
    IReadOnlyDictionary<BodyPartType, IBodyPart> Parts { get; }
    IBodyPart GetPart(BodyPartType type);
    IBodyPart GetRandomPart(Random? rng = null);
    // IBodyPart GetState(BodyPartType state);

}

public enum BodyPartType
{
    Head,
    Torso,
    LeftArm,
    RightArm,
    RightLeg,
    LeftLeg
}