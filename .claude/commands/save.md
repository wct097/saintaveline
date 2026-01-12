# Save Command - AI-Enhanced Git Commits

## Overview

The `/save` command provides an intelligent, AI-enhanced approach to creating git commits with proper attribution and context. It automatically handles the complex workflow of switching git authors, creating meaningful commits, and maintaining project history.

## Basic Usage

```bash
# Save all changes with AI-generated commit message
/save

# Save specific files
/save src/auth.js src/tests/auth.test.js

# Save with custom message
/save --message "Implement JWT refresh token rotation"

# Save without running tests
/save --no-test
```

## Key Features

### 1. Intelligent Commit Messages
- Analyzes your changes to generate descriptive commit messages
- Follows conventional commit format (feat:, fix:, docs:, etc.)
- Includes bullet points for multiple changes
- Adds appropriate context and reasoning

### 2. Automatic Git Author Management
- Temporarily switches to Claude as commit author
- Properly attributes AI assistance with co-author tag
- Automatically restores your original git author
- Handles errors gracefully with fallback restoration

### 3. Change Analysis
- Groups related changes logically
- Identifies the type of change (feature, fix, refactor, etc.)
- Highlights breaking changes
- Includes relevant file paths

### 4. Quality Checks
- Optionally runs tests before committing
- Validates code formatting
- Checks for common issues
- Ensures commit readiness

## Workflow

### What Happens When You Run `/save`

1. **Preparation Phase**
   - Saves current git author configuration
   - Checks for uncommitted changes
   - Analyzes modified files

2. **Analysis Phase**
   - Reviews all changes
   - Identifies patterns and relationships
   - Determines change type and impact
   - Generates commit message

3. **Commit Phase**
   - Switches to Claude git author
   - Stages specified files (or all if none specified)
   - Creates commit with generated message
   - Adds AI attribution

4. **Cleanup Phase**
   - Restores original git author
   - Reports commit success
   - Provides commit hash for reference

## Options

### --message, -m
Provide custom commit message instead of AI-generated:
```bash
/save --message "Fix authentication timeout issue"
```

### --no-test
Skip test execution before commit:
```bash
/save --no-test
```

### --amend
Amend the previous commit:
```bash
/save --amend
```

### --push
Automatically push after successful commit:
```bash
/save --push
```

## Examples

### Basic Save
```bash
/save
# Output:
# 📝 Analyzing changes...
# ✅ Created commit: a1b2c3d
# 
# feat: Add user authentication system
# 
# - Implement JWT token generation
# - Add login/logout endpoints
# - Create user session management
# - Include refresh token support
#
# 🤖 Generated with Claude Code
# Co-Authored-By: Claude <noreply@anthropic.com>
```

### Saving Specific Files
```bash
/save src/api/users.js src/tests/users.test.js
# Only commits the specified files
```

### With Custom Message
```bash
/save -m "hotfix: Resolve critical security vulnerability in auth flow"
# Uses your message but still adds AI attribution
```

## Best Practices

### 1. Review Before Saving
- Check `git status` first
- Ensure related changes are included
- Verify no sensitive data is staged

### 2. Logical Grouping
- Save related changes together
- Split unrelated changes into separate commits
- Keep commits focused and atomic

### 3. Message Customization
- Let AI generate for routine changes
- Provide custom messages for critical fixes
- Always review generated messages

### 4. Testing Integration
- Default behavior runs tests
- Only skip tests when absolutely necessary
- Fix failing tests before committing

## Troubleshooting

### "No changes to save"
- Check `git status` for modified files
- Ensure you're in the correct directory
- Verify files aren't ignored by .gitignore

### "Author restoration failed"
- The command includes automatic fallback
- Manually run: `./scripts/git/git-author-human.sh`
- Check `.claude/git-authors.json` for correct config

### "Tests failed"
- Review test output
- Fix failing tests before committing
- Use `--no-test` only for documentation changes

### "Commit failed"
- Check for merge conflicts
- Ensure repository permissions
- Verify git configuration

## Integration with Other Commands

### With /test
```bash
/test
# If tests pass:
/save
```

### With /format
```bash
/format
/save -m "style: Apply consistent code formatting"
```

### With /review
```bash
/review
# After implementing review suggestions:
/save
```

## Advanced Usage

### Selective Staging
```bash
# Stage specific changes interactively
git add -p
/save
```

### Commit Series
```bash
# First commit
/save src/models/ -m "refactor: Extract user model"

# Second commit  
/save src/controllers/ -m "refactor: Update controllers for new model"

# Third commit
/save src/tests/ -m "test: Update tests for refactored code"
```

### Integration with Hooks
The command respects git hooks:
- Pre-commit hooks run normally
- Commit-msg hooks validate messages
- Post-commit hooks trigger as expected

## Configuration

### Customizing Behavior
Add to `.claude/settings.json`:
```json
{
  "save": {
    "runTests": true,
    "pushAfterCommit": false,
    "signCommits": true,
    "validateMessages": true
  }
}
```

### Git Aliases
Add to `.gitconfig`:
```ini
[alias]
    cs = !claude-code /save
    csm = !claude-code /save -m
```

## Security Considerations

### Sensitive Data
- Never commit secrets or credentials
- Review changes before saving
- Use .gitignore appropriately
- Consider git-secrets or similar tools

### Author Attribution
- Maintains clear record of AI assistance
- Preserves accountability
- Enables tracking of AI-generated code
- Supports compliance requirements

---

*The /save command streamlines the commit process while maintaining best practices for version control and AI attribution.*
