// 1. The qualitative states

using TextBasedGame.DamageMechanics.BodyParts;

namespace TextBasedGame;

public enum StrikeQuality
{
    Glancing,
    Solid,
    Heavy,
    Devastating
}

public enum BodyPartHealthState
{
    Uninjured,
    BarelyHit,
    Wounded,
    BadlyWounded,
    CriticallyWounded,
    Mangled,
    Destroyed
}

public enum CharacterHealthState
{
    Healthy,
    Hurt,
    Injured,
    BadlyInjured,
    Critical,
    Dying
}

// 2. Hit processing result
public class HitResult
{
    public StrikeQuality Quality { get; set; }
    public int Damage { get; set; }
    public bool InflictsWound { get; set; }
    public int BleedingRate { get; set; }
    public required string NarrativeText { get; set; }
}

// 3. The profile system
public static class HitProfile
{
    private static StrikeQuality DetermineQuality(float normalizedValue)
    {
        return normalizedValue switch
        {
            < 0.3f => StrikeQuality.Glancing,
            < 0.7f => StrikeQuality.Solid,
            < 1.2f => StrikeQuality.Heavy,
            _ => StrikeQuality.Devastating
        };
    }
    
    public static HitResult ProcessHit(float normalizedValue)
    {
        var quality = DetermineQuality(normalizedValue);
        
        return quality switch
        {
            StrikeQuality.Glancing => new HitResult
            {
                Quality = quality,
                Damage = (int)(normalizedValue * 15), // 0-4 damage
                InflictsWound = false,
                BleedingRate = 0,
                NarrativeText = "A glancing blow"
            },
            
            StrikeQuality.Solid => new HitResult
            {
                Quality = quality,
                Damage = (int)(normalizedValue * 40), // 12-28 damage
                InflictsWound = Random.Shared.NextDouble() < 0.3, // 30% wound chance
                BleedingRate = 1,
                NarrativeText = "A solid hit"
            },
            
            StrikeQuality.Heavy => new HitResult
            {
                Quality = quality,
                Damage = (int)(normalizedValue * 60), // 42-72 damage
                InflictsWound = Random.Shared.NextDouble() < 0.7, // 70% wound chance
                BleedingRate = 3,
                NarrativeText = "A heavy blow"
            },
            
            StrikeQuality.Devastating => new HitResult
            {
                Quality = quality,
                Damage = (int)(normalizedValue * 80), // 96+ damage
                InflictsWound = true, // Always wounds
                BleedingRate = 5,
                NarrativeText = "A devastating strike"
            },
            
            _ => throw new ArgumentException("Invalid hit quality")
        };
    }
}

// 4. Body part state evaluation
public static class BodyPartProfile
{
    public static BodyPartHealthState DetermineState(int effectiveness)
    {
        return effectiveness switch
        {
            >= 95 => BodyPartHealthState.Uninjured,
            >= 80 => BodyPartHealthState.BarelyHit,
            >= 60 => BodyPartHealthState.Wounded,
            >= 40 => BodyPartHealthState.BadlyWounded,
            >= 20 => BodyPartHealthState.CriticallyWounded,
            > 0 => BodyPartHealthState.Mangled,
            _ => BodyPartHealthState.Destroyed
        };
    }
    
    public static string GetDescription(BodyPartHealthState state)
    {
        return state switch
        {
            BodyPartHealthState.Uninjured => "Uninjured",
            BodyPartHealthState.BarelyHit => "Barely Scratched",
            BodyPartHealthState.Wounded => "Wounded",
            BodyPartHealthState.BadlyWounded => "Badly Wounded",
            BodyPartHealthState.CriticallyWounded => "Critically Wounded",
            BodyPartHealthState.Mangled => "Mangled",
            BodyPartHealthState.Destroyed => "Destroyed",
            _ => "Unknown"
        };
    }
}