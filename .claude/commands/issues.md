# GitHub Issues Integration Command

## Overview
The `/issues` command provides GitHub Issues integration for project management and bug tracking within Claude Code sessions.

## Usage
```
/issues [action] [options]
```

## Actions

### List Issues
```
/issues list
/issues list --state=open
/issues list --label=bug
/issues list --assignee=@me
```

### Create Issue
```
/issues create --title "Bug in authentication" --body "Description..."
/issues create --title "Feature request" --label=enhancement
```

### Update Issue
```
/issues update #123 --state=closed
/issues update #123 --assignee=username
/issues update #123 --label=bug,priority:high
```

### View Issue
```
/issues view #123
/issues show #123
```

## Integration with Development

### Bug Tracking
When encountering issues during development:
```
/issues create --title "Fix memory leak in user service" --body "Memory usage increases during bulk operations" --label=bug,priority:medium
```

### Feature Requests
For new feature planning:
```
/issues create --title "Add OAuth2 integration" --body "Support for Google and GitHub OAuth" --label=enhancement
```

### Task Management
Convert development tasks to trackable issues:
```
/issues create --title "Refactor database models" --body "Update user model to support new authentication system" --label=refactor
```

## Labels

### Standard Labels
- `bug` - Something isn't working
- `enhancement` - New feature or request
- `documentation` - Improvements or additions to documentation
- `help wanted` - Extra attention is needed
- `question` - Further information is requested

### Priority Labels
- `priority:low` - Low priority
- `priority:medium` - Medium priority
- `priority:high` - High priority
- `priority:critical` - Critical priority

### Type Labels
- `type:feature` - New feature implementation
- `type:bugfix` - Bug fix
- `type:improvement` - Enhancement to existing feature
- `type:refactor` - Code refactoring

## Workflow Integration

### Issue-Driven Development
1. Create issue for work to be done
2. Reference issue in branch name: `git checkout -b fix/123-memory-leak`
3. Reference issue in commits: `fix: resolve memory leak (fixes #123)`
4. Close issue automatically with commit or PR

### Code Review Integration
Link issues to pull requests:
```
This PR addresses #123 and #124
Closes #125
```

### Project Planning
Use issues for sprint planning:
- Create milestone for sprint
- Assign issues to milestone
- Track progress through issue status

## Best Practices

### Issue Creation
- Use clear, descriptive titles
- Provide detailed descriptions
- Add relevant labels
- Assign to appropriate person
- Link to related issues

### Issue Management
- Keep issues updated with progress
- Close issues when resolved
- Use issue comments for discussion
- Reference commits and PRs

### Team Collaboration
- Use @mentions for notifications
- Assign issues appropriately
- Use project boards for visualization
- Regular triage and cleanup

## Configuration

### GitHub CLI Setup
Ensure GitHub CLI is configured:
```bash
gh auth login
gh repo set-default
```

### Repository Access
Verify repository permissions:
- Issues read/write access
- Repository contributor access
- Proper authentication tokens

## Examples

### Bug Report
```
/issues create --title "Login fails with special characters in password" --body "When password contains symbols like @#$, login request returns 400 error. Steps to reproduce: 1. Create account with complex password, 2. Attempt login, 3. Observe 400 response." --label=bug,priority:high
```

### Feature Request
```
/issues create --title "Add dark mode support" --body "Users have requested dark mode theme support. Should include: - Toggle in settings, - Dark color scheme, - Persistence across sessions" --label=enhancement,ui
```

### Task Creation
```
/issues create --title "Update dependencies to latest versions" --body "Audit and update all dependencies to address security vulnerabilities and get latest features" --label=maintenance
```

---

*GitHub Issues integration helps maintain organized project management and ensures important tasks don't get lost during development.*
