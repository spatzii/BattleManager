# Progress Tracker

Update this document as you complete tasks and make decisions.

---

## Current Status

**Current Phase**: Phase 1 — Core Combat Loop  
**Last Completed**: Phase 0 — Text-Based Foundation  
**Last Updated**: January 2025

---

## Completed Work

### Phase 0: Text-Based Foundation ✅ COMPLETE

#### 0.1 Project Setup ✅
- [x] C# console project created (.NET 10.0)
- [x] Folder structure established (Characters/, DamageMechanics/, Equipment/)
- [x] JSON serialization configured (System.Text.Json with enum converter)
- [x] Namespace conventions (TextBasedGame)
- [x] Constants.cs with GamePaths for data locations

#### 0.2 Character Foundation ✅
- [x] `ICharacter` interface with Name, Body, Weapon, Stats, GameState
- [x] `IBaseStats` interface (Melee, Accuracy, Evasion, Strength, Stamina, Initiative)
- [x] `BaseStats` implementation using Dictionary<CharacterStatType, float>
- [x] `ICharacterState` interface with HealthState, CurrentStamina
- [x] `CharacterState` implementation with stamina consumption and health updates
- [x] `TestCharacter` implementation for testing
- [x] `CharacterLoader` for JSON-based character data
- [x] `CharacterData` JSON structure class

#### 0.3 Body System ✅
- [x] `IBody` interface with Parts dictionary, GetPart(), GetRandomPart()
- [x] `IBodyPart` interface (Name, Type, Effectiveness)
- [x] `BodyPartType` enum (Head, Torso, LeftArm, RightArm, LeftLeg, RightLeg)
- [x] `Body` class with humanoid parts
- [x] Cached `_partList` array for efficient random selection
- [x] `GetOverallCondition()` calculating average effectiveness

#### 0.4 Weapon System ✅
- [x] `IWeapons` interface
- [x] `IWeaponStats` interface (Name, Damage, MinStrengthRequired)
- [x] `WeaponStats` implementation with Dictionary<WeaponStatType, float>
- [x] `WeaponLoader` for JSON-based weapon data
- [x] `Sword` and `Club` implementations
- [x] Static `Create()` factory methods on weapon classes

#### 0.5 Damage Calculation ✅
- [x] `DamageCalculator` class with full pipeline
- [x] Weapon access calculation (exponential decay: `1 - exp(-strengthRatio)`)
- [x] Access clamped to 20%-100% range
- [x] Variance system (±10% multiplicative)
- [x] Normalization to 0-1.5 scale
- [x] `DamageResult` class with RawAttackValue, RawDefenseValue, NetAdvantage, NormalizedValue

#### 0.6 Profile System ✅
- [x] `StrikeQuality` enum (Glancing, Solid, Heavy, Devastating)
- [x] `BodyPartHealthState` enum (7 states: Uninjured → Destroyed)
- [x] `CharacterHealthState` enum (6 states: Healthy → Dying)
- [x] `HitProfile.ProcessHit()` — normalized value → HitResult
- [x] `BodyPartProfile.DetermineState()` — effectiveness → state enum
- [x] `BodyPartProfile.GetDescription()` — state → display string
- [x] `HitResult` class with Quality, Damage, InflictsWound, BleedingRate, NarrativeText

#### 0.7 Combat Resolution ✅
- [x] `CombatResolver` static class with extension method pattern
- [x] `ResolveAttackAgainst()` orchestrating full attack pipeline
- [x] Before/after state capture for debugging
- [x] Integration with DamageCalculator, HitProfile, BodyPartProfile
- [x] `ApplyDamage()` reducing body part effectiveness
- [x] `DisplayCombatResult()` producing narrative output
- [x] Bleeding description generation
- [x] CharacterState.UpdateHealthState() integration

#### 0.8 Debug System ✅
- [x] `CombatDebugger` class for development visibility
- [x] `RecordAttackPhase()` capturing calculation intermediates
- [x] `RecordHitResolution()` capturing hit profile results
- [x] `RecordBodyEffect()` capturing state transitions
- [x] Formatted `Print()` with box drawing characters
- [x] Quality band and bleeding label helpers

#### 0.9 Round Management ✅
- [x] `RoundManager` class handling turn flow
- [x] Initiative-based attacker/defender determination
- [x] `Players()` method returning AttackerDefender pair
- [x] `ExecuteAttack()` running both characters' turns
- [x] `IsCharacterAlive()` checks before attack resolution
- [x] Death announcement on character elimination

#### 0.10 JSON Data ✅
- [x] TestDefaultHero.json (Bowie character)
- [x] Ogre.json (enemy character)
- [x] BasicSword.json
- [x] Club.json
- [x] All JSON files copy to output directory (csproj configured)

---

## In Progress

### Phase 1: Core Combat Loop

#### 1.1 Combat Flow Completion
- [ ] Loop ExecuteAttack() until victory/defeat
- [ ] Track round counter
- [ ] End-of-round processing hook

#### 1.2 Bleeding Mechanics
- [ ] Accumulate BleedingRate per character per round
- [ ] Apply bleeding damage at end of round
- [ ] Bleeding reduction mechanic

#### 1.3 Hit Resolution Enhancement
- [ ] Integrate Accuracy for hit chance (currently unused)
- [ ] Roll-to-hit before damage calculation
- [ ] Miss narrative text

#### 1.4 Incapacitation & Death
- [ ] Define thresholds for incapacitation vs death
- [ ] Vital parts (head/torso) vs non-vital (limbs)
- [ ] Proper victory/defeat detection

---

## Questions / Blockers

### Active Questions
- [ ] Should Accuracy affect hit chance or hit quality? (Currently: neither, only Evasion reduces damage)
- [ ] How should bleeding damage scale? (Linear with rate? Accelerating?)
- [ ] Critical hit mechanics — random on any hit, or only on high normalized values?

### Resolved Questions
- ✅ Grid type: Hexagonal (flat-top)
- ✅ Text vs Unity first: Text-based C# first
- ✅ Stats separation: IBaseStats (static) vs ICharacterState (dynamic)
- ✅ Numbers visibility: Obfuscated — players see qualitative descriptors
- ✅ Body location: Direct component of ICharacter, not nested in State
- ✅ Health derivation: From body part average, not separate tracking

---

## Design Decisions Log

| Date | Decision | Rationale |
|------|----------|-----------|
| Dec 2024 | Text-based C# first | Test logic without UI complexity |
| Dec 2024 | JSON for definitions | Easy iteration, no recompile |
| Dec 2024 | Dictionary-based stats | Flexible, extensible |
| Dec 2024 | Exponential decay for weapon access | Smooth curve, never fully blocked |
| Dec 2024 | Hexagonal grid (flat-top) | Better tactical positioning |
| Dec 2024 | IBaseStats vs ICharacterState | Static vs dynamic separation |
| Jan 2025 | Obfuscated numbers | Qualitative descriptors only |
| Jan 2025 | Profile-based translation | Centralized internal→display conversion |
| Jan 2025 | Body as ICharacter component | Avoids Russian-dolling |
| Jan 2025 | Extension method for combat | Clean syntax: attacker.ResolveAttackAgainst(defender) |
| Jan 2025 | CombatDebugger as separate class | Optional, doesn't clutter main logic |

---

## Code Quality Notes

### Technical Debt Identified
1. `IBaseStats.Initative` typo (should be Initiative)
2. `Attack.cs` contains deprecated `Combat` class
3. `TestCharacter` hardcodes `new Body()` — should inject IBody
4. No null/error handling in JSON loaders
5. `HitProfile.ProcessHit()` uses `Random.Shared` directly — not testable

### Patterns Working Well
- Dictionary-based stats from JSON — very flexible
- Interface-first design — clean separation
- Static factory methods on weapons — simple creation
- Profile classes for translation — centralized logic
- Extension method for combat — reads naturally
- Separate debugger class — clean optional output

### Patterns to Adopt
- Dependency injection for Random (testability)
- Builder pattern for test data creation
- Event bus for system communication (Phase 2)

---

## Test Coverage Needed

### Unit Tests (Priority)
- [ ] DamageCalculator weapon access curve (edge cases)
- [ ] DamageCalculator normalization bounds
- [ ] HitProfile threshold boundaries
- [ ] BodyPartProfile threshold boundaries
- [ ] CharacterState health state transitions

### Integration Tests
- [ ] Full attack pipeline with deterministic random
- [ ] Character creation from JSON
- [ ] Multiple rounds of combat

### Test Infrastructure Needed
- [ ] Mock IBody implementation
- [ ] Deterministic Random wrapper/injection
- [ ] Test character builders

---

## Ideas for Later

### Translation Layer Enhancements
- Weapon descriptions based on stats
- Enemy assessment based on stat comparison
- "Perceptive" trait reveals more detail
- Post-battle detailed reports

### Body System Extensions
- Non-humanoid bodies (quadrupeds, creatures)
- Body part importance weighting for targeting
- Nested parts (hand is part of arm)

### Combat Enhancements
- Aimed shots with hit penalty
- Defensive stances
- Environmental modifiers
- Flanking bonuses

---

## Files Changed Recently

| File | Change | Date |
|------|--------|------|
| Profiles.cs | Added all enums and profile classes | Jan 2025 |
| CombatResolver.cs | Created full attack pipeline | Jan 2025 |
| CombatDebugger.cs | Created debug output system | Jan 2025 |
| RoundManager.cs | Added initiative and turn flow | Jan 2025 |
| CharacterState.cs | Added UpdateHealthState() | Jan 2025 |
| ICharacterState.cs | Added HealthState property | Jan 2025 |
| IBaseStats.cs | Added Initiative stat | Jan 2025 |
