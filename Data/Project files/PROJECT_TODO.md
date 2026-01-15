# Project TODO List

## Current Sprint — Complete Core Combat Loop

### Immediate Tasks (This Week)

#### Combat Flow
- [ ] Loop `RoundManager.ExecuteAttack()` until victory/defeat
- [ ] Add round counter to track combat length
- [ ] Define victory condition (one side all dead/incapacitated)
- [ ] End-of-round processing hook for bleeding

#### Bleeding System
- [ ] Track accumulated bleeding per character
- [ ] Apply bleeding damage at end of each round
- [ ] Bleeding narrative ("He's losing blood fast")

#### Hit Resolution Phase 2
- [ ] Integrate Accuracy stat (currently unused)
- [ ] Roll-to-hit before damage calculation
- [ ] Miss/hit narrative text ("The blow goes wide")

---

## Technical Debt (Address Soon)

### Code Fixes
- [ ] Fix typo: `IBaseStats.Initative` → `Initiative`
- [ ] Remove or refactor deprecated `Attack.cs` / `Combat` class
- [ ] Add null checks in JSON loaders
- [ ] TestCharacter should accept `IBody` parameter for testability

### Testability Improvements
- [ ] Inject `Random` into `HitProfile.ProcessHit()` instead of using `Random.Shared`
- [ ] Inject `Random` into `DamageCalculator.GetVariance()`
- [ ] Create mock `IBody` for unit tests

---

## Phase 1 Completion Checklist

### Combat Resolution
- [x] DamageCalculator with weapon access curve
- [x] Normalization to 0-1.5 scale
- [x] HitProfile processing
- [x] Body part damage application
- [x] Character health state updates
- [ ] Accuracy/hit chance integration
- [ ] Miss mechanics

### Survival Mechanics
- [ ] Bleeding accumulation per round
- [ ] Bleeding damage application
- [ ] Incapacitation threshold (below X% overall)
- [ ] Death threshold (0% on vital part or 0% overall)
- [ ] Vital vs non-vital body parts

### Turn System
- [x] Initiative-based turn order
- [x] Both characters attack per round
- [ ] Multi-round loop until combat ends
- [ ] End-of-round processing

### Output
- [x] Combat narrative text
- [x] Debug output (CombatDebugger)
- [ ] Round summary
- [ ] Victory/defeat announcement

**Phase 1 Deliverable**: Two characters fight multiple rounds to death with full narrative output.

---

## Unit Testing Setup

### Infrastructure
- [ ] Create `BattleManagerGame.Tests` project
- [ ] Add reference to main project
- [ ] Choose test framework (xUnit recommended)
- [ ] Create `CLAUDE.md` for Claude Code context

### Priority Tests
- [ ] `DamageCalculator` weapon access curve edge cases
- [ ] `DamageCalculator` normalization bounds
- [ ] `HitProfile` threshold boundaries (0.3, 0.7, 1.2)
- [ ] `BodyPartProfile` threshold boundaries
- [ ] `CharacterState.UpdateHealthState()` transitions

### Test Helpers Needed
- [ ] Test character builder with configurable stats
- [ ] Mock `IBody` with settable effectiveness
- [ ] Deterministic random wrapper

---

## Design Decisions Pending

### Hit Chance
**Question**: Should Accuracy affect hit chance or hit quality?
- Option A: Roll Accuracy vs Evasion for hit/miss, then calculate damage
- Option B: Current system where Evasion just reduces final damage
- **Leaning**: Option A feels more tactical

### Bleeding Scale
**Question**: How should bleeding damage work?
- Option A: Fixed damage per bleeding rate (rate 3 = 3 damage/round)
- Option B: Percentage of max health per rate
- Option C: Accelerating (bleeding gets worse each round if untreated)

### Critical Hits
**Question**: When can crits occur?
- Option A: Random chance on any hit
- Option B: Only on Devastating hits (1.2+ normalized)
- Option C: Only on aimed shots that succeed
- **Concern**: Random crits could make minor hits lethal

---

## Future Features (Not This Sprint)

### Phase 2 — Stat Architecture
- StatModifier class with source tracking
- Modifier types (Flat, PercentAdd, PercentMult)
- Trait system with JSON definitions
- Wound system with healing/scarring

### Phase 3 — Combat Integration
- Event system for cross-system communication
- Morale system
- Relationship effects on combat

### Phase 4 — Unity Integration
- Hex grid implementation
- Character visualization
- Combat UI

---

## Quick Reference

### File Locations
```
Characters:     Assets/Data/Characters/*.json
Weapons:        Assets/Data/Weapons/*.json
Entry point:    Program.cs
Combat flow:    RoundManager.cs → CombatResolver.cs → DamageCalculator.cs
Profiles:       Profiles.cs (all enums and translation)
Debug:          CombatDebugger.cs
```

### Key Methods
```csharp
// Start combat
var round = new RoundManager(hero, enemy);
round.ExecuteAttack();

// Manual attack
attacker.ResolveAttackAgainst(defender, targetPart, showDebug: true);

// Calculate damage only
var result = new DamageCalculator(attacker, defender).Calculate();

// Process hit quality
var hitResult = HitProfile.ProcessHit(result.NormalizedValue);

// Check body part state
var state = BodyPartProfile.DetermineState(part.Effectiveness);
```

### Current Character Stats
**Bowie (Hero)**:
- Melee: 2, Accuracy: 70, Evasion: 6
- Strength: 4, Stamina: 100, Initiative: 7
- Weapon: Sword (Damage: 10, MinStr: 4)

**Ogre (Enemy)**:
- Melee: 10, Accuracy: 60, Evasion: 5
- Strength: 6, Stamina: 80, Initiative: 3
- Weapon: Club (Damage: 8, MinStr: 3)

---

## Notes

**Architecture Philosophy**: Text-first, stats-heavy internally, narrative-driven externally.

**Key Principle**: A stat-heavy game must not feel like a number-cruncher to the player.

**Testing Philosophy**: Heavy reliance on statistics means we need deterministic tests with controlled randomness.

Last Updated: January 2025
