# Game Vision & Architecture

## Game Concept

A 2D tactical squad management RPG inspired by:
- **Battle Brothers**: Turn-based tactical combat, squad management, character permadeath
- **RimWorld**: Emergent storytelling, "story generator" feel
- **Football Manager / OOTP**: Deep stat systems, spreadsheet complexity, interconnected numbers creating unique dynamics

### Core Experience Goals
- Players feel emotionally invested in their squad members
- Each battle carries meaningful consequences for individuals and the squad
- Character deaths have emotional and mechanical impact
- Complex stat interactions create unique dynamics each playthrough
- Characters are far more than basic RPG stat blocks
- **Decisions feel intuitive, not mathematical** — the game rewards tactical thinking over spreadsheet optimization

---

## Design Philosophy: Obfuscated Numbers

### The Core Principle
**A stat-heavy game must not feel like a number-cruncher to the player.**

All game mechanics run on precise numerical systems internally, but players never see raw numbers. Instead, they see:
- **Qualitative descriptors**: "Wounded", "Badly Injured" instead of "Health: 73%"
- **Relative comparisons**: "Much stronger than you", "Evenly matched"
- **Ranges and uncertainty**: "This sword hits hard" not "Damage: 10-15"

### Why This Matters
- **Discourages meta-gaming**: Players can't optimize based on exact thresholds
- **Encourages intuition**: Decisions feel like tactical judgment, not math problems
- **Preserves mystery**: Even experienced players won't know exact mechanics
- **Creates emergent stories**: "He survived against all odds" vs "He had 2 HP left"
- **Matches the fiction**: Characters don't see health bars floating above enemies

### Translation Layer Architecture (Implemented)
```
Internal Systems (Precise)     →     Translation Layer     →     Player Display (Qualitative)
────────────────────────────────────────────────────────────────────────────────────────────
Effectiveness: 73              →     BodyPartProfile       →     "Wounded"
NormalizedValue: 0.85          →     HitProfile            →     "Heavy blow"
HealthState enum               →     CharacterState        →     "Injured"
BleedingRate: 3                →     CombatResolver        →     "Moderate bleeding"
```

### What Gets Obfuscated
| System | Internal | Player Sees |
|--------|----------|-------------|
| Body Part Health | Effectiveness 0-100 | "Mangled", "Barely Scratched" |
| Hit Quality | NormalizedValue 0-1.5 | "Glancing blow", "Devastating strike" |
| Character Health | HealthState enum | "Healthy", "Badly Injured", "Dying" |
| Weapon Damage | Float damage value | "Hits hard", "Quick but light" |
| Bleeding | Integer rate 0-6+ | "Light bleeding", "Critical bleeding" |

### Controlled Revelation (Planned)
Some information can be revealed through:
- **Character traits**: "Perceptive" trait shows slightly more detail about enemies
- **Experience**: Veterans might give better assessments
- **Scout actions**: Spending time/resources to learn more
- **Post-battle reports**: More detailed breakdown after fights end

---

## Architectural Principles

### 1. Data-Driven Design
All game definitions are stored in JSON files—not hardcoded. This allows tweaking and expanding without recompiling.

**Current JSON Data**:
- Characters: `Assets/Data/Characters/*.json` (TestDefaultHero.json, Ogre.json)
- Weapons: `Assets/Data/Weapons/*.json` (BasicSword.json, Club.json)

**JSON Structure Pattern**:
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

### 2. Interface-First Design
Every major system is defined by an interface before implementation:
- `ICharacter` — character contract
- `IBody` / `IBodyPart` — body system contract
- `IWeapons` / `IWeaponStats` — equipment contract
- `IBaseStats` / `ICharacterState` — stat system contract

**Why it matters**: Enables dependency injection, mock objects for testing, and clean separation between definition and implementation.

### 3. Static vs Dynamic State Separation
A critical architectural decision separates unchanging character definitions from mutable game state:

| Concept | Interface | Purpose |
|---------|-----------|---------|
| **Blueprint** | `IBaseStats` | Loaded from JSON, never changes during gameplay |
| **Living State** | `ICharacterState` | Changes every moment (stamina, health state) |
| **Identity** | `IBody` | Part of character but contains mutable effectiveness |

**Why Body isn't inside CharacterState**: Body represents both identity (what parts exist) and state (how damaged they are). Nesting it would create awkward "Russian-dolling" where accessing body requires going through state.

### 4. Component-Based Characters
Each character is a container for modular systems:
```csharp
public interface ICharacter
{
    string Name { get; }
    IBody Body { get; }              // Physical structure
    IWeapons? Weapon { get; }        // Equipment
    IBaseStats Stats { get; }        // Static definition
    ICharacterState GameState { get } // Dynamic state
    float GetOverallCondition();     // Derived from body
}
```

### 5. Profile-Based Translation
The game uses "Profile" classes to translate internal values to qualitative states:

- **HitProfile**: `NormalizedValue` → `StrikeQuality` enum + `HitResult` object
- **BodyPartProfile**: `Effectiveness` → `BodyPartHealthState` enum + description string

This keeps translation logic centralized and consistent.

---

## Core Systems Overview

### Damage Calculation Pipeline
The combat system uses a multi-phase pipeline:

```
Phase 1: Attack Calculation
  Weapon Damage → Weapon Access % → Accessible Damage → Variance → Final Attack Value
                       ↑
                Strength / MinRequired (exponential decay curve)

Phase 2: Hit Resolution
  Final Attack Value - Evasion = Net Advantage → Normalize to 0-1.5 → StrikeQuality

Phase 3: Body Effect
  HitResult.Damage → Apply to BodyPart.Effectiveness → Update CharacterState.HealthState
```

### Weapon Access Formula
```csharp
var strengthRatio = strength / Math.Max(minRequirement, 1f);
var accessPercent = 1f - (float)Math.Exp(-strengthRatio);
return Math.Clamp(accessPercent, 0.2f, 1.0f);
```

This exponential decay curve means:
- At exact requirement: ~63% weapon access
- Double requirement: ~86% access
- Half requirement: ~39% (clamped to 20% minimum)

### Body System
Characters have bodies composed of `BodyPart` objects:
- Head, Torso, LeftArm, RightArm, LeftLeg, RightLeg
- Each part has `Effectiveness` (0-100)
- Overall health derived from average effectiveness across all parts

### State Enums (Implemented)

**StrikeQuality** — Hit quality bands:
- Glancing (0.0-0.3), Solid (0.3-0.7), Heavy (0.7-1.2), Devastating (1.2+)

**BodyPartHealthState** — Body part condition:
- Uninjured, BarelyHit, Wounded, BadlyWounded, CriticallyWounded, Mangled, Destroyed

**CharacterHealthState** — Overall character condition:
- Healthy, Hurt, Injured, BadlyInjured, Critical, Dying

---

## Technical Stack

- **Language**: C# (.NET 10.0)
- **Architecture**: Console application (core logic), Unity planned as visualization layer
- **Data**: JSON files with System.Text.Json for serialization
- **Grid**: Hexagonal (flat-top) planned for tactical combat
- **State Management**: Enum-based state tracking with profile translators

---

## Design Decisions Log

| Date | Decision | Rationale |
|------|----------|-----------|
| Dec 2024 | Text-based C# first, Unity later | Test game logic without UI complexity |
| Dec 2024 | JSON for all definitions | Easy iteration, human-readable, no recompile |
| Dec 2024 | Dictionary-based stats in JSON | Flexible, add stats without code changes |
| Dec 2024 | Exponential decay for weapon access | Smooth curve, weapons never fully unusable |
| Dec 2024 | Hexagonal grid (flat-top) | Better tactical positioning than square |
| Dec 2024 | IBaseStats vs ICharacterState separation | Static definitions vs dynamic game state |
| Jan 2025 | Obfuscated numbers | Qualitative descriptors, not raw numbers |
| Jan 2025 | Profile-based translation | Centralized logic for internal→display conversion |
| Jan 2025 | Body as direct ICharacter component | Avoids Russian-dolling, body is identity + state |

---

## Out of Scope for Current Phase
- Detailed graphics/animations
- Sound/music
- Save/load system
- Complex UI
- Balancing (systems first)
- Traits, wounds, relationships (Phase 2)
