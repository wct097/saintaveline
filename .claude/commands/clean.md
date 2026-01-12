# Repository Maintenance and Validation Command (v2.40.0)

## Overview

The `/clean` command provides comprehensive repository cleanup and validation specifically designed for the ai-setup repository maintenance workflow.

## Key Features

- **Structure compliance** - Ensures repository adheres to defined architecture
- **Documentation currency** - Updates all version references and cross-references
- **Test validation** - Runs all GitHub Actions and validation hooks
- **Git health** - Ensures clean working tree and synchronized branches
- **Automatic archiving** - Moves outdated content to appropriate archive locations

## Usage Patterns

```bash
# Full cleanup and validation
/clean

# Specific operations
/clean --structure-only    # Repository structure compliance only
/clean --docs-only        # Documentation updates only  
/clean --tests-only       # Validation and testing only
/clean --dry-run          # Preview changes without executing
```

## What It Does

### 1. Repository Structure Validation
- Verifies required directories exist
- Removes test artifacts and temporary files
- Ensures consistent file organization
- Archives outdated content appropriately

### 2. Documentation Updates
- Synchronizes version numbers across all files
- Updates cross-references between documents
- Validates markdown link integrity
- Ensures README files are current

### 3. Test Execution
- Runs validation hooks
- Executes GitHub Actions locally
- Verifies deployment script integrity
- Checks for security vulnerabilities

### 4. Git Repository Health
- Ensures clean working tree
- Synchronizes with remote branches
- Validates commit history
- Removes orphaned branches

## Success Criteria

The command ensures the repository meets these standards:

1. ✅ **Repository Structure**: Matches defined architecture
2. ✅ **Documentation**: All versions current, accurate content
3. ✅ **Tests Passing**: All validations successful
4. ✅ **Git Status**: Clean working tree, synchronized
5. ✅ **Deploy Script**: Current version, validated

## Integration with Deploy Script Maintenance

The `/clean` command is essential for ai-setup repository maintenance:
- **Pre-release validation** - Ensures repository is clean before updating deploy script
- **Version synchronization** - Updates all version references when deploy script version changes
- **Quality assurance** - Validates that all GitHub Actions pass before release
- **Structure enforcement** - Removes development artifacts and maintains clean template structure

## Options

### --structure-only
Focuses on repository structure:
- Validates directory layout
- Removes temporary files
- Archives old content
- Organizes specifications

### --docs-only
Updates documentation:
- Synchronizes versions
- Fixes broken links
- Updates timestamps
- Refreshes examples

### --tests-only
Runs all validations:
- Validation hooks
- GitHub Actions
- Security scans
- Deploy script tests

### --dry-run
Preview mode:
- Shows what would change
- No actual modifications
- Safe for review
- Generates report

## Best Practices

### Regular Maintenance
```bash
# Weekly maintenance
/clean

# Before releases
/clean --tests-only

# After major changes
/clean --structure-only
```

### Pre-Commit Workflow
```bash
# Before committing deploy script updates
/clean
git add .
git commit -m "Release v2.x.x"
```

### CI/CD Integration
The command can be integrated into automated workflows:
```yaml
- name: Repository Cleanup
  run: |
    /clean --dry-run
    /clean
```

## Troubleshooting

### Common Issues

**"Working tree not clean"**
```bash
# Stash or commit changes first
git stash
/clean
git stash pop
```

**"Tests failing"**
```bash
# Run specific test category
/clean --tests-only

# Check test output
cat logs/validation.log
```

**"Version mismatch"**
```bash
# Force version update
/clean --docs-only

# Verify version
grep VERSION scripts/deploy-ai-setup.sh
```

## What Gets Cleaned

### Temporary Files
- `*.tmp`, `*.log`, `*.bak`
- Build artifacts
- Test outputs
- Cache files

### Development Artifacts
- TODO comments
- Debug statements
- Test scaffolding
- Deprecated files

### Outdated Content
- Old specifications → archive
- Deprecated commands → removed
- Stale documentation → updated
- Unused scripts → cleaned

## Validation Checks

### Structure Validation
- Required directories present
- No unauthorized files in template directories
- Consistent naming conventions
- Proper file permissions

### Documentation Validation
- Version numbers synchronized
- No broken internal links
- Updated timestamps
- Complete examples

### Code Quality
- No prohibited comment patterns
- Proper error handling
- Security best practices
- Performance optimizations

## Output

The command provides detailed feedback:

```
🧹 AI Setup Repository Cleanup
==============================

📁 Structure Check... ✅
📚 Documentation... ✅ (3 files updated)
🧪 Tests... ✅ (all passing)
🌿 Git Status... ✅ (clean)
📦 Deploy Script... ✅ (v2.37.0)

✨ Repository is clean and ready!
```

## Advanced Usage

### Custom Validations
Add custom checks in:
```
scripts/validation/custom-checks.sh
```

### Exclude Patterns
Configure exclusions:
```
.cleanignore
```

### Hook Integration
Pre-clean hooks:
```
scripts/hooks/pre-clean.sh
```

---

*The /clean command maintains repository health and ensures consistent, high-quality templates for AI development setup.*
