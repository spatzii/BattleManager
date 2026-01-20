using TextBasedGame.Characters;
using TextBasedGame.DamageMechanics.Body;
using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame.Combat;

public class Combat (ICharacter attacker, ICharacter defender)
{
    private readonly ICharacter _attacker = attacker;
    private readonly ICharacter _defender = defender;
    
    // Weapon damage or melee
    private int BaseDamage => (int)(_attacker.Weapon?.Stats.Damage ?? _attacker.Stats.Melee); 
    
    
    public int TestResolveAttackHead()
    {
        var targetBodyPart = _defender.Body.GetPart(BodyPartType.Head);
        ApplyDamage(BaseDamage, targetBodyPart);

        return targetBodyPart.Effectiveness;

    }

    public int ResolveAttack_SpecificPart(IBodyPart targetBodyPart)
    {
        ApplyDamage(BaseDamage, targetBodyPart);
        return targetBodyPart.Effectiveness;
    
    }

    public int ResolveAttack_Random()
    {
        var targetBodyPart = _defender.Body.GetRandomPart();
        ApplyDamage(BaseDamage, targetBodyPart);
        Console.WriteLine(targetBodyPart.Name);
        Console.WriteLine(targetBodyPart.Effectiveness);
        return targetBodyPart.Effectiveness;

    }

    private void ApplyDamage(int dmg, IBodyPart target)
    {
        target.Effectiveness = Math.Max(0, target.Effectiveness - dmg);
        
    }
    
}

