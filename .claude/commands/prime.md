# Prime Command
<!-- AI-SETUP-SCRIPT-TAG: v2.39.0 -->

## Project Context Display

Display comprehensive project context through organized information gathering and presentation. This command provides context through pure information display without taking actions or managing files.

### Operation

The `/prime` command operates in single mode, gathering and displaying project context across multiple dimensions:

**Project Structure Context:**
- Current directory structure and file organization
- Technology stack identification (package.json, requirements.txt, etc.)
- Configuration files and project settings
- Key directories and their purposes

**Development State Context:**
- Current git branch and repository status
- Recent commit history and development activity
- Work in progress and unstaged changes
- Project version and build information

**Documentation Context:**
- README.md content and project overview
- AI development documentation (ai_docs/project_context.md)
- Specification files and requirements
- Code conventions and development guidelines

**Session Context:**
- Previous session work and decisions (if available)
- Outstanding tasks and development patterns
- Recent conversation history and context
- Development continuity information

### Context Display Format

Information is presented in organized sections:

1. **Project Overview**: Name, technology stack, current branch
2. **Recent Activity**: Last commits, current changes, work in progress
3. **Documentation Summary**: Key project information and conventions
4. **Session Continuity**: Previous work and outstanding tasks (if applicable)
5. **Development Environment**: Configuration, dependencies, tools

### Boundaries and Constraints

**This command only displays information. It does not:**
- Create, modify, or delete files
- Execute commands or scripts
- Update todo lists or project state
- Manage git repository or commits
- Install dependencies or run builds
- Make configuration changes

**Context sources are read-only:**
- File system exploration for structure
- Git history and status reading
- Documentation content review
- Session history access (if available)
- Configuration file parsing

### Git Author Verification

**CRITICAL**: The /prime command MUST verify and display the current git author configuration to prevent commit attribution errors.

When loading context, the AI must:
1. **Check current git configuration** - Read git user.name and user.email
2. **Verify git-authors.json** - If exists, read the stored human author configuration
3. **Display author information clearly** - Show who will be used for commits
4. **Request confirmation** - Ask the user to verify the author information is correct

### AI Behavior Instructions

When this command is executed:
1. **Load project context** - Gather all relevant project information
2. **Verify git authors** - Check and display current git author configuration
3. **Display author confirmation** - Clearly show the human author that will be used for commits
4. **Wait for confirmation** - Ask user to confirm or correct the author information
5. **No autonomous actions** - This is a context-loading command only; take no actions beyond loading information

### Example Response
After running /prime, the AI should respond:
```
✓ Project context loaded. I understand your project structure, recent changes, and development patterns.

📝 Git Author Configuration:
   Current git user: John Doe <john.doe@example.com>
   This is the identity I'll use when switching back to human author after AI commits.
   
   ⚠️ Please confirm this is correct. If not, please run:
      git config user.name "Your Name"
      git config user.email "your.email@example.com"
   
What would you like to work on?
```

### Communication Guidelines

After context display, maintain professional development communication:

- **Technical focus**: Emphasize accuracy and practical solutions
- **Direct feedback**: Provide honest assessment of approaches and issues
- **Constructive challenge**: Ask clarifying questions and suggest alternatives
- **Measured responses**: Avoid excessive validation or praise
- **Substance over style**: Prioritize technical merit and problem-solving

### Usage Pattern

```
/prime
```

Single command execution provides comprehensive project context through organized information display. No parameters required - the command automatically adapts to the current project structure and available information sources.

The command establishes context foundation for productive development sessions without file system modifications or state changes.
