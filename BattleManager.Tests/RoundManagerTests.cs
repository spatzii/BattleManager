using TextBasedGame;
using TextBasedGame.Characters;
using TextBasedGame.Characters.BaseStats;
using TextBasedGame.Characters.CharacterState;
using TextBasedGame.DamageMechanics.Body;
using TextBasedGame.DamageMechanics.BodyParts;
using TextBasedGame.Equipment.Weapons;

namespace BattleManager.Tests;

public class RoundManagerTests
{
    [Fact]
    public void ExecuteAttack_DefenderIsDead_AttackIsSkipped()
    {
        // Arrange
        var aliveHero = new StubCharacter("Hero", overallCondition: 100f, initiative: 10f);
        var deadEnemy = new StubCharacter("Enemy", overallCondition: 0f, initiative: 5f);

        var roundManager = new RoundManager(aliveHero, deadEnemy);

        // Act & Assert
        // If the attack is properly skipped, GetRandomPart() won't be called on the dead defender's body.
        // The dead character's body throws if GetRandomPart is called, so no exception means success.
        var exception = Record.Exception(() => roundManager.ExecuteAttack());

        Assert.Null(exception);
    }
}

#region Test Stubs

internal class StubCharacter : ICharacter
{
    private readonly float _overallCondition;

    public string Name { get; }
    public IBody Body { get; }
    public IWeapons? Weapon => null;
    public IBaseStats Stats { get; }
    public ICharacterState GameState { get; }

    public StubCharacter(string name, float overallCondition, float initiative)
    {
        Name = name;
        _overallCondition = overallCondition;
        Stats = new StubBaseStats(initiative);
        GameState = new StubCharacterState();
        Body = overallCondition <= 0
            ? new DeadCharacterBody()
            : new StubBody();
    }

    public float GetOverallCondition() => _overallCondition;
}

internal class StubBaseStats : IBaseStats
{
    public float Melee => 50f;
    public float Accuracy => 50f;
    public float Evasion => 50f;
    public float Strength => 50f;
    public float Stamina => 100f;
    public float Initative { get; }
    public float StartingHealth => 20f;

    public StubBaseStats(float initiative)
    {
        Initative = initiative;
    }
}

internal class StubCharacterState : ICharacterState
{
    public CharacterHealthState HealthState => CharacterHealthState.Healthy;
    public float CurrentStamina { get; set; } = 100f;

    public void ConsumeStamina(float number) { }
    public void UpdateHealthState(IBody body) { }
}

internal class StubBody : IBody
{
    private readonly Dictionary<BodyPartType, IBodyPart> _parts;

    public IReadOnlyDictionary<BodyPartType, IBodyPart> Parts => _parts;

    public StubBody()
    {
        _parts = new Dictionary<BodyPartType, IBodyPart>
        {
            { BodyPartType.Head, new StubBodyPart("Head", BodyPartType.Head) },
            { BodyPartType.Torso, new StubBodyPart("Torso", BodyPartType.Torso) },
            { BodyPartType.LeftArm, new StubBodyPart("Left Arm", BodyPartType.LeftArm) },
            { BodyPartType.RightArm, new StubBodyPart("Right Arm", BodyPartType.RightArm) },
            { BodyPartType.LeftLeg, new StubBodyPart("Left Leg", BodyPartType.LeftLeg) },
            { BodyPartType.RightLeg, new StubBodyPart("Right Leg", BodyPartType.RightLeg) }
        };
    }

    public IBodyPart GetPart(BodyPartType type) => _parts[type];
    public IBodyPart GetRandomPart(Random? rng = null) => _parts[BodyPartType.Torso];
}

internal class DeadCharacterBody : IBody
{
    public IReadOnlyDictionary<BodyPartType, IBodyPart> Parts =>
        throw new InvalidOperationException("Should not access body parts of a dead character during attack");

    public IBodyPart GetPart(BodyPartType type) =>
        throw new InvalidOperationException("Should not access body parts of a dead character during attack");

    public IBodyPart GetRandomPart(Random? rng = null) =>
        throw new InvalidOperationException("Should not call GetRandomPart on a dead character - attack should be skipped");
}

internal class StubBodyPart : IBodyPart
{
    public string Name { get; }
    public BodyPartType Type { get; }
    public int Effectiveness { get; set; } = 100;

    public StubBodyPart(string name, BodyPartType type)
    {
        Name = name;
        Type = type;
    }
}

#endregion
