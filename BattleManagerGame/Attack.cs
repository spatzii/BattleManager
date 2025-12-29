using System.Runtime.InteropServices.Swift;
using TextBasedGame.Characters;
using TextBasedGame.DamageMechanics.Body;
using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame;

public class CombatResolver
{
    public int Test_ResolveAttack_Head(ICharacter attacker, ICharacter defender)
    {
        var damage = (int)attacker.Weapon.WeaponStats.Damage;
        var targetBodyPart = defender.Body.GetPart(BodyPartType.Head);
        ApplyDamage(damage, targetBodyPart);

        return targetBodyPart.Effectiveness;

    }

    public int ResolveAttack_SpecificPart(ICharacter attacker, ICharacter defender, IBodyPart targetBodyPart)
    {
        var damage = (int)attacker.Weapon.WeaponStats.Damage;
        ApplyDamage(damage, targetBodyPart);
        return targetBodyPart.Effectiveness;
    
    }

    public int ResolveAttack_Random(ICharacter attacker, ICharacter defender)
    {
        var damage = (int)attacker.Weapon.WeaponStats.Damage;
        var targetBodyPart = defender.Body.GetRandomPart();
        ApplyDamage(damage, targetBodyPart);
        Console.WriteLine(targetBodyPart.Name);
        Console.WriteLine(targetBodyPart.Effectiveness);
        return targetBodyPart.Effectiveness;

    }

    private void ApplyDamage(int dmg, IBodyPart target)
    {
        target.Effectiveness = Math.Max(0, target.Effectiveness - dmg);
        
    }

    
    
}

