using TextBasedGame.Characters;

namespace TextBasedGame;

public class DamageCalculator(ICharacter attacker, ICharacter defender)
{
    private readonly ICharacter _attacker = attacker;
    private readonly ICharacter _defender = defender;

    // Configuration for normalization - tune these as you add complexity
    private const float ExpectedMinResult = -20f;  // Weak attack vs heavy armor
    private const float ExpectedMaxResult = 20f;   // Strong attack vs no armor
    private const float NeutralPoint = 0f;          // Evenly matched
    
     


    public DamageResult Calculate()
    {
        var rawAttackValue = CalculateAttackValue();
        var netAdvantage = rawAttackValue - _defender.Stats.Evasion;
        var normalizedValue = NormalizeToScale(netAdvantage);

        return new DamageResult
        {
            RawAttackValue = rawAttackValue,
            RawDefenseValue = _defender.Stats.Evasion,
            NetAdvantage = netAdvantage,
            NormalizedValue = normalizedValue
        };
    }

    private float CalculateAttackValue()
    {
        var weaponDamage = _attacker.Weapon?.Stats.Damage ?? _attacker.Stats.Melee;
        var strength = _attacker.Stats.Strength;
        var weaponAccessPercent = CalculateWeaponAccess(strength);
        var accessibleDamage = weaponDamage * weaponAccessPercent;
        var variance = GetVariance();
        var finalAttackValue = accessibleDamage * variance;
        
        return finalAttackValue;
    }

    private float CalculateWeaponAccess(float strength)
    {
        var strengthRequirement = _attacker.Weapon.Stats.MinStrengthRequired;
        // todo: min requirement check
        var strengthRatio = strength / strengthRequirement;
        var accessPercent = 1f - (float)Math.Exp(-strengthRatio);
        return Math.Clamp(accessPercent, 0.2f, 1.0f);
    }

    private float CalculateDefenceValue()
    {
        var evasionValue = _defender.Stats.Evasion;
        return evasionValue;
    }

    private float GetVariance()
    {
        // ±10% variance
        // 0.9 to 1.1 multiplier
        return 0.9f + ((float)Random.Shared.NextDouble() * 0.2f);
    }
    
    // NORMALIZATION - Converts complex math to 0-1 scale
    private float NormalizeToScale(float netAdvantage)
    {
        // Map expected range to 0-1 scale
        // EXPECTED_MIN_RESULT (-20) → 0.0 (glancing)
        // NEUTRAL_POINT (0) → 0.5 (solid)
        // EXPECTED_MAX_RESULT (20) → 1.0 (devastating)
        
        const float range = ExpectedMaxResult - ExpectedMinResult;
        var normalized = (netAdvantage - ExpectedMinResult) / range;
        
        // Allow values slightly outside 0-1 for truly extreme cases
        // but cap them so they don't break hit profile lookup
        return Math.Clamp(normalized, 0f, 1.5f);
    }
    
    
    public DamageResult CalculateWithDebug()
{
    Console.WriteLine("════════════════════════════════════════════════════════════════");
    Console.WriteLine("DAMAGE CALCULATION DEBUG");
    Console.WriteLine("════════════════════════════════════════════════════════════════");
    Console.WriteLine();

    // ATTACKER SECTION
    Console.WriteLine($"ATTACKER: {_attacker.Name}");
    Console.WriteLine("────────────────────────────────────────────────────────────────");
    
    var weaponDamage = _attacker.Weapon?.Stats.Damage ?? _attacker.Stats.Melee;
    var weaponName = _attacker.Weapon?.Stats.Name ?? "Unarmed";
    var strength = _attacker.Stats.Strength;
    var minStrengthReq = _attacker.Weapon?.Stats.MinStrengthRequired ?? 0f;
    
    Console.WriteLine($"  Weapon: {weaponName} (Base Damage: {weaponDamage:F1})");
    Console.WriteLine($"  Character Stats:");
    Console.WriteLine($"    - Strength: {strength:F1}");
    Console.WriteLine($"    - Melee: {_attacker.Stats.Melee:F1}");
    Console.WriteLine();
    
    Console.WriteLine($"  Weapon Requirements:");
    Console.WriteLine($"    - Minimum Strength Required: {minStrengthReq:F1}");
    bool canUseWeapon = strength >= minStrengthReq;
    Console.WriteLine($"    - Can Use Weapon: {(canUseWeapon ? "YES ✓" : "NO ❌")}");
    Console.WriteLine();
    
    // Attack calculation with intermediate values
    Console.WriteLine($"  Attack Calculation:");
    
    var strengthRatio = strength / minStrengthReq;
    var accessPercentRaw = 1f - (float)Math.Exp(-strengthRatio);
    var weaponAccessPercent = Math.Clamp(accessPercentRaw, 0.2f, 1.0f);
    
    Console.WriteLine($"    - Strength Ratio: {strengthRatio:F2} ({strength:F1} / {minStrengthReq:F1})");
    Console.WriteLine($"    - Weapon Access (raw): {accessPercentRaw * 100:F2}%");
    Console.WriteLine($"    - Weapon Access (final): {weaponAccessPercent * 100:F2}% (clamped 20% - 100%)");
    
    var accessibleDamage = weaponDamage * weaponAccessPercent;
    var variance = GetVariance();
    var finalAttackValue = accessibleDamage * variance;
    
    Console.WriteLine($"    - Accessible Damage: {accessibleDamage:F2} ({weaponDamage:F1} × {weaponAccessPercent:F2})");
    Console.WriteLine($"    - Variance Roll: {variance:F2} (±10% RNG)");
    Console.WriteLine($"    - Final Attack Value: {finalAttackValue:F2}");
    Console.WriteLine();

    // DEFENDER SECTION
    Console.WriteLine($"DEFENDER: {_defender.Name}");
    Console.WriteLine("────────────────────────────────────────────────────────────────");
    
    var evasionValue = _defender.Stats.Evasion;
    Console.WriteLine($"  Evasion Value: {evasionValue:F1}");
    Console.WriteLine();
    Console.WriteLine($"  Defense Calculation:");
    Console.WriteLine($"    - Final Defense Value: {evasionValue:F1}");
    Console.WriteLine();

    // RESULT SECTION
    var netAdvantage = finalAttackValue - evasionValue;
    
    Console.WriteLine("RESULT");
    Console.WriteLine("────────────────────────────────────────────────────────────────");
    Console.WriteLine($"  Raw Attack Value: {finalAttackValue:F2}");
    Console.WriteLine($"  Raw Defense Value: {evasionValue:F2}");
    Console.WriteLine($"  Net Advantage: {netAdvantage:F2} (attack - defense)");
    Console.WriteLine();
    
    Console.WriteLine($"  Normalization:");
    Console.WriteLine($"    - Expected Range: {ExpectedMinResult:F1} to {ExpectedMaxResult:F1}");
    Console.WriteLine($"    - Neutral Point: {NeutralPoint:F1}");
    
    const float range = ExpectedMaxResult - ExpectedMinResult;
    var normalizedRaw = (netAdvantage - ExpectedMinResult) / range;
    var normalizedValue = Math.Clamp(normalizedRaw, 0f, 1.5f);
    
    Console.WriteLine($"    - Normalized Value (raw): {normalizedRaw:F2} (mapped to 0-1 scale)");
    Console.WriteLine($"    - Normalized Value (clamped): {normalizedValue:F2} (within 0.0 to 1.5)");
    Console.WriteLine();
    
    // Determine hit quality
    string hitQuality = normalizedValue switch
    {
        < 0.3f => "GLANCING ⚔️",
        < 0.7f => "SOLID ⚔️⚔️",
        < 1.2f => "HEAVY ⚔️⚔️⚔️",
        _ => "DEVASTATING ⚔️⚔️⚔️⚔️"
    };
    Console.WriteLine($"  Hit Quality: {hitQuality}");
    Console.WriteLine();
    
    Console.WriteLine("════════════════════════════════════════════════════════════════");
    Console.WriteLine();

    return new DamageResult
    {
        RawAttackValue = finalAttackValue,
        RawDefenseValue = evasionValue,
        NetAdvantage = netAdvantage,
        NormalizedValue = normalizedValue
    };
}
    
    
}

public class DamageResult
{
    public float RawAttackValue { get; set; }
    public float RawDefenseValue { get; set; }
    public float NetAdvantage { get; set; }
    public float NormalizedValue { get; set; }
    
    public override string ToString()
    {
        return $"Attack: {RawAttackValue:F2} | Defense: {RawDefenseValue:F2} | " +
               $"Advantage: {NetAdvantage:F2} | Normalized: {NormalizedValue:F2}";
    }
    
}
