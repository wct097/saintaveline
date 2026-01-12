# CLAUDE.md

> **LOCAL ONLY - DO NOT PUSH TO REMOTE**
> This branch (feature/ai-setup) and all AI configuration changes are LOCAL ONLY.
> NEVER push these changes to the remote repository until explicitly authorized by the user.

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

**Project Name**: Saint Aveline
**Purpose**: A tense survival horror game set in the world of Yanorra, where players take the role of a family patriarch struggling to escape a siege with his family.
**Tech Stack**: Unity 6000, C#, NavMesh Components, TextMeshPro, Unity UI Toolkit

## Key Architecture

- **Unity 6000** game engine with C# scripting
- **First-Person / Third-Person toggle** camera system using CharacterController
- **AI State Machine** for enemy and NPC behavior (Patrol, Alert, Chase, Attack)
- **NavMesh-based pathfinding** for AI navigation
- **Custom C# state machines** for family NPCs (Idle, Follow states)
- **Stealth mechanics** with raycast-based vision and detection systems
- **Permadeath system** for family members

## Development Guidelines

### Code Style
- Follow Unity/C# conventions with PascalCase for public members, camelCase for private
- Use meaningful variable and method names
- Keep MonoBehaviour scripts focused on single responsibilities
- Prefer composition over inheritance for game systems
- Use SerializeField for inspector-exposed private fields

### Testing Approach
- Test AI behavior in isolation before integration
- Verify NavMesh navigation works across all floor levels
- Test stealth detection with various lighting conditions
- Validate family NPC state transitions

### Git Workflow
- Use feature branches (feature/description)
- Atomic commits with descriptive messages
- Main branch: master
- Use conventional commits format

## Important Context

- The game has 3 test floors: Ground Floor (guards), 9th Floor (player apartment), Roof (escape point)
- Family NPCs: Pachacuti (son), Kusi-Rose (daughter), Tupac (grandfather) - each with unique abilities
- Victory conditions: Reach roof and signal helicopter OR eliminate all guards
- Failure conditions: Player or essential family member death

## AI Assistant Instructions

When assisting with this project:
1. Follow existing Unity/C# patterns and conventions
2. Consider performance implications for AI systems
3. Use descriptive commit messages
4. Be mindful of NavMesh requirements for new areas
5. Test state machine transitions thoroughly

## Do's and Don'ts

**Do:**
- Read relevant scripts before making changes
- Ask clarifying questions when requirements are unclear
- Suggest improvements while respecting existing patterns
- Consider mobile/VR performance where applicable
- Use Unity's built-in systems when appropriate

**Don't:**
- Make breaking changes to state machines without discussion
- Skip null checks for Unity object references
- Ignore performance implications for AI raycasts
- Add dependencies without justification
- Modify prefab overrides unnecessarily
