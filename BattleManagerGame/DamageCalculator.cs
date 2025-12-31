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
        var rawDefenseValue = CalculateDefenceValue();
        var netAdvantage = rawAttackValue - rawDefenseValue;
        var normalizedValue = NormalizeToScale(netAdvantage);

        return new DamageResult
        {
            RawAttackValue = rawAttackValue,
            RawDefenseValue = rawDefenseValue,
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
        var armorValue = _defender.Stats.Armor;
        return armorValue;
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
