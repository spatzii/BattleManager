# Development Roadmap

## Phase 0: Text-Based Foundation ✅ COMPLETE
**Goal**: Build core game logic in pure C# before adding Unity visualization.

### 0.1 Project Setup ✅
- [x] Create C# console project (.NET 10.0)
- [x] Set up folder structure (Characters/, DamageMechanics/, Equipment/)
- [x] Configure JSON serialization with System.Text.Json
- [x] Establish namespace conventions (TextBasedGame)
- [x] Constants class with GamePaths for data locations

### 0.2 Character Foundation ✅
- [x] `ICharacter` interface with Name, Body, Weapon, Stats, GameState
- [x] `IBaseStats` interface (Melee, Accuracy, Evasion, Strength, Stamina, Initiative)
- [x] `ICharacterState` interface with HealthState, CurrentStamina, UpdateHealthState()
- [x] `CharacterState` implementation with stamina consumption
- [x] `TestCharacter` implementation
- [x] `CharacterLoader` for JSON-based character data
- [x] `CharacterStatType` enum for flexible stat definitions

### 0.3 Body System ✅
- [x] `IBody` interface with Parts dictionary, GetPart(), GetRandomPart()
- [x] `IBodyPart` interface (Name, Type, Effectiveness)
- [x] `BodyPartType` enum (Head, Torso, LeftArm, RightArm, LeftLeg, RightLeg)
- [x] `Body` class with standard humanoid parts
- [x] Cached `_partList` array for efficient random selection
- [x] `GetOverallCondition()` on ICharacter calculating average effectiveness

### 0.4 Weapon System ✅
- [x] `IWeapons` interface
- [x] `IWeaponStats` interface (Name, Damage, MinStrengthRequired)
- [x] `WeaponStats` class with dictionary-based storage
- [x] `WeaponLoader` for JSON-based weapon data
- [x] `WeaponStatType` enum (Damage, MinStrengthRequired)
- [x] `Sword` and `Club` implementations
- [x] Static `Create()` factory methods

### 0.5 Damage Calculation ✅
- [x] `DamageCalculator` class with full pipeline
- [x] Weapon access calculation (exponential decay curve)
- [x] Variance system (±10%)
- [x] Normalization to 0-1.5 scale
- [x] `DamageResult` class carrying all calculation outputs

### 0.6 Profile System ✅
- [x] `StrikeQuality` enum (Glancing, Solid, Heavy, Devastating)
- [x] `BodyPartHealthState` enum (7 states from Uninjured to Destroyed)
- [x] `CharacterHealthState` enum (6 states from Healthy to Dying)
- [x] `HitProfile.ProcessHit()` — normalized value to HitResult
- [x] `BodyPartProfile.DetermineState()` — effectiveness to state enum
- [x] `HitResult` class with Quality, Damage, InflictsWound, BleedingRate, NarrativeText

### 0.7 Combat Resolution ✅
- [x] `CombatResolver` static class as extension method on ICharacter
- [x] `ResolveAttackAgainst()` orchestrating full attack pipeline
- [x] `ApplyDamage()` reducing body part effectiveness
- [x] `DisplayCombatResult()` producing narrative output
- [x] Integration with CharacterState.UpdateHealthState()

### 0.8 Debug System ✅
- [x] `CombatDebugger` class for development visibility
- [x] `RecordAttackPhase()` capturing calculation details
- [x] `RecordHitResolution()` capturing hit profile results
- [x] `RecordBodyEffect()` capturing state changes
- [x] Formatted `Print()` output with box drawing

### 0.9 Round Management ✅
- [x] `RoundManager` class handling turn flow
- [x] Initiative-based attacker/defender determination
- [x] `ExecuteAttack()` running both characters' attacks
- [x] `AttackerDefender` helper class
- [x] Alive checks before attack resolution

---

## Phase 1: Core Combat Loop (Current Phase)
**Goal**: Complete combat works end-to-end with proper turn cycling.

### 1.1 Combat Flow Completion
- [ ] Multiple rounds until victory/defeat condition
- [ ] Turn counter tracking
- [ ] End-of-round processing hook (for bleeding, effects)
- [ ] Victory/defeat determination

### 1.2 Bleeding Mechanics
- [ ] `BleedingRate` already captured in `HitResult`
- [ ] Track accumulated bleeding per character
- [ ] End-of-turn bleeding damage application
- [ ] Bleeding reduction over time or with treatment

### 1.3 Hit Resolution Phase 2
- [ ] Integrate Accuracy stat from attacker
- [ ] Integrate Evasion stat for miss chance (currently only reduces damage)
- [ ] Roll-to-hit before damage calculation
- [ ] Miss/hit narrative text

### 1.4 Incapacitation & Death
- [ ] Define incapacitation threshold (overall condition)
- [ ] Define death threshold
- [ ] Vital body parts (head, torso) causing faster death
- [ ] Non-vital parts (limbs) causing effectiveness loss but slower death

**Phase 1 Deliverable**: Two characters fight multiple rounds to death with narrative output.

---

## Phase 2: Stat Architecture
**Goal**: Build the modifier systems that make stats dynamic.

### 2.1 Stat Modifier System
- [ ] `StatModifier` class (value, type, priority, source)
- [ ] Modifier types: Flat, PercentAdd, PercentMult
- [ ] `Stat` class with modifier list and caching
- [ ] Add/remove modifiers with source tracking
- [ ] Order of operations enforcement

### 2.2 Trait System
- [ ] `TraitDefinition` JSON structure
- [ ] Traits apply stat modifiers
- [ ] Tag system for trait interactions (Fearsome vs Fearless)
- [ ] Trait loading from JSON

### 2.3 Wound System
- [ ] `WoundDefinition` JSON structure
- [ ] Wounds as stat modifier sources
- [ ] Wound healing over time (battle count)
- [ ] Scar conversion system (permanent lesser version)
- [ ] Location-based wound effects

### 2.4 Relationship System
- [ ] Pairwise relationship tracking
- [ ] Modification triggers (combat events)
- [ ] Cohesion bonus calculation
- [ ] Morale impact from relationships

**Phase 2 Deliverable**: Characters have traits and wounds affecting their stats.

---

## Phase 3: Combat Integration
**Goal**: All systems affect combat meaningfully.

### 3.1 Modifier Resolution in Combat
- [ ] Combat pulls from all modifier sources
- [ ] Order of operations: base → traits → wounds → relationships → situational
- [ ] Trait interaction checks (negation)

### 3.2 Combat Events
- [ ] Event definitions: OnAttackDeclared, OnDamageTaken, OnCharacterDowned
- [ ] Systems subscribe to relevant events
- [ ] Trait triggers on events
- [ ] Relationship updates from combat

### 3.3 Morale System
- [ ] Squad morale tracking
- [ ] Individual morale affected by relationships
- [ ] Morale stat modifiers
- [ ] Route/flee mechanics

### 3.4 Full Translation Layer
- [ ] All stats have display descriptors
- [ ] Combat narrative generator enhancements
- [ ] Character assessment system ("He looks tough")
- [ ] Relative comparison system

**Phase 3 Deliverable**: Combat feels consequential. Stats interact. No numbers visible.

---

## Phase 4: Unity Integration
**Goal**: Add visual layer to existing logic.

### 4.1 Project Setup
- [ ] Create Unity project (Unity 6.2, 2D URP template)
- [ ] Reference C# game logic as separate assembly
- [ ] New Input System setup
- [ ] Folder structure mirroring game logic

### 4.2 Hexagonal Grid
- [ ] Hex grid implementation (flat-top)
- [ ] Cube coordinate system (q, r, s where q + r + s = 0)
- [ ] Grid rendering with tiles
- [ ] Click detection and coordinate conversion

### 4.3 Character Visualization
- [ ] Character sprites on grid
- [ ] Selection and highlighting
- [ ] Health/status indicators (qualitative, not numbers)
- [ ] Movement visualization

### 4.4 Combat UI
- [ ] Action selection UI
- [ ] Target selection overlay
- [ ] Combat log panel (narrative text)
- [ ] Character status panels with qualitative descriptors

**Phase 4 Deliverable**: Playable tactical combat in Unity with hex grid.

---

## Phase 5: Campaign Loop
**Goal**: Basic campaign structure.

### 5.1 World Map
- [ ] Node-based world map
- [ ] Squad movement between nodes
- [ ] Time passage system

### 5.2 Between-Battle Systems
- [ ] Wound healing over time
- [ ] Character rest/recovery
- [ ] Scar processing from healed wounds

### 5.3 Campaign Flow
- [ ] Battle → Heal → Travel → Battle loop
- [ ] Random events during travel
- [ ] Character recruitment

**Phase 5 Deliverable**: Playable campaign loop with squad evolution.

---

## Future Phases (Post-Prototype)
- Visual polish and animations
- Sound and music
- Save/load system
- UI improvements
- Balancing pass
- Content expansion (more traits, wounds, events)
- Story events and narrative systems
- Advanced translation layer (trait-based perception differences)
