# Saint Aveline - Development Plan

> **Status**: Active Development
> **Current Completion**: ~65-70%
> **Target**: MVP Release

## Executive Summary

Saint Aveline is a survival horror game with solid core architecture. The remaining work focuses on:
- Completing partially-implemented systems (combat, UI)
- Implementing missing features (dialogue, animation sync)
- Addressing technical debt (performance, code quality)
- Polish and integration

---

## Parallel Development Strategy

This plan uses **git worktrees** to enable multiple AI agents to work simultaneously on independent features. Each workstream operates in its own worktree with a dedicated feature branch.

### Worktree Structure

```
D:\source\GitHub\
├── saintaveline/                    # Main worktree (master)
├── saintaveline-combat/             # Worktree: Combat System
├── saintaveline-dialogue/           # Worktree: Dialogue System
├── saintaveline-audio/              # Worktree: Audio System
├── saintaveline-ui/                 # Worktree: UI Framework
├── saintaveline-animation/          # Worktree: Animation Integration
└── saintaveline-tech-debt/          # Worktree: Technical Debt
```

### Setup Commands

```bash
cd D:\source\GitHub\saintaveline

# Create worktrees for each workstream
git worktree add ../saintaveline-combat -b feature/combat-system
git worktree add ../saintaveline-dialogue -b feature/dialogue-system
git worktree add ../saintaveline-audio -b feature/audio-system
git worktree add ../saintaveline-ui -b feature/ui-framework
git worktree add ../saintaveline-animation -b feature/animation-integration
git worktree add ../saintaveline-tech-debt -b feature/tech-debt-cleanup
```

---

## Workstream Definitions

### WS-1: Combat System Enhancement
**Branch**: `feature/combat-system`
**Priority**: High
**Dependencies**: None
**Estimated Effort**: Medium

#### Current State (50%)
- Basic melee attack working
- Damage calculation exists
- Distance-based falloff implemented

#### Deliverables
- [ ] **CS-1.1**: Implement ranged weapon system (pistol mechanics exist but unused)
- [ ] **CS-1.2**: Add melee range indicators and hit feedback
- [ ] **CS-1.3**: Implement damage feedback (screen shake, blood effects, hit sounds)
- [ ] **CS-1.4**: Add parry/block mechanics for player
- [ ] **CS-1.5**: Balance damage values and weapon stats

#### Key Files
- `Assets/_Game/Scripts/EnemyNPCs/EnemyAttackState.cs`
- `Assets/_Game/Items/Pistol1Entity.cs`
- `Assets/_Game/Items/RustyKnife/KnifeItemInteraction.cs`

---

### WS-2: Dialogue System
**Branch**: `feature/dialogue-system`
**Priority**: High
**Dependencies**: None
**Estimated Effort**: Large

#### Current State (5%)
- No dialogue implementation exists
- NPC interaction framework exists (can be extended)

#### Deliverables
- [ ] **DL-1.1**: Design dialogue data structure (ScriptableObject-based)
- [ ] **DL-1.2**: Create DialogueManager singleton
- [ ] **DL-1.3**: Implement dialogue tree with branching
- [ ] **DL-1.4**: Build dialogue UI (text box, character portrait, choices)
- [ ] **DL-1.5**: Integrate with FriendlyNPC interaction system
- [ ] **DL-1.6**: Add dialogue triggers for story events

#### Key Files (New)
- `Assets/_Game/Scripts/Dialogue/DialogueManager.cs`
- `Assets/_Game/Scripts/Dialogue/DialogueSO.cs`
- `Assets/_Game/Scripts/Dialogue/DialogueNode.cs`
- `Assets/_Game/UI/DialogueOverlay/`

---

### WS-3: Audio System
**Branch**: `feature/audio-system`
**Priority**: Medium
**Dependencies**: None
**Estimated Effort**: Medium

#### Current State (40%)
- StimulusBus exists for gunshot propagation
- Basic audio clips on some actions
- MusicManager exists but minimal

#### Deliverables
- [ ] **AU-1.1**: Implement AudioManager singleton with pooling
- [ ] **AU-1.2**: Create ambient sound zone system
- [ ] **AU-1.3**: Implement dynamic music (tension-based)
- [ ] **AU-1.4**: Add footstep variations by surface type
- [ ] **AU-1.5**: Integrate voice line playback system
- [ ] **AU-1.6**: Add 3D spatial audio for enemy sounds

#### Key Files
- `Assets/_Game/Scenes/Scripts/MusicManager.cs`
- `Assets/_Game/Scripts/StimulusBus.cs`
- `Assets/_Game/Scripts/PlayerCharacter/FootstepAudio.cs`

---

### WS-4: UI Framework Completion
**Branch**: `feature/ui-framework`
**Priority**: Medium
**Dependencies**: None
**Estimated Effort**: Medium

#### Current State (60%)
- Inventory UI working
- Mission overlay working
- Pause menu exists
- HUD elements present

#### Deliverables
- [ ] **UI-1.1**: Implement Settings/Options menu (graphics, audio, controls)
- [ ] **UI-1.2**: Create tutorial/help overlay system
- [ ] **UI-1.3**: Build quest log/journal UI
- [ ] **UI-1.4**: Add accessibility options (subtitles, colorblind modes)
- [ ] **UI-1.5**: Implement loading screen with tips
- [ ] **UI-1.6**: Polish existing UI (transitions, animations)

#### Key Files
- `Assets/_Game/UI/PauseMenu/PauseMenuController.cs`
- `Assets/_Game/Scripts/UI/MainMenuController.cs`
- `Assets/_Game/Missions/UI/MissionOverlay/`

---

### WS-5: Animation Integration
**Branch**: `feature/animation-integration`
**Priority**: Medium
**Dependencies**: None
**Estimated Effort**: Medium

#### Current State (30%)
- NPCs have Animator components
- No state machine synchronization
- Movement animations not synced to NavMesh speed

#### Deliverables
- [ ] **AN-1.1**: Create AnimationController base class for NPCs
- [ ] **AN-1.2**: Sync NPC state machine with Animator states
- [ ] **AN-1.3**: Add movement animation blending (idle/walk/run)
- [ ] **AN-1.4**: Implement combat animations (attack, hit reactions)
- [ ] **AN-1.5**: Add player animation controller
- [ ] **AN-1.6**: Create animation event system for gameplay hooks

#### Key Files
- `Assets/_Game/Scripts/CommonNPC/BaseNPC.cs`
- `Assets/_Game/Scripts/CommonNPC/NPCStateMachine.cs`
- `Assets/_Game/Scripts/EnemyNPCs/*.cs` (all state files)

---

### WS-6: Technical Debt Cleanup
**Branch**: `feature/tech-debt-cleanup`
**Priority**: High
**Dependencies**: None (but should merge early)
**Estimated Effort**: Small-Medium

#### Issues to Address

**Critical**:
- [ ] **TD-1.1**: Refactor reflection-based dispatch in `FriendlyNPC.cs` and `ItemEntity.cs`
  - Create `ActionRegistry<T>` with cached method lookups
  - Replace O(n) reflection with O(1) dictionary lookup

- [ ] **TD-1.2**: Fix runtime GameObject creation in `EnemyAttackState.cs`
  - Serialize `_firePoint` as Inspector field
  - Remove `new GameObject()` from field initialization

**High**:
- [ ] **TD-1.3**: Decouple UI from NPC logic
  - Extract `onGoTo()` dialog instantiation to UIController
  - Use events/callbacks instead of direct Instantiate

- [ ] **TD-1.4**: Enable nullable reference types project-wide
  - Add `#nullable enable` to all C# files
  - Fix resulting warnings

**Medium**:
- [ ] **TD-1.5**: Fix naming convention inconsistencies
  - `setState()` → `SetState()` (PascalCase)
  - Audit all public methods

- [ ] **TD-1.6**: Replace string-based UI paths with direct references
  - `transform.Find("ButtonContainer/...")` → serialized fields

#### Key Files
- `Assets/_Game/Scripts/FriendlyNPCs/FriendlyNPC.cs`
- `Assets/_Game/Scripts/EnemyNPCs/EnemyAttackState.cs`
- `Assets/_Game/Items/ItemEntity.cs`

---

## Integration Schedule

### Phase 1: Foundation (Parallel)
All workstreams can begin simultaneously. Technical debt (WS-6) should merge first to establish clean patterns.

```
Week 1-2:
├── WS-6: Tech Debt Cleanup (merge first)
├── WS-1: Combat System (in progress)
├── WS-2: Dialogue System (in progress)
├── WS-3: Audio System (in progress)
├── WS-4: UI Framework (in progress)
└── WS-5: Animation Integration (in progress)
```

### Phase 2: Integration
After initial features complete, integration branches merge to master with testing.

```
Week 3-4:
├── Merge WS-1 (Combat) → master
├── Merge WS-5 (Animation) → master
├── Merge WS-3 (Audio) → master
└── Integration testing
```

### Phase 3: Polish
Final features and cross-system integration.

```
Week 5-6:
├── Merge WS-2 (Dialogue) → master
├── Merge WS-4 (UI) → master
├── Cross-system integration
├── Bug fixing
└── Performance optimization
```

---

## Conflict Avoidance Guidelines

### File Ownership by Workstream

| Directory/File | Owner | Notes |
|----------------|-------|-------|
| `Scripts/EnemyNPCs/EnemyAttackState.cs` | WS-1, WS-6 | Coordinate on `_firePoint` fix |
| `Scripts/CommonNPC/*` | WS-5, WS-6 | Animation adds, debt fixes |
| `Scripts/FriendlyNPCs/*` | WS-2, WS-6 | Dialogue integration, debt |
| `Scripts/Dialogue/*` | WS-2 | Exclusive |
| `Scripts/Audio/*` | WS-3 | Exclusive |
| `UI/*` | WS-4 | Exclusive |
| `Missions/*` | WS-4 | Quest log UI |

### Merge Order
1. **WS-6 (Tech Debt)** - First, establishes patterns
2. **WS-5 (Animation)** - Base NPC changes
3. **WS-1 (Combat)** - Uses animation system
4. **WS-3 (Audio)** - Independent
5. **WS-2 (Dialogue)** - Uses UI patterns
6. **WS-4 (UI)** - Final polish

---

## Agent Instructions

When working in a worktree:

1. **Stay in your lane**: Only modify files owned by your workstream
2. **Sync regularly**: `git fetch origin && git rebase origin/master`
3. **Small commits**: Atomic, focused changes
4. **Test before PR**: Verify Unity compiles without errors
5. **Document changes**: Update this plan as work progresses

### Starting Work in a Worktree

```bash
# Navigate to worktree
cd D:\source\GitHub\saintaveline-combat

# Verify branch
git branch

# Sync with master
git fetch origin
git rebase origin/master

# Begin work
# ...

# Commit
git add .
git commit -m "feat(combat): implement ranged weapon system"

# Push for PR
git push -u origin feature/combat-system
```

---

## Success Criteria

### MVP Requirements
- [ ] Player can complete a full playthrough (ground floor → roof)
- [ ] Combat feels responsive with feedback
- [ ] Family NPCs respond to player commands
- [ ] Basic dialogue for story beats
- [ ] Audio enhances atmosphere
- [ ] UI is functional and polished
- [ ] No critical bugs or crashes

### Performance Targets
- 60 FPS on mid-range hardware
- < 3 second load times
- No memory leaks during extended play

---

## Revision History

| Date | Author | Changes |
|------|--------|---------|
| 2026-01-12 | Claude | Initial plan creation |
