using System;
using System.IO;
using TextBasedGame.Characters;
using TextBasedGame.Combat;
using TextBasedGame.DamageMechanics;
using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame;

public class CombatDebugger
{
    // Phase 1: Attack calculation data
    private string _attackerName;
    private string _defenderName;
    private string _weaponName;
    private float _baseDamage;
    private float _strength;
    private float _minStrengthRequired;
    private float _accessPercent;
    private float _accessibleDamage;
    private float _variance;
    private float _finalAttackValue;
    private float _evasion;
    private float _netAdvantage;
    
    // Phase 2: Hit resolution data
    private float _normalizedValue;
    private StrikeQuality _strikeQuality;
    private int _hitDamage;
    private int _bleedingRate;
    
    // Phase 3: Body effect data
    private string _bodyPartName;
    private int _effectivenessBefore;
    private int _effectivenessAfter;
    private BodyPartHealthState _partStateBefore;
    private BodyPartHealthState _partStateAfter;
    private CharacterHealthState _healthStateBefore;
    private CharacterHealthState _healthStateAfter;
    private float _health;

    private static readonly string Timestamp = DateTime.Now.ToString("yyyy_MM_dd HHmmss");
    private static readonly string FileName = $"combat_log {Timestamp}.txt";
    public bool LogToFile { get; set; } = false;
    private readonly string _logFilePath = 
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", FileName);

    public void RecordAttackPhase(
        ICharacter attacker, 
        ICharacter defender, 
        DamageResult result)
    {
        _attackerName = attacker.Name;
        _defenderName = defender.Name;
        _weaponName = attacker.Weapon?.Stats.Name ?? "Unarmed";
        _baseDamage = attacker.Weapon?.Stats.Damage ?? attacker.Stats.Melee;
        _strength = attacker.Stats.Strength;
        _minStrengthRequired = attacker.Weapon?.Stats.MinStrengthRequired ?? 0f;
        _evasion = defender.Stats.Evasion;
        
        _finalAttackValue = result.RawAttackValue;
        _netAdvantage = result.NetAdvantage;
        _normalizedValue = result.NormalizedValue;
        
        // Recalculate intermediates for display
        // (or we expand DamageResult to carry these)
        _accessPercent = CalculateAccess(_strength, _minStrengthRequired);
        _accessibleDamage = _baseDamage * _accessPercent;
        _variance = _finalAttackValue / _accessibleDamage;
    }

    public void RecordHitResolution(HitResult hitResult)
    {
        _strikeQuality = hitResult.Quality;
        _hitDamage = hitResult.Damage;
        _bleedingRate = hitResult.BleedingRate;
    }

    public void RecordBodyEffect(
        IBodyPart targetPart,
        int effectivenessBefore,
        BodyPartHealthState partStateBefore,
        CharacterHealthState healthStateBefore,
        CharacterHealthState healthStateAfter,
        float health)
    {
        _bodyPartName = targetPart.Name;
        _effectivenessBefore = effectivenessBefore;
        _effectivenessAfter = targetPart.Effectiveness;
        _partStateBefore = partStateBefore;
        _partStateAfter = BodyPartProfile.DetermineState(targetPart.Effectiveness);
        _healthStateBefore = healthStateBefore;
        _healthStateAfter = healthStateAfter;
        _health = health;
    }

    public void Print()
    {
        Console.WriteLine($"╔══════════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"║  COMBAT DEBUG: {_attackerName} → {_defenderName} ({_bodyPartName})".PadRight(67) + "║");
        Console.WriteLine($"╠══════════════════════════════════════════════════════════════════╣");
        
        PrintAttackSection();
        PrintHitResolutionSection();
        PrintBodyEffectSection();
        
        Console.WriteLine($"╚══════════════════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }
    
    private void WriteLine(string text = "")
    {
        Console.WriteLine(text);

        if (!LogToFile) return;
        try
        {
            File.AppendAllText(_logFilePath, text + Environment.NewLine);
        }
        catch (Exception ex)
        {
            // Fail silently or print once to console to avoid infinite loops/spam
        }
    }

    private void PrintAttackSection()
    {
        WriteLine($"║  ATTACK CALCULATION".PadRight(67) + "║");
        WriteLine($"║  ├─ Weapon: {_weaponName} (base {_baseDamage})".PadRight(67) + "║");
        WriteLine($"║  ├─ Strength: {_strength} / Required: {_minStrengthRequired} → Access: {_accessPercent:P1}".PadRight(67) + "║");
        WriteLine($"║  ├─ Accessible Damage: {_accessibleDamage:F2} × Variance({_variance:F2}) = {_finalAttackValue:F2}".PadRight(67) + "║");
        WriteLine($"║  └─ vs Evasion: {_evasion} → Net Advantage: {_netAdvantage:+0.00;-0.00}".PadRight(67) + "║");
        WriteLine($"╠══════════════════════════════════════════════════════════════════╣");
    }

    private void PrintHitResolutionSection()
    {
        var qualityBand = GetQualityBand(_strikeQuality);
        WriteLine($"║  HIT RESOLUTION".PadRight(67) + "║");
        WriteLine($"║  ├─ Normalized: {_normalizedValue:F2} (range -20 to +20 → 0 to 1.5)".PadRight(67) + "║");
        WriteLine($"║  ├─ Quality: {_strikeQuality.ToString().ToUpper()} ({qualityBand})".PadRight(67) + "║");
        WriteLine($"║  ├─ Damage Roll: {_hitDamage} ({_normalizedValue:F2} × {GetDamageMultiplier(_strikeQuality)})".PadRight(67) + "║");
        WriteLine($"║  └─ Bleeding: {_bleedingRate} ({GetBleedingLabel(_bleedingRate)})".PadRight(67) + "║");
        WriteLine($"╠══════════════════════════════════════════════════════════════════╣");
    }

    private void PrintBodyEffectSection()
    {
        WriteLine($"║  BODY EFFECT".PadRight(67) + "║");
        WriteLine($"║  ├─ {_bodyPartName}: {_effectivenessBefore} → {_effectivenessAfter} (−{_effectivenessBefore - _effectivenessAfter})".PadRight(67) + "║");
        WriteLine($"║  ├─ Part State: {_partStateBefore} → {_partStateAfter}".PadRight(67) + "║");
        WriteLine($"║  └─ Overall: {_healthStateBefore} → {_healthStateAfter} (HP: {_health:F1})".PadRight(67) + "║");
    }

    private float CalculateAccess(float strength, float minReq)
    {
        var ratio = strength / Math.Max(minReq, 1f);
        var access = 1f - (float)Math.Exp(-ratio);
        return Math.Clamp(access, 0.2f, 1.0f);
    }

    private string GetQualityBand(StrikeQuality quality) => quality switch
    {
        StrikeQuality.Glancing => "0.0–0.3",
        StrikeQuality.Solid => "0.3–0.7",
        StrikeQuality.Heavy => "0.7–1.2",
        StrikeQuality.Devastating => "1.2+",
        _ => "?"
    };

    private int GetDamageMultiplier(StrikeQuality quality) => quality switch
    {
        StrikeQuality.Glancing => 15,
        StrikeQuality.Solid => 40,
        StrikeQuality.Heavy => 60,
        StrikeQuality.Devastating => 80,
        _ => 0
    };

    private string GetBleedingLabel(int rate) => rate switch
    {
        0 => "None",
        1 => "Light",
        < 4 => "Moderate",
        < 6 => "Heavy",
        _ => "Critical"
    };
}