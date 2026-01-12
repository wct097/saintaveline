# Feature Command
<!-- AI-SETUP-SCRIPT-TAG: v2.39.0 -->

## Intelligent Feature Development Session Management

Provides structured feature development with context preservation, progress tracking, and seamless session resumption across multiple Claude Code sessions.

### Core Functionality

The `/feature` command enables:
- **Feature Management**: Create, continue, and archive feature development work
- **Session Continuity**: Save and restore context between AI sessions
- **Progress Tracking**: Requirement checklists and completion status
- **Branch Association**: Track features across multiple git branches
- **Decision History**: Preserve technical decisions and reasoning

### Command Usage

```bash
/feature                    # Interactive menu to start/continue features
/feature save              # Save current session context
/feature list              # List all active features
/feature archive <name>    # Archive completed feature
/feature status            # Show current feature status
```

### Feature Lifecycle

**Status Progression:**
1. `planning` - Requirements gathering and design
2. `in-progress` - Active development
3. `testing` - Implementation complete, testing phase
4. `completed` - Ready for merge
5. `archived` - Moved to archive after merge

### File Structure

Features are tracked in markdown files:
```
docs/
├── features/              # Active features
│   └── <feature-name>.md  # Feature tracking document
├── features-archive/      # Completed features
└── features-template.md   # Template for new features
```

### Feature Document Format

Each feature document contains:
- **Overview**: Name, status, dates, branch associations
- **Requirements**: Checkbox list of requirements
- **Technical Design**: Architecture decisions and approach
- **Session History**: Progress from each work session
- **Code Changes**: Modified files and purposes
- **Next Steps**: Clear starting points for resumption

### Branch Association

Features intelligently track associated branches without constraining naming:
- **Started On Branch**: Where feature work began
- **Current Branch**: Active development branch
- **Associated Branches**: All branches used for this feature
- Auto-detects branch switches and prompts for association
- Supports any branch naming convention (JIRA, descriptive, etc.)

### Session Management

**Starting New Feature:**
```
$ /feature
🚀 Feature Development Manager

No active features found.
Create new feature? (y/n): y
Enter feature name: user-authentication
Enter brief description: Implement JWT-based authentication system
```

**Continuing Existing Feature:**
```
$ /feature
Found 2 active features:
1. user-authentication (in-progress) - feature/auth-system
2. api-rate-limiting (planning) - main

Select feature to continue [1-2]: 1
Loading feature: user-authentication

📋 Last Session Summary:
- Implemented JWT token generation
- Added refresh token mechanism
- Started middleware integration

⚠️ Outstanding Tasks:
- Complete route protection middleware
- Add token revocation endpoint
- Write authentication tests
```

**Saving Session Progress:**
```
$ /feature save
Saving session for: user-authentication

What did you accomplish this session?
> Completed middleware, added token revocation

Any technical decisions made?
> Used Redis for token blacklist, 15min expiry

What should be done next session?
> Write comprehensive tests, update API docs

Session saved successfully!
```

### Integration with Other Commands

**With `/prime`:**
- Automatically loads feature context after selection
- Includes feature requirements in project context

**With `/save`:**
- Adds feature name to commit messages
- Links commits to feature progress

**With `/clean`:**
- Validates feature branch cleanliness
- Ensures tests pass before marking complete

### AI Behavior Instructions

When `/feature` is executed:

1. **Check for existing features** in `docs/features/`
2. **Detect current git branch** to suggest relevant feature
3. **Interactive prompts** for feature selection or creation
4. **Load full context** including session history
5. **Display actionable summary** with clear next steps

When `/feature save` is executed:

1. **Identify current feature** from context or prompt
2. **Gather session information** via interactive prompts
3. **Update feature document** with timestamped session entry
4. **Preserve technical decisions** for future reference
5. **Generate next session hints** for easy resumption

### Smart Branch Detection

The command intelligently handles branch associations:

```bash
# When switching branches
$ git checkout feature/JIRA-1234-oauth
$ /feature

Detected branch change: feature/JIRA-1234-oauth
This branch is not associated with any feature.

Options:
1. Continue 'user-authentication' on this branch
2. Start new feature for this branch
3. Work without feature tracking

Selection: 1
✓ Added branch to user-authentication feature
```

### Feature Templates

New features are created from template with sections for:
- Requirements checklist
- Acceptance criteria
- Technical approach
- Testing strategy
- Documentation needs
- Session history

### Archive Management

Completed features are preserved in `docs/features-archive/`:
- Maintains full history
- Searchable reference
- Decision documentation
- Success patterns

### Best Practices

**For Developers:**
- Use descriptive feature names
- Update progress after each session
- Document key decisions
- Link related issues/PRs

**For AI Assistants:**
- Always check for active features on startup
- Save session context before ending work
- Preserve decision reasoning
- Maintain clear next steps

### Example Feature Document

```markdown
# Feature: user-authentication

## Overview
- **Status**: in-progress
- **Started**: 2024-11-20
- **Last Updated**: 2024-11-22
- **Started On Branch**: main
- **Current Branch**: feature/JIRA-1234-auth
- **Associated Branches**:
  - feature/JIRA-1234-auth
  - bugfix/refresh-token-expiry

## Requirements
- [x] JWT token generation
- [x] Token refresh mechanism
- [x] Route protection middleware
- [ ] Token revocation
- [ ] Rate limiting
- [ ] Comprehensive tests

## Session History

### Session 2024-11-22 14:30
**Progress:**
- Completed route protection middleware
- Added token revocation with Redis blacklist
- Fixed refresh token expiry issue

**Decisions:**
- Using Redis for token blacklist (15min TTL)
- Separate refresh token rotation strategy

**Next Steps:**
- Write comprehensive test suite
- Update API documentation
- Add rate limiting to auth endpoints
```

### Error Handling

The command handles common scenarios:
- No features directory exists (creates it)
- Corrupted feature files (offers repair)
- Branch conflicts (prompts for resolution)
- Missing git repository (works in degraded mode)

### Security Considerations

- All data stored locally in repository
- No external dependencies or services
- Respects .gitignore patterns
- No automatic commits without consent
- Sensitive data warnings in templates

### Performance Optimization

- Lazy loading of feature documents
- Cached git branch detection
- Incremental session updates
- Efficient file operations

### Future Enhancements

Planned improvements:
- GitHub issue integration
- PR linking and tracking
- Team collaboration features
- Metrics and velocity tracking
- AI-powered session summaries
