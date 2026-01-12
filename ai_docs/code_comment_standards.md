# Code Comment Standards for AI Development

*Version: 2.18.0 - Professional commenting guidelines for AI-assisted development*

## Philosophy

**Professional code comments enhance understanding without stating the obvious.** Comments should explain *why* and *how*, not *what*. Maintain a **12-14% comment-to-code ratio** for optimal documentation density.

## Prohibited Comment Patterns

### Development-Phase Comments (Forbidden)
```bash
# TODO: fix this later
# FIXME: temporary solution  
# HACK: quick fix
# XXX: broken code
# BUG: doesn't work properly
# TEMP: temporary code
# DEBUG: remove before commit
```

### Low-Value Comments (Eliminate)
```bash
# Setup
# Initialize variables
# Loop through items
# End of function
# Return result
echo "hello"  # prints hello
```

### Development Process Meta-Commentary (Remove)
```bash
# Phase 1 implementation
# Added in version 2.0
# Modified by John on 3/15
# Fixed bug #123
```

## Professional Comment Standards

### File Headers (Required)
```bash
#!/bin/bash
# validation_orchestrator.sh - Coordinates validation pipeline execution
# Part of AI Setup Enhanced Validation Framework
# Version: 2.18.0
```

### Function Documentation (Required for Complex Functions)
```bash
# Enhanced logging for Claude integration with comprehensive error handling
log_claude() {
    local level="$1"
    local message="$2"
    # Implementation follows...
}
```

### Section Organization (Recommended)
```bash
### Phase 1: Context Loading
# Load project context and initialize validation state

### Configuration Management  
# Handle validation configuration parsing and validation
```

### Explanatory Comments (Encourage)
```bash
# Use timeout to prevent infinite hangs during deployment testing
timeout 45s bash test-deployment.sh

# Preserve important runs from the last 7 days regardless of count
if [[ $run_age -lt 604800 ]]; then
```

## Good Comment Examples

### Clear Purpose Documentation
```bash
# Claude Code integration configuration with validation hooks
setup_claude_integration() {
```

### Technical Context
```bash
# Windows line endings cause syntax errors in bash validation
sed -i 's/\r$//' "$script_file"
```

### Business Logic Explanation
```bash
# Fast validation: 5 runs, Release: 10 runs, Weekly: 8 runs
configure_retention_policy() {
```

## Comment Quality Validation

### Automated Quality Checks
- Pattern detection for prohibited comment types
- Comment-to-code ratio analysis (target: 12-14%)
- Function documentation coverage requirements
- Integration with pre-commit hooks and CI/CD

### Quality Scoring System
- **EXCELLENT (90-100%)**: Professional documentation, appropriate density
- **GOOD (80-89%)**: Minor improvements needed
- **FAIR (70-79%)**: Moderate cleanup required  
- **POOR (60-69%)**: Significant violations present
- **FAILING (<60%)**: Unacceptable comment quality

## AI Development Integration

### Claude Code Comment Guidelines
- Explain complex Claude Code command logic
- Document AI workflow integration points
- Clarify context preservation mechanisms
- Avoid obvious AI tool operation comments

### Multi-Agent Pattern Documentation
```bash
# Deploy parallel agents for comprehensive project analysis
# Agent 1: Project structure, Agent 2: Recent activity, Agent 3: Documentation
```

### Session Context Comments
```bash
# Automatic session integration loads previous context without user prompts
# Enhances Claude understanding of recent work and development patterns
```

## Enforcement and Validation

### Pre-commit Hook Integration
- Validates comment quality before commits
- Blocks commits with prohibited patterns
- Provides specific guidance for violations
- Supports emergency bypass for critical fixes

### Continuous Integration
- GitHub Actions workflow validates comment quality
- PR comments report violations and suggestions
- Automated scoring and quality metrics
- Integration with existing validation framework

### `/clean` Command Integration
- Comment quality validation during repository cleanup
- Automatic detection and reporting of violations
- Integration with comprehensive repository maintenance

## Implementation Guidelines

### Comment Review Process
1. **Write**: Focus on why and how, not what
2. **Review**: Ensure comments add value beyond code
3. **Validate**: Run automated quality checks
4. **Maintain**: Update comments when code changes

### Quality Maintenance
- Regular comment audits during code reviews
- Automated enforcement prevents regression
- Integration with AI development workflows
- Continuous improvement based on team feedback

## Success Metrics

### Quality Indicators
- **Zero prohibited patterns** in codebase
- **12-14% comment-to-code ratio** maintained
- **80%+ function documentation** for complex functions
- **Professional quality scores** (80%+ average)

### Enforcement Effectiveness
- **100% pre-commit validation** coverage
- **Automated CI/CD integration** functional
- **Developer productivity** maintained or improved
- **Code maintainability** enhanced through clear documentation

## Conclusion

Professional code comment standards enhance code maintainability while avoiding development artifacts and low-value documentation. The combination of clear guidelines, automated enforcement, and AI development integration ensures sustainable code quality.

**Core Principles:**
- **Explain why and how, not what**
- **Maintain 12-14% comment-to-code ratio**
- **Eliminate development-phase artifacts**
- **Integrate with AI development workflows**
- **Enforce through automation and validation**
- **Focus on professional, value-added documentation**

This standard supports both individual developer productivity and team collaboration in AI-assisted development environments.
