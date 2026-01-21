using TextBasedGame.Characters;
using TextBasedGame.DamageMechanics;
using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame.Combat;

public static class CombatResolver
{
    // This will act as an interface between DamageCalculator and RoundManager. RM will call DC, DC returns a DC
    // object with damage information. CombatResolver will take this damage and apply it to limbs. In the future it
    // will probably be responsible for applying bleeding and other effects also.
    

    public static void ResolveAttackAgainst(
        this ICharacter attacker, 
        ICharacter defender, 
        IBodyPart targetPart, 
        bool showDebug = false,
        bool printDebug = false)
    
    {
        
        // Create debugger only if needed
        var debugger = (showDebug || printDebug) ? new CombatDebugger() : null;
        debugger?.LogToFile = printDebug;

        // Capture "before" state
        var effectivenessBefore = targetPart.Effectiveness;
        var partStateBefore = BodyPartProfile.DetermineState(targetPart.Effectiveness);
        var healthStateBefore = defender.GameState.HealthState;
    
        // Phase 1: Calculate hit quality
        var damageResult = new DamageCalculator(attacker, defender).Calculate();
        debugger?.RecordAttackPhase(attacker, defender, damageResult);
    
        // Phase 2: Process hit through profile system
        var hitResult = HitProfile.ProcessHit(damageResult.NormalizedValue);
        debugger?.RecordHitResolution(hitResult);
    
        // Phase 3: Apply damage to body part
        ApplyDamage(hitResult.Damage, targetPart);
    
        // Phase 4: Update character overall health state
        defender.GameState.UpdateHealthState(defender.Body);
    
        // Phase 5: Record body effect and print debug
        var health = defender.GameState.CurrentHealth;
        
        debugger?.RecordBodyEffect(
            targetPart,
            effectivenessBefore,
            partStateBefore,
            healthStateBefore,
            defender.GameState.HealthState,
            health);
    
        debugger?.Print();
    
        // Phase 6: Output narrative (what player sees)
        DisplayCombatResult(attacker, defender, targetPart, hitResult);
    }
    
    private static void ApplyDamage(int damage, IBodyPart target)
    {
        target.Effectiveness = Math.Max(0, target.Effectiveness - damage);
    }
    
    private static void DisplayCombatResult(ICharacter attacker, ICharacter defender, 
        IBodyPart targetPart, HitResult hitResult)
    {
        var bodyPartState = BodyPartProfile.DetermineState(targetPart.Effectiveness);
        var bodyPartDesc = BodyPartProfile.GetDescription(bodyPartState);
        
        Console.WriteLine($"{attacker.Name} attacks {defender.Name}'s {targetPart.Name}!");
        Console.WriteLine($"{hitResult.NarrativeText}. The {targetPart.Name} is now: {bodyPartDesc}");
        Console.WriteLine($"{defender.Name} overall condition: {defender.GameState.HealthState}");
        
        if (hitResult.BleedingRate > 0)
        {
            Console.WriteLine($"Bleeding: {GetBleedingDescription(hitResult.BleedingRate)}");
        }
    }
    
    private static string GetBleedingDescription(int bleedingRate)
    {
        return bleedingRate switch
        {
            1 => "Light bleeding",
            < 4 => "Moderate bleeding",      // 2-3
            < 6 => "Heavy bleeding",         // 4-5
            _ => "Critical bleeding"         // 6+
        };
    }
}