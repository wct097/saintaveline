# GitHub Workflows Best Practices for AI Development (Billing Optimized)

*Version: 2.18.0 - Enhanced with cleanup automation and retention policies*

## Executive Summary

This guide provides **billing-conscious** best practices for creating reliable GitHub Actions workflows in AI-assisted development projects. **Primary focus: Keep workflows under 1 minute (target) and 2 minutes (maximum)** to protect your GitHub Actions minutes budget.

## ⚠️ BILLING OPTIMIZATION PRIORITY ⚠️

**Critical Requirements:**
- **Target runtime: <1 minute per workflow**
- **Maximum runtime: <2 minutes per workflow**
- **Budget protection: 2-3000 minutes/month limit**
- **Strategy: Essential validation only, comprehensive testing weekly/on-release**

## Common GitHub Actions Failure Patterns

### 1. Environment and Dependency Issues

**Common Problems:**
- Missing dependencies or incorrect versions
- Environment variables not properly set
- OS-specific path issues
- Node.js/Python version mismatches

**Prevention:**
```yaml
- name: Setup Node.js
  uses: actions/setup-node@v4
  with:
    node-version: '18'
    cache: 'npm'

- name: Install dependencies
  run: npm ci  # Use ci instead of install for consistency
```

### 2. Permissions and Security Issues

**Common Problems:**
- Insufficient permissions for actions
- Secrets not properly configured
- Write permissions not granted when needed

**Prevention:**
```yaml
permissions:
  contents: read
  issues: write
  pull-requests: write
```

### 3. Conditional Logic Errors

**Common Problems:**
- Incorrect condition syntax
- Matrix builds with inconsistent conditions
- Branch protection conflicts

**Prevention:**
```yaml
- name: Run tests
  if: github.event_name == 'pull_request'
  run: npm test
```

### 4. Artifact and Cache Issues

**Common Problems:**
- Artifacts not properly uploaded/downloaded
- Cache keys not unique enough
- Large artifacts causing timeouts

**Prevention:**
```yaml
- name: Cache dependencies
  uses: actions/cache@v3
  with:
    path: ~/.npm
    key: ${{ runner.os }}-node-${{ hashFiles('**/package-lock.json') }}
    restore-keys: |
      ${{ runner.os }}-node-
```

## GitHub Actions Cleanup and Retention Policies

### Execution History Requirements

**≤5 Execution History Rule**: GitHub Actions workflows should maintain **no more than 5 execution entries** in the Actions history to ensure optimal performance and cost management.

**Rationale:**
- **Performance**: Excessive execution history slows down Actions page loading and API calls
- **Cost Efficiency**: Reduces storage and compute overhead for GitHub infrastructure
- **Clarity**: Keeps recent, relevant execution history visible without clutter
- **Maintenance**: Easier to identify patterns and issues in recent workflow runs

### Retention Policies by Workflow Type

Different workflow types require different retention strategies based on their purpose and frequency:

#### Fast Validation Workflows (Daily/Per-commit)
```yaml
name: Fast Validation
# Retention: 5 executions maximum
# Cleanup frequency: Weekly
# Rationale: High frequency, short-term relevance
```
**Target retention: 5 executions**
- Triggered frequently (every push/PR)
- Results are immediately actionable
- Historical data beyond 5 runs rarely needed
- Quick cleanup cycle prevents accumulation

#### Release Validation Workflows (Per-release)
```yaml
name: Release Validation
# Retention: 10 executions maximum
# Cleanup frequency: Monthly
# Rationale: Lower frequency, higher historical value
```
**Target retention: 10 executions**
- Triggered less frequently (releases/tags)
- Historical release validation data valuable
- Longer retention for compliance and debugging
- Monthly cleanup balances history with performance

#### Weekly Comprehensive Workflows (Scheduled)
```yaml
name: Weekly Comprehensive
# Retention: 8 executions maximum (2 months)
# Cleanup frequency: Bi-weekly
# Rationale: Scheduled runs, trend analysis value
```
**Target retention: 8 executions**
- Scheduled weekly execution
- Trend analysis and pattern detection valuable
- Quarterly review cycle for comprehensive validation
- Bi-weekly cleanup maintains optimal history

### Cleanup Automation Guidelines

#### Using GitHub CLI for Cleanup

**Automated cleanup script:**
```bash
#!/bin/bash
# GitHub Actions cleanup script

REPO="owner/repository"
WORKFLOW_FILE="$1"
RETENTION_COUNT="${2:-5}"

# Get workflow runs older than retention count
OLD_RUNS=$(gh run list \
  --repo="$REPO" \
  --workflow="$WORKFLOW_FILE" \
  --limit=100 \
  --json=databaseId \
  --jq ".[${RETENTION_COUNT}:] | .[].databaseId")

# Delete old runs
for run_id in $OLD_RUNS; do
  echo "Deleting workflow run: $run_id"
  gh run delete "$run_id" --repo="$REPO"
  sleep 1  # Rate limiting
done

echo "Cleanup completed. Retained $RETENTION_COUNT most recent runs."
```

### Integration with `/clean` Command

The `/clean` command includes GitHub Actions cleanup as part of repository maintenance with configurable retention policies and automated execution.

## Performance Optimization for <2 Minute Targets

**Key optimization strategies:**
- Parallel job execution for independent tasks
- Conditional execution based on file changes
- Cached dependencies and validation tools
- Early termination on failure with fail-fast strategy

## Conclusion

These enhanced best practices provide comprehensive foundation for reliable GitHub Actions workflows with proper cleanup automation and retention policies. The combination of billing optimization, performance targets, and automated maintenance ensures sustainable workflow management.

**Key Principles:**
- **Maintain ≤5 execution history** for optimal performance
- **Implement retention policies** based on workflow type and frequency
- **Automate cleanup processes** using GitHub CLI and scheduled workflows
- **Integrate with `/clean` command** for comprehensive repository maintenance
- **Monitor performance** to maintain <2 minute execution targets
