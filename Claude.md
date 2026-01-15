# CLAUDE.md — Project Context for Claude Code

## Project Overview

This is a tactical squad management RPG inspired by Battle Brothers, RimWorld, and Football Manager. It's a stat-heavy game where complex numerical systems drive emergent storytelling, but **players never see raw numbers** — only qualitative descriptors like "Wounded" or "Devastating blow".

**Current Phase**: Phase 1 — Core Combat Loop (Phase 0 complete)  
**Tech Stack**: C# (.NET 10.0) console application, System.Text.Json, no external packages  
**Architecture**: Text-based core logic first, Unity visualization layer added later

---

## Key Architectural Principles

### 1. Obfuscated Numbers
All internal calculations use precise floats/ints, but player-facing output uses qualitative descriptors. The `Profiles.cs` file contains translation logic via `HitProfile` and `BodyPartProfile` static classes.

### 2. Interface-First Design
Every major system has an interface (`ICharacter`, `IBody`, `IWeapons`, `IBaseStats`, `ICharacterState`). This enables dependency injection and testability.

### 3. Static vs Dynamic Separation
- `IBaseStats` — Loaded from JSON, never changes (character blueprint)
- `ICharacterState` — Changes during gameplay (stamina, health state)
- `IBody` — Direct component of ICharacter (not nested in state), contains mutable `Effectiveness` per part

### 4. Data-Driven Design
All game definitions live in JSON files under `Assets/Data/`. Characters and weapons use `Dictionary<EnumType, float>` for flexible stat storage.

---

## Solution Structure

```
BattleManagerGame/           # Main game project
├── Characters/              # ICharacter, IBaseStats, ICharacterState
├── DamageMechanics/         # IBody, IBodyPart, Body
├── Equipment/               # IWeapons, IWeaponStats, Sword, Club
├── Assets/Data/             # JSON definitions
├── CombatResolver.cs        # Main combat pipeline (extension method)
├── DamageCalculator.cs      # Attack value calculation
├── Profiles.cs              # Enums and translation (HitProfile, BodyPartProfile)
├── RoundManager.cs          # Turn flow
└── CombatDebugger.cs        # Debug output

BattleManagerGame.Tests/     # Test project (to be created)
```

---

## Core Combat Pipeline

```
DamageCalculator.Calculate()     → DamageResult (NormalizedValue 0-1.5)
HitProfile.ProcessHit()          → HitResult (Quality, Damage, BleedingRate)
CombatResolver.ApplyDamage()     → BodyPart.Effectiveness reduced
CharacterState.UpdateHealthState() → CharacterHealthState enum updated
```

Entry point for attacks: `attacker.ResolveAttackAgainst(defender, targetPart, showDebug)`

---

## Important Enums

**StrikeQuality**: Glancing (0-0.3), Solid (0.3-0.7), Heavy (0.7-1.2), Devastating (1.2+)

**BodyPartHealthState**: Uninjured (95+), BarelyHit (80+), Wounded (60+), BadlyWounded (40+), CriticallyWounded (20+), Mangled (1+), Destroyed (0)

**CharacterHealthState**: Healthy (90+), Hurt (70+), Injured (50+), BadlyInjured (30+), Critical (10+), Dying (<10)

---

## Testing Guidelines

### What to Test
- `DamageCalculator`: Weapon access curve (exponential decay), normalization bounds (0-1.5)
- `HitProfile`: Threshold boundaries at 0.3, 0.7, 1.2
- `BodyPartProfile`: Threshold boundaries at 95, 80, 60, 40, 20, 1, 0
- `CharacterState.UpdateHealthState()`: State transitions based on average effectiveness

### Testing Challenges
- `HitProfile.ProcessHit()` uses `Random.Shared` for wound chance — needs injection for determinism
- `DamageCalculator.GetVariance()` uses `Random.Shared` — needs injection
- `Body` is hardcoded in `TestCharacter` — should accept `IBody` parameter

### Test Patterns to Use
- Inject `Random` with fixed seed for deterministic tests
- Create mock `IBody` with preset effectiveness values
- Use builder pattern for test characters with specific stat configurations
- Test boundary conditions (exactly at thresholds, just above, just below)

### Test Naming Convention
`MethodName_Scenario_ExpectedResult`
Example: `CalculateWeaponAccess_StrengthEqualsRequirement_ReturnsApprox63Percent`

---

## Known Technical Debt

1. **Typo**: `IBaseStats.Initative` should be `Initiative` (fix requires updating interface, implementation, and JSON files)
2. **Deprecated**: `Attack.cs` contains old `Combat` class — superseded by `CombatResolver`
3. **Hardcoded**: `TestCharacter` creates `new Body()` directly — should accept `IBody` via constructor
4. **No error handling**: JSON loaders assume files exist
5. **Untestable randomness**: `HitProfile` and `DamageCalculator` use `Random.Shared` directly

---

## File Reference

| Purpose | Location |
|---------|----------|
| Character interface | `Characters/ICharacter.cs` |
| Base stats interface | `Characters/BaseStats/IBaseStats.cs` |
| Character state | `Characters/CharacterState/CharacterState.cs` |
| Body system | `DamageMechanics/Body/Body.cs` |
| Combat resolution | `CombatResolver.cs` |
| Damage calculation | `DamageCalculator.cs` |
| All enums & profiles | `Profiles.cs` |
| Turn management | `RoundManager.cs` |
| Debug output | `CombatDebugger.cs` |
| JSON characters | `Assets/Data/Characters/*.json` |
| JSON weapons | `Assets/Data/Weapons/*.json` |

---

## When Writing Tests

1. **Use xUnit** as the test framework
2. **Mirror the folder structure** of the main project in the test project
3. **One test class per production class** (e.g., `DamageCalculatorTests.cs`)
4. **Arrange-Act-Assert pattern** for test structure
5. **Test the boundaries** — this game relies heavily on threshold-based state transitions
6. **Don't test console output** — test the underlying calculations and state changes

---

## Current Priorities

1. Complete combat loop (multiple rounds until death)
2. Implement bleeding mechanics
3. Set up unit test project with priority tests for calculation classes
4. Fix testability issues (Random injection)

---

## Documentation Files

For deeper context, read these in order:
1. `01-game-vision-and-architecture.md` — Design philosophy and core concepts
2. `02-development-roadmap.md` — Phase breakdown and completion status
3. `03-technical-specifications.md` — Code patterns and class details
4. `04-progress-tracker.md` — Current status and decisions log
5. `PROJECT_TODO.md` — Immediate tasks and priorities