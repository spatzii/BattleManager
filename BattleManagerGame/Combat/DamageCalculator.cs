using TextBasedGame.Characters;

namespace TextBasedGame.Combat;

public class DamageCalculator(ICharacter attacker, ICharacter defender)
{
    // Configuration for normalization - tune these as you add complexity
    private const float ExpectedMinResult = -20f;  // Weak attack vs heavy armor
    private const float ExpectedMaxResult = 20f;   // Strong attack vs no armor
    private const float NeutralPoint = 0f;          // Evenly matched // todo: ask Claude why this?

    public DamageResult Calculate(bool showDebug = false)
    {
        var weaponStats = attacker.Weapon?.Stats;
        var weaponDamage = weaponStats?.Damage ?? attacker.Stats.Melee;
        var minStrengthReq = weaponStats?.MinStrengthRequired ?? 0f;
        var strength = attacker.Stats.Strength;

        // 1. Calculate Weapon Access
        var access = CalculateWeaponAccess(strength, minStrengthReq);
        
        // 2. Calculate Final Attack Value (Base * Access * Variance)
        var variance = GetVariance();
        var accessibleDamage = weaponDamage * access;
        var finalAttackValue = accessibleDamage * variance;

        // 3. Calculate Defense and Advantage
        var evasionValue = defender.Stats.Evasion;
        var netAdvantage = finalAttackValue - evasionValue;
        
        // 4. Normalize Result
        var normalizedValue = NormalizeToScale(netAdvantage);

        var result = new DamageResult
        {
            RawAttackValue = finalAttackValue, // Weapon damage times access through strength
            RawDefenseValue = evasionValue,    // Just evasion atm 
            NetAdvantage = netAdvantage,       // Raw attack value minus the defense/evasion
            NormalizedValue = normalizedValue  // Net advantage that goes through 0-1 normalization
        };
        
        return result;
    }

    private float CalculateWeaponAccess(float strength, float minRequirement)
    {
        // todo: min requirement check
        var strengthRatio = strength / Math.Max(minRequirement, 1f);
        var accessPercent = 1f - (float)Math.Exp(-strengthRatio);
        return Math.Clamp(accessPercent, 0.2f, 1.0f);
    }

    private float GetVariance()
    {
        // ±10% variance (0.9 to 1.1 multiplier)
        return 0.9f + ((float)Random.Shared.NextDouble() * 0.2f);
    }

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