using TextBasedGame.Characters;
using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame;

public class Attack
{
    public void AttackBodyPart(IBodyPart bodyPart, int damage)
    {
        // Math.Max(0...) clamps the effectiveness to zero. Remove if we implement spillover
        bodyPart.Effectiveness = Math.Max(0, bodyPart.Effectiveness - damage);
        Console.WriteLine($"{bodyPart.Name} took {damage} damage, and it has" +
                          $" {bodyPart.Effectiveness} effectiveness left.");
    }
}

public class CombatResolver
{
    public int ResolveAttack(ICharacter attacker, ICharacter defender)
    {
        var damage = attacker.Weapon.Damage;
        var targetBodyPart = defender.Body.Head;
        ApplyDamage(damage, targetBodyPart);

        return targetBodyPart.Effectiveness;

    }

    private void ApplyDamage(int dmg, IBodyPart target)
    {
        target.Effectiveness = Math.Max(0, target.Effectiveness - dmg);
        
    }
    
}

