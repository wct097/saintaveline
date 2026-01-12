# Saint Aveline - Development Plan v2.0

> **LOCAL ONLY - DO NOT PUSH TO REMOTE**
> All development in this plan occurs locally. No branches are pushed to origin.
> Integration branch: `feature/ai-build-testing`

> **Status**: Active Development
> **Current Completion**: ~35-40% (revised from codebase audit)
> **Target**: MVP Release - Playable Game Loop

---

## Executive Summary

Saint Aveline is a survival horror game requiring focused completion work. A codebase audit revealed:

- **Combat**: ~25-30% complete (dead code, no ammo system, memory leaks)
- **NPC/AI**: ~35-40% complete (1/3 family NPCs, stimulus bus not wired)
- **UI/Flow**: ~40% complete (main menu non-functional, no win/lose conditions)
- **Dialogue**: 0% complete (no implementation)

**Critical Finding**: The game cannot currently be played start-to-finish. Phase 0 establishes a working game loop before feature work begins.

---

## Gantt Chart - Development Timeline

```
PHASE 0: PLAYABLE SKELETON (Sequential - Must Complete First)
═══════════════════════════════════════════════════════════════════════════════
         │ Day 1    │ Day 2    │ Day 3    │ Day 4    │ Day 5    │
─────────┼──────────┼──────────┼──────────┼──────────┼──────────┤
P0-CORE  │██████████│██████████│██████████│          │          │ Game loop works
P0-TEST  │          │          │██████████│██████████│          │ Smoke tests pass
─────────┴──────────┴──────────┴──────────┴──────────┴──────────┘
         ▼ GATE: User confirms game is playable before Phase 1 begins

PHASE 1: PARALLEL FEATURE DEVELOPMENT
═══════════════════════════════════════════════════════════════════════════════
         │ Day 6    │ Day 7    │ Day 8    │ Day 9    │ Day 10   │ Day 11   │
─────────┼──────────┼──────────┼──────────┼──────────┼──────────┼──────────┤
TRACK A  │░░░░░░░░░░│░░░░░░░░░░│░░░░░░░░░░│░░░░░░░░░░│░░░░░░░░░░│          │
Combat   │ WS-1A    │ WS-1A    │ WS-1B    │ WS-1B    │ WS-1C    │  MERGE   │
─────────┼──────────┼──────────┼──────────┼──────────┼──────────┼──────────┤
TRACK B  │▒▒▒▒▒▒▒▒▒▒│▒▒▒▒▒▒▒▒▒▒│▒▒▒▒▒▒▒▒▒▒│▒▒▒▒▒▒▒▒▒▒│▒▒▒▒▒▒▒▒▒▒│          │
NPC      │ WS-2A    │ WS-2A    │ WS-2B    │ WS-2B    │ WS-2C    │  MERGE   │
─────────┼──────────┼──────────┼──────────┼──────────┼──────────┼──────────┤
TRACK C  │▓▓▓▓▓▓▓▓▓▓│▓▓▓▓▓▓▓▓▓▓│▓▓▓▓▓▓▓▓▓▓│▓▓▓▓▓▓▓▓▓▓│          │          │
Audio    │ WS-3A    │ WS-3A    │ WS-3B    │ WS-3B    │  MERGE   │          │
─────────┴──────────┴──────────┴──────────┴──────────┴──────────┴──────────┘
         ▼ GATE: All tracks merged, integration tests pass

PHASE 2: POLISH & DIALOGUE (Sequential after Phase 1)
═══════════════════════════════════════════════════════════════════════════════
         │ Day 12   │ Day 13   │ Day 14   │ Day 15   │ Day 16   │
─────────┼──────────┼──────────┼──────────┼──────────┼──────────┤
UI       │██████████│██████████│██████████│          │          │
Polish   │ WS-4A    │ WS-4B    │ WS-4C    │  MERGE   │          │
─────────┼──────────┼──────────┼──────────┼──────────┼──────────┤
Dialogue │          │██████████│██████████│██████████│██████████│
(if time)│          │ WS-5A    │ WS-5B    │ WS-5C    │  MERGE   │
─────────┴──────────┴──────────┴──────────┴──────────┴──────────┘

LEGEND:
  ██ = Sequential work (blocking)
  ░░ = Track A (Combat) - can run in parallel
  ▒▒ = Track B (NPC) - can run in parallel
  ▓▓ = Track C (Audio) - can run in parallel
```

---

## Parallel Processing Strategy

### Why Not Full Worktrees

Unity projects have **Library/** folder dependencies that cause corruption when multiple Unity instances access the same project. Instead:

1. **Phase 0**: Single branch, sequential work (establishes foundation)
2. **Phase 1**: Three parallel tracks using **code-only changes** in separate directories
3. **Phase 2**: Sequential polish after integration

### Track Isolation Rules

Tracks work in **non-overlapping file sets** to enable parallel development without merge conflicts:

| Track | Owns These Directories | Cannot Touch |
|-------|------------------------|--------------|
| **A: Combat** | `Items/`, `Scripts/EnemyNPCs/EnemyAttackState.cs` | FriendlyNPCs, UI, Audio |
| **B: NPC** | `Scripts/FriendlyNPCs/`, `Scripts/CommonNPC/` | Items, EnemyAttackState, UI |
| **C: Audio** | `Scripts/Audio/` (new), `Scenes/Scripts/MusicManager.cs` | Everything else |

### Merge Strategy

```
feature/ai-build-testing (integration branch)
    │
    ├── Phase 0 commits (sequential)
    │
    ├── feature/track-a-combat ──────┐
    ├── feature/track-b-npc ─────────┼── Merge all at Phase 1 end
    └── feature/track-c-audio ───────┘
    │
    └── Phase 2 commits (sequential)
```

---

## Phase 0: Playable Skeleton (REQUIRED FIRST)

**Goal**: A human can start the game, play, win, and lose.
**Branch**: `feature/ai-build-testing` (direct commits)
**Blocking**: All other work waits until Phase 0 passes smoke tests.

### P0-CORE: Critical Path Fixes

| ID | Task | File(s) | Issue |
|----|------|---------|-------|
| P0-1 | Fix main menu "New Game" button | `MainMenuController.cs` | Button has no click handler |
| P0-2 | Fix enemy combat (dead code) | `EnemyAttackState.cs:109` | `Shoot()` method never called |
| P0-3 | Fix memory leak | `EnemyAttackState.cs:10` | `new GameObject()` in field init |
| P0-4 | Add win condition | `Game.unity` + new script | No victory trigger exists |
| P0-5 | Fix game over flow | `GameOver.unity` | Scene exists but no retry/menu |
| P0-6 | Wire basic player damage | `PlayerStats.cs` | Verify death → GameOver works |

### P0-TEST: Smoke Test Verification

All must pass before Phase 1:

```
[ ] START: Main menu → "New Game" → Game scene loads
[ ] PLAY:  Player moves with WASD, looks with mouse
[ ] EQUIP: Player can open inventory, equip weapon
[ ] FIGHT: Player shoots enemy → enemy takes damage → enemy dies
[ ] HURT:  Enemy shoots player → player takes damage
[ ] LOSE:  Player dies → GameOver scene → "Retry" works
[ ] WIN:   Player reaches roof trigger → Victory screen
[ ] LOOP:  Victory/GameOver → Main Menu → New Game works
```

### Agent Instructions for Phase 0

```
AUTONOMOUS OPERATION - NO USER INPUT REQUIRED

1. Read each file before modifying
2. Make minimal changes to fix the specific issue
3. Run `dotnet build *.csproj` after each file change to verify compilation
4. Commit after each task completes: "fix(phase0): P0-X description"
5. After all P0 tasks complete, output SMOKE_TEST_READY
6. Do NOT proceed to Phase 1 until user confirms smoke tests pass
```

---

## Phase 1: Parallel Feature Development

**Prerequisite**: Phase 0 smoke tests pass
**Strategy**: Three independent tracks, merge at end

---

### Track A: Combat System (WS-1)

**Branch**: `feature/track-a-combat`
**Owns**: `Assets/_Game/Items/`, `EnemyAttackState.cs` (post Phase 0)

#### WS-1A: Ammo & Reload System
| ID | Task | Details |
|----|------|---------|
| 1A-1 | Add ammo fields to PistolItemData | `int MagazineSize`, `int CurrentAmmo`, `float ReloadTime` |
| 1A-2 | Implement reload in Pistol1Entity | R key triggers reload, blocks firing during reload |
| 1A-3 | Add ammo UI to HUD | Display "Ammo: X/Y" on screen |
| 1A-4 | Play reload animation/sound | Use existing animation system |

#### WS-1B: Damage Feedback
| ID | Task | Details |
|----|------|---------|
| 1B-1 | Add screen shake on hit | Camera shake when player takes damage |
| 1B-2 | Add hit sound effects | Play impact sound on successful hit |
| 1B-3 | Add damage numbers (optional) | Floating text showing damage dealt |

#### WS-1C: Combat Balance
| ID | Task | Details |
|----|------|---------|
| 1C-1 | Balance weapon damage values | Tune damage so combat feels fair |
| 1C-2 | Balance enemy health/damage | Enemies challenging but beatable |
| 1C-3 | Add distance falloff to player weapons | Damage decreases with range |

---

### Track B: NPC Completion (WS-2)

**Branch**: `feature/track-b-npc`
**Owns**: `Assets/_Game/Scripts/FriendlyNPCs/`, `Assets/_Game/Scripts/CommonNPC/`

#### WS-2A: Missing Family NPCs
| ID | Task | Details |
|----|------|---------|
| 2A-1 | Create DaughterNPC.cs | Clone SonNPC pattern, unique personality |
| 2A-2 | Create GrandfatherNPC.cs | Clone SonNPC pattern, slower movement |
| 2A-3 | Add prefabs for each NPC | Configure NavMeshAgent, Animator |
| 2A-4 | Place NPCs in Game.unity | Position in apartment with player |

#### WS-2B: Stimulus Integration
| ID | Task | Details |
|----|------|---------|
| 2B-1 | Wire StimulusBus to enemies | Enemies respond to gunshot events |
| 2B-2 | Wire MentalState.Tick() | Call in BaseNPC.Update() |
| 2B-3 | Add footstep stimulus | Loud footsteps alert nearby enemies |

#### WS-2C: NPC Polish
| ID | Task | Details |
|----|------|---------|
| 2C-1 | Fix null reference in target check | EnemyAttackState.cs line 86 |
| 2C-2 | Add NPC death notification | Typewriter message when family member dies |
| 2C-3 | Test all NPC commands | Follow, GoTo, Stay work correctly |

---

### Track C: Audio System (WS-3)

**Branch**: `feature/track-c-audio`
**Owns**: `Assets/_Game/Scripts/Audio/` (new), `MusicManager.cs`

#### WS-3A: Audio Manager
| ID | Task | Details |
|----|------|---------|
| 3A-1 | Create AudioManager singleton | Central audio control with pooling |
| 3A-2 | Implement ambient zones | Trigger ambient sounds by area |
| 3A-3 | Add tension music system | Music intensifies during combat |

#### WS-3B: Sound Effects
| ID | Task | Details |
|----|------|---------|
| 3B-1 | Add footstep variations | Different sounds for surface types |
| 3B-2 | Add combat sounds | Gunshots, impacts, enemy alerts |
| 3B-3 | Add UI sounds | Button clicks, inventory open/close |

---

## Phase 2: Polish & Dialogue

**Prerequisite**: Phase 1 tracks merged
**Branch**: `feature/ai-build-testing` (direct commits)

### WS-4: UI Polish

| ID | Task | Details |
|----|------|---------|
| 4A-1 | Implement Settings menu | Graphics, audio volume, controls |
| 4A-2 | Add loading screen | Show tips during scene transitions |
| 4B-1 | Polish pause menu | Better styling, animations |
| 4B-2 | Add control hints | Show key bindings on HUD |
| 4C-1 | Add accessibility options | Subtitles toggle, UI scaling |

### WS-5: Dialogue System (If Time Permits)

| ID | Task | Details |
|----|------|---------|
| 5A-1 | Create DialogueSO structure | ScriptableObject for dialogue trees |
| 5A-2 | Create DialogueManager | Singleton to control dialogue flow |
| 5B-1 | Build dialogue UI | Text box, character name, choices |
| 5B-2 | Integrate with FriendlyNPC | Trigger dialogue on interaction |
| 5C-1 | Write basic story dialogue | Opening, family interactions, ending |

---

## Agent Autonomy Guidelines

### Minimize User Input

Agents should operate autonomously with these principles:

1. **Read Before Write**: Always read a file completely before modifying
2. **Compile Check**: Run build verification after each change
3. **Atomic Commits**: One logical change per commit
4. **Self-Verify**: Run smoke tests programmatically where possible
5. **Document Blockers**: If stuck, document the issue and move to next task

### Decision Authority

| Decision Type | Agent Can Decide | Requires User |
|---------------|------------------|---------------|
| Code implementation details | Yes | No |
| File organization within owned dirs | Yes | No |
| Variable/method naming | Yes | No |
| Adding new files in owned dirs | Yes | No |
| Modifying files outside owned dirs | No | Yes |
| Changing game mechanics | No | Yes |
| Deleting existing features | No | Yes |
| Architecture changes | No | Yes |

### When Stuck

```
IF compilation_error:
    1. Read the error message carefully
    2. Check for typos, missing references
    3. Verify file paths and namespaces
    4. If unresolved after 3 attempts: document and skip task

IF merge_conflict:
    1. Keep BOTH changes if non-overlapping
    2. Prefer newer code if same lines
    3. If unclear: document and request user decision

IF unclear_requirement:
    1. Check CLAUDE.md for guidance
    2. Check existing similar code for patterns
    3. Make reasonable assumption and document it
    4. Proceed with implementation
```

---

## File Ownership Matrix

### Phase 0 (Single Agent)
All files accessible, minimal changes only.

### Phase 1 (Parallel Tracks)

```
TRACK A: Combat                    TRACK B: NPC                      TRACK C: Audio
─────────────────────────────      ─────────────────────────────     ─────────────────────────────
Assets/_Game/Items/                Assets/_Game/Scripts/CommonNPC/   Assets/_Game/Scripts/Audio/
├── ItemEntity.cs                  ├── BaseNPC.cs                    ├── AudioManager.cs (NEW)
├── ItemData.cs                    ├── CharacterEntity.cs            ├── AmbientZone.cs (NEW)
├── Pistol1Entity.cs               ├── NPCStateMachine.cs            └── TensionMusic.cs (NEW)
├── PistolItemData.cs              ├── EntityScanner.cs
├── RustyKnife/                    └── PersonalityTraits/            Assets/_Game/Scenes/Scripts/
│   └── KnifeItemInteraction.cs                                      └── MusicManager.cs
└── InventoryUI.cs                 Assets/_Game/Scripts/FriendlyNPCs/
                                   ├── FriendlyNPC.cs
Assets/_Game/Scripts/EnemyNPCs/    ├── SonNPC.cs
└── EnemyAttackState.cs (shared*)  ├── DaughterNPC.cs (NEW)
                                   ├── GrandfatherNPC.cs (NEW)
                                   └── NPC*State.cs

* EnemyAttackState.cs: Track A owns combat logic, Track B may read only
```

### Forbidden Cross-Track Modifications

| Track | Cannot Modify |
|-------|---------------|
| A | `FriendlyNPCs/*`, `CommonNPC/*` (except combat interfaces), `UI/*`, `Audio/*` |
| B | `Items/*`, `EnemyAttackState.cs` combat methods, `UI/*`, `Audio/*` |
| C | `Items/*`, `*NPC*/*`, `UI/*` |

---

## Build Verification

### Compilation Check (Run After Each Change)

```bash
# From project root
# Unity batch mode compile check
Unity -batchmode -projectPath . -quit -logFile build.log 2>/dev/null
grep -q "Compilation failed" build.log && echo "FAIL" || echo "PASS"

# Alternative: Use dotnet if .csproj is configured
dotnet build Assembly-CSharp.csproj 2>&1 | tail -5
```

### Smoke Test Script

Save as `scripts/smoke_test.sh`:

```bash
#!/bin/bash
echo "=== SAINT AVELINE SMOKE TEST ==="
echo ""
echo "Manual verification required. Check each item:"
echo ""
echo "[ ] MENU:   Main Menu 'New Game' loads Game scene"
echo "[ ] MOVE:   Player moves with WASD"
echo "[ ] LOOK:   Player looks with mouse"
echo "[ ] INVENTORY: Press I, equip weapon"
echo "[ ] SHOOT:  Left click fires weapon"
echo "[ ] DAMAGE: Enemy takes damage and dies"
echo "[ ] HURT:   Player takes damage from enemy"
echo "[ ] DEATH:  Player death shows GameOver"
echo "[ ] RETRY:  GameOver 'Retry' restarts game"
echo "[ ] WIN:    Reaching roof shows Victory"
echo "[ ] LOOP:   Victory -> Main Menu -> New Game works"
echo ""
echo "All items checked? Phase 0 complete."
```

---

## Success Criteria

### Phase 0 Complete When:
- [ ] All 8 smoke test items pass
- [ ] No compilation errors
- [ ] Game loop is playable start-to-finish

### Phase 1 Complete When:
- [ ] All three tracks merged without conflicts
- [ ] Smoke tests still pass
- [ ] Combat has ammo/reload
- [ ] All 3 family NPCs functional
- [ ] Audio plays during gameplay

### MVP Complete When:
- [ ] Player can complete full playthrough
- [ ] Combat feels responsive
- [ ] Family NPCs follow commands
- [ ] Audio enhances atmosphere
- [ ] No critical bugs or crashes
- [ ] 60 FPS on mid-range hardware

---

## Quick Reference: Agent Startup Checklist

When an agent begins work:

```
1. [ ] Read this development plan completely
2. [ ] Read CLAUDE.md for project conventions
3. [ ] Identify which Phase/Track you're working on
4. [ ] Verify you're on the correct branch
5. [ ] Read ALL files you'll modify before changing them
6. [ ] Make changes, compile, commit
7. [ ] Document any blockers or decisions
8. [ ] Do NOT push to remote
```

---

## Revision History

| Date | Author | Changes |
|------|--------|---------|
| 2026-01-12 | Claude | Initial plan creation |
| 2026-01-12 | Claude | Updated for LOCAL ONLY workflow |
| 2026-01-12 | Claude | v2.0: Complete rewrite with accurate estimates, Gantt chart, parallel tracks, agent autonomy guidelines |
