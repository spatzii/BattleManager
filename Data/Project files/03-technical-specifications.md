# Technical Specifications

This document contains code patterns, class structures, and technical decisions reflecting the current implementation.

---

## Folder Structure (Current)

```
BattleManagerGame/
├── Assets/
│   └── Data/
│       ├── Characters/
│       │   ├── TestDefaultHero.json
│       │   └── Ogre.json
│       └── Weapons/
│           ├── BasicSword.json
│           └── Club.json
├── Characters/
│   ├── BaseStats/
│   │   ├── IBaseStats.cs          # Interface + CharacterStatType enum
│   │   ├── BaseStats.cs           # Dictionary-based implementation
│   │   └── CharacterData.cs       # JSON structure + CharacterLoader
│   ├── CharacterState/
│   │   ├── ICharacterState.cs     # Interface
│   │   └── CharacterState.cs      # Implementation with stamina, health state
│   ├── ICharacter.cs              # Core character interface
│   └── TestCharacter.cs           # Simple implementation for testing
├── DamageMechanics/
│   ├── Body/
│   │   ├── IBody.cs               # Interface + BodyPartType enum
│   │   └── Body.cs                # Humanoid body implementation
│   └── BodyParts/
│       ├── IBodyPart.cs           # Interface
│       └── BodyPart.cs            # Implementation
├── Equipment/
│   ├── EquipmentStats/
│   │   └── WeaponStats/
│   │       ├── IWeaponStats.cs    # Interface + WeaponStatType enum
│   │       ├── WeaponStats.cs     # Dictionary-based implementation
│   │       └── WeaponData.cs      # JSON structure + WeaponLoader
│   └── Weapons/
│       ├── IWeapons.cs            # Interface
│       ├── Sword.cs               # Sword with Create() factory
│       └── Club.cs                # Club with Create() factory
├── Attack.cs                      # Legacy Combat class (deprecated)
├── CombatDebugger.cs              # Debug output for combat pipeline
├── CombatResolver.cs              # Main combat resolution logic
├── Constants.cs                   # GamePaths static class
├── DamageCalculator.cs            # Attack value calculation
├── Profiles.cs                    # All enums and profile classes
├── RoundManager.cs                # Turn flow management
└── Program.cs                     # Entry point
```

---

## Core Interfaces

### ICharacter
```csharp
public interface ICharacter
{
    string Name { get; }
    IBody Body { get; }
    IWeapons? Weapon { get; }
    IBaseStats Stats { get; }
    ICharacterState GameState { get; }
    float GetOverallCondition();
}
```

### IBaseStats
```csharp
public interface IBaseStats
{
    float Melee { get; }
    float Accuracy { get; }
    float Evasion { get; }
    float Strength { get; }
    float Stamina { get; }
    float Initative { get; }  // Note: typo in current code
}

public enum CharacterStatType
{
    Melee, Accuracy, Evasion, Strength, Stamina, Initiative
}
```

### ICharacterState
```csharp
public interface ICharacterState
{
    CharacterHealthState HealthState { get; }
    float CurrentStamina { get; set; }
    void ConsumeStamina(float number);
    void UpdateHealthState(IBody body);
}
```

### IBody & IBodyPart
```csharp
public interface IBody
{
    IReadOnlyDictionary<BodyPartType, IBodyPart> Parts { get; }
    IBodyPart GetPart(BodyPartType type);
    IBodyPart GetRandomPart(Random? rng = null);
}

public enum BodyPartType
{
    Head, Torso, LeftArm, RightArm, RightLeg, LeftLeg
}

public interface IBodyPart
{
    string Name { get; }
    BodyPartType Type { get; }
    int Effectiveness { get; set; }
}
```

### IWeapons & IWeaponStats
```csharp
public interface IWeapons
{
    IWeaponStats Stats { get; }
}

public interface IWeaponStats
{
    string Name { get; }
    float Damage { get; }
    float MinStrengthRequired { get; }
}

public enum WeaponStatType
{
    Damage, MinStrengthRequired
}
```

---

## JSON Data Formats

### Character JSON
```json
{
  "characterType": "Hero",
  "displayName": "Bowie",
  "baseStats": {
    "melee": 2,
    "accuracy": 70,
    "evasion": 6,
    "strength": 4,
    "stamina": 100,
    "initiative": 7
  }
}
```

### Weapon JSON
```json
{
  "weaponType": "Sword",
  "displayName": "Sword",
  "baseStats": {
    "damage": 10,
    "minStrengthRequired": 4
  }
}
```

---

## Damage Calculation System

### Pipeline Overview
```
┌─────────────────────────────────────────────────────────────────────────┐
│ PHASE 1: ATTACK CALCULATION                                             │
│                                                                         │
│   Weapon Damage ──► Weapon Access % ──► Accessible Damage ──► Variance  │
│                          ▲                                      │       │
│                    Strength / MinReq                             ▼       │
│                    (exponential decay)                    Final Attack  │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│ PHASE 2: HIT RESOLUTION                                                 │
│                                                                         │
│   Final Attack - Evasion = Net Advantage ──► Normalize ──► StrikeQuality│
│                                              (0 to 1.5)                 │
│                                                   │                     │
│                                                   ▼                     │
│                                              HitResult                  │
│                                        (Damage, Bleeding, etc)          │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│ PHASE 3: BODY EFFECT                                                    │
│                                                                         │
│   HitResult.Damage ──► BodyPart.Effectiveness ──► CharacterState.Health │
│                              (reduce)                  (update)         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Weapon Access Formula
```csharp
var strengthRatio = strength / Math.Max(minRequirement, 1f);
var accessPercent = 1f - (float)Math.Exp(-strengthRatio);
return Math.Clamp(accessPercent, 0.2f, 1.0f);
```

**Curve Characteristics**:
- At 50% of requirement: ~39% access (clamped to 20%)
- At exact requirement: ~63% access
- At 150% of requirement: ~78% access
- At 200% of requirement: ~86% access
- Never reaches 100% (asymptotic)
- Never drops below 20% (floor clamp)

### Normalization
Maps net advantage to 0-1.5 scale:
```csharp
private const float ExpectedMinResult = -20f;
private const float ExpectedMaxResult = 20f;

var normalized = (netAdvantage - ExpectedMinResult) / (ExpectedMaxResult - ExpectedMinResult);
return Math.Clamp(normalized, 0f, 1.5f);
```

**Quality Bands**:
- 0.0-0.3: Glancing
- 0.3-0.7: Solid
- 0.7-1.2: Heavy
- 1.2+: Devastating

---

## Profile System

### StrikeQuality Enum
```csharp
public enum StrikeQuality
{
    Glancing,    // 0.0-0.3 normalized
    Solid,       // 0.3-0.7 normalized
    Heavy,       // 0.7-1.2 normalized
    Devastating  // 1.2+ normalized
}
```

### HitResult Class
```csharp
public class HitResult
{
    public StrikeQuality Quality { get; set; }
    public int Damage { get; set; }
    public bool InflictsWound { get; set; }
    public int BleedingRate { get; set; }
    public required string NarrativeText { get; set; }
}
```

### HitProfile Processing
| Quality | Damage Formula | Wound Chance | Bleeding Rate |
|---------|----------------|--------------|---------------|
| Glancing | normalized × 15 | 0% | 0 |
| Solid | normalized × 40 | 30% | 1 |
| Heavy | normalized × 60 | 70% | 3 |
| Devastating | normalized × 80 | 100% | 5 |

### BodyPartHealthState Enum
```csharp
public enum BodyPartHealthState
{
    Uninjured,          // 95-100
    BarelyHit,          // 80-94
    Wounded,            // 60-79
    BadlyWounded,       // 40-59
    CriticallyWounded,  // 20-39
    Mangled,            // 1-19
    Destroyed           // 0
}
```

### CharacterHealthState Enum
```csharp
public enum CharacterHealthState
{
    Healthy,      // avg >= 90
    Hurt,         // avg >= 70
    Injured,      // avg >= 50
    BadlyInjured, // avg >= 30
    Critical,     // avg >= 10
    Dying         // avg < 10
}
```

---

## Combat Resolution Flow

### CombatResolver.ResolveAttackAgainst()
```csharp
public static void ResolveAttackAgainst(
    this ICharacter attacker, 
    ICharacter defender, 
    IBodyPart targetPart, 
    bool showDebug = false)
{
    // 1. Capture "before" state for debug
    // 2. Phase 1: Calculate hit quality via DamageCalculator
    // 3. Phase 2: Process hit through HitProfile
    // 4. Phase 3: Apply damage to body part
    // 5. Phase 4: Update character overall health state
    // 6. Phase 5: Debug output (if enabled)
    // 7. Phase 6: Narrative output (what player sees)
}
```

### RoundManager.ExecuteAttack()
```csharp
public void ExecuteAttack()
{
    var attacker = Players().Attacker;  // Higher initiative
    var defender = Players().Defender;
    
    if (IsCharacterAlive(defender))
        attacker.ResolveAttackAgainst(defender, defender.Body.GetRandomPart());
    
    if (IsCharacterAlive(attacker))
        defender.ResolveAttackAgainst(attacker, attacker.Body.GetRandomPart());
}
```

---

## Debug System

### CombatDebugger Output Format
```
╔══════════════════════════════════════════════════════════════════╗
║  COMBAT DEBUG: Bowie → Ogre (Left Arm)                           ║
╠══════════════════════════════════════════════════════════════════╣
║  ATTACK CALCULATION                                              ║
║  ├─ Weapon: Sword (base 10)                                      ║
║  ├─ Strength: 4 / Required: 4 → Access: 63.2%                    ║
║  ├─ Accessible Damage: 6.32 × Variance(1.05) = 6.64              ║
║  └─ vs Evasion: 5 → Net Advantage: +1.64                         ║
╠══════════════════════════════════════════════════════════════════╣
║  HIT RESOLUTION                                                  ║
║  ├─ Normalized: 0.54 (range -20 to +20 → 0 to 1.5)              ║
║  ├─ Quality: SOLID (0.3–0.7)                                     ║
║  ├─ Damage Roll: 21 (0.54 × 40)                                  ║
║  └─ Bleeding: 1 (Light)                                          ║
╠══════════════════════════════════════════════════════════════════╣
║  BODY EFFECT                                                     ║
║  ├─ Left Arm: 100 → 79 (−21)                                     ║
║  ├─ Part State: Uninjured → Wounded                              ║
║  └─ Overall: Healthy → Hurt (avg: 96.5%)                         ║
╚══════════════════════════════════════════════════════════════════╝
```

---

## Class Relationships

```
ICharacter ◄─────────────────────────────────────────┐
    │                                                │
    ├──► IBody ◄──────────────────────┐              │
    │       │                         │              │
    │       └──► IBodyPart (many)     │              │
    │                                 │              │
    ├──► IWeapons? ──► IWeaponStats   │              │
    │                                 │              │
    ├──► IBaseStats                   │              │
    │                                 │              │
    └──► ICharacterState ─────────────┘              │
              │         (uses body for health calc)  │
              │                                      │
              └──► CharacterHealthState (enum)       │
                                                     │
TestCharacter ───────────────────────────────────────┘
```

---

## Design Patterns Used

### Factory Pattern
Weapons use static `Create()` methods:
```csharp
public static Sword Create()
{
    var stats = WeaponLoader.LoadStatsFromFile(GamePaths.WEAPON_STATS + "BasicSword.json");
    return new Sword(stats);
}
```

### Extension Method Pattern
Combat resolution as extension on ICharacter:
```csharp
public static void ResolveAttackAgainst(this ICharacter attacker, ...)
```

### Dictionary-Based Stats
Both characters and weapons use `Dictionary<EnumType, float>` for flexible stat storage loaded from JSON.

### Profile/Translator Pattern
Static classes (`HitProfile`, `BodyPartProfile`) translate internal values to display states.

---

## Known Issues / Technical Debt

1. **Typo**: `IBaseStats.Initative` should be `Initiative`
2. **Legacy code**: `Attack.cs` contains old `Combat` class, now superseded by `CombatResolver`
3. **Body hardcoded**: `TestCharacter` creates `new Body()` directly; should accept `IBody` via constructor
4. **No error handling**: JSON loaders assume files exist and are valid
5. **Random coupling**: `HitProfile.ProcessHit()` uses `Random.Shared` directly; should inject for testing

---

## Dependencies

- .NET 10.0
- System.Text.Json (JSON serialization)
- System.Text.Json.Serialization (JsonStringEnumConverter)

No external NuGet packages.
