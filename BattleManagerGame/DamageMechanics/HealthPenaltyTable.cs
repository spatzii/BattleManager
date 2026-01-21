using TextBasedGame.DamageMechanics.Body;

namespace TextBasedGame.DamageMechanics;

public static class HealthPenaltyTable
{
    private static readonly Dictionary<BodyPartHealthState, float> BasePenalties = new()
    {
        { BodyPartHealthState.Uninjured, 0 },
        { BodyPartHealthState.BarelyHit, 5 },
        { BodyPartHealthState.Wounded, 20 },
        { BodyPartHealthState.BadlyWounded, 30 },
        { BodyPartHealthState.CriticallyWounded, 35 },
        { BodyPartHealthState.Mangled, 40 },
        { BodyPartHealthState.Destroyed, 47 }
    };

    private static readonly Dictionary<BodyPartType, float> Multipliers = new()
    {
        { BodyPartType.Head, 2f },
        { BodyPartType.Torso, 1.5f },
        { BodyPartType.LeftArm, 1f },
        { BodyPartType.RightArm, 1f },
        { BodyPartType.LeftLeg, 1f },
        { BodyPartType.RightLeg, 1f }
    };

    public static float GetPenalty(BodyPartType type, BodyPartHealthState state)
    {
        return BasePenalties[state] * Multipliers[type];
    }
}