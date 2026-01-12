# Project Context for AI Tools

*[Copy this template to `project_context.md` and customize for your specific project]*

## Project Overview

**Project Name**: [Your Project Name]  
**Description**: [Brief description of what the project does]  
**Primary Goal**: [Main objective or problem being solved]  
**Target Users**: [Who will use this project]  
**Current Phase**: [Planning / Development / Testing / Production]

## Technology Stack

### Frontend
- **Framework**: [e.g., React, Vue, Angular, Next.js]
- **State Management**: [e.g., Redux, Zustand, Context API]
- **Styling**: [e.g., Tailwind CSS, styled-components, SCSS]
- **Build Tool**: [e.g., Vite, Webpack, Parcel]
- **Testing**: [e.g., Jest, React Testing Library, Cypress]

### Backend
- **Runtime**: [e.g., Node.js, Python, Go, Java]
- **Framework**: [e.g., Express, FastAPI, Gin, Spring Boot]
- **Database**: [e.g., PostgreSQL, MongoDB, MySQL, Redis]
- **ORM/ODM**: [e.g., Prisma, Sequelize, SQLAlchemy, Mongoose]
- **API Style**: [e.g., REST, GraphQL, gRPC]

### Infrastructure
- **Hosting**: [e.g., AWS, Google Cloud, Azure, Vercel, Netlify]
- **Container**: [e.g., Docker, Kubernetes]
- **CI/CD**: [e.g., GitHub Actions, GitLab CI, Jenkins]
- **Monitoring**: [e.g., DataDog, New Relic, Prometheus]

### Development Tools
- **Version Control**: [e.g., Git with GitHub/GitLab/Bitbucket]
- **Package Manager**: [e.g., npm, yarn, pnpm, pip, go mod]
- **Code Quality**: [e.g., ESLint, Prettier, Black, golangci-lint]
- **Documentation**: [e.g., JSDoc, Sphinx, Swagger/OpenAPI]

## Project Structure

```
project-root/
├── src/                    # [Main source code]
│   ├── components/         # [UI components]
│   ├── pages/             # [Page components/routes]
│   ├── services/          # [API/business logic]
│   ├── utils/             # [Helper functions]
│   └── types/             # [TypeScript types/interfaces]
├── tests/                  # [Test files]
├── docs/                   # [Documentation]
├── scripts/               # [Build/deployment scripts]
└── config/                # [Configuration files]
```

*[Customize the structure above to match your project]*

## Key Architecture Decisions

### Design Patterns
- **Architecture Pattern**: [e.g., MVC, MVVM, Clean Architecture, Microservices]
- **State Management Pattern**: [e.g., Flux, Event Sourcing, CQRS]
- **API Design**: [e.g., RESTful principles, GraphQL schema-first]
- **Database Design**: [e.g., Normalized relational, Document-based, Key-value]

### Code Organization Principles
- [e.g., Feature-based modules]
- [e.g., Separation of concerns]
- [e.g., Domain-driven design]
- [e.g., Repository pattern for data access]

### Security Considerations
- **Authentication**: [e.g., JWT, OAuth2, Session-based]
- **Authorization**: [e.g., RBAC, ABAC, Policy-based]
- **Data Protection**: [e.g., Encryption at rest, TLS, Input validation]
- **Security Headers**: [e.g., CSP, CORS configuration]

## Development Workflow

### Branch Strategy
- **Main Branch**: [e.g., main/master - production ready]
- **Development Branch**: [e.g., develop - integration branch]
- **Feature Branches**: [e.g., feature/ticket-description]
- **Release Process**: [e.g., release/v1.2.3]

### Code Review Process
1. [e.g., Create feature branch from develop]
2. [e.g., Implement feature with tests]
3. [e.g., Create pull request with description]
4. [e.g., Automated tests must pass]
5. [e.g., Peer review required]
6. [e.g., Merge after approval]

### Testing Strategy
- **Unit Tests**: [Coverage target, key areas]
- **Integration Tests**: [API endpoints, database operations]
- **E2E Tests**: [Critical user flows]
- **Performance Tests**: [Load testing thresholds]

## Coding Standards

### General Principles
- **Code Style**: [e.g., Airbnb JavaScript Style Guide]
- **Naming Conventions**: 
  - Variables: [e.g., camelCase]
  - Functions: [e.g., camelCase, descriptive verbs]
  - Classes: [e.g., PascalCase]
  - Constants: [e.g., UPPER_SNAKE_CASE]
  - Files: [e.g., kebab-case.js]

### TypeScript/JavaScript Specific
```typescript
// Example of preferred patterns
// [Add your specific examples]

// Good
export const calculateTotalPrice = (items: CartItem[]): number => {
  return items.reduce((total, item) => total + item.price * item.quantity, 0);
};

// Avoid
export function calc(i: any) {
  let t = 0;
  for(let x of i) t += x.p * x.q;
  return t;
}
```

### Error Handling
```typescript
// Preferred error handling pattern
try {
  const result = await riskyOperation();
  return { success: true, data: result };
} catch (error) {
  logger.error('Operation failed', { error, context });
  return { success: false, error: error.message };
}
```

### Documentation Standards
```typescript
/**
 * Calculates the total price of items in the cart
 * @param items - Array of cart items with price and quantity
 * @returns Total price including tax and discounts
 * @example
 * const total = calculateTotalPrice([
 *   { id: '1', price: 10.00, quantity: 2 },
 *   { id: '2', price: 5.50, quantity: 1 }
 * ]); // Returns 25.50
 */
```

## API Conventions

### RESTful Endpoints
- **GET** `/api/resources` - List resources
- **GET** `/api/resources/:id` - Get specific resource
- **POST** `/api/resources` - Create new resource
- **PUT** `/api/resources/:id` - Update resource
- **PATCH** `/api/resources/:id` - Partial update
- **DELETE** `/api/resources/:id` - Delete resource

### Response Format
```json
{
  "success": true,
  "data": { },
  "meta": {
    "timestamp": "2023-12-01T12:00:00Z",
    "version": "1.0.0"
  }
}
```

### Error Response Format
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid input provided",
    "details": []
  }
}
```

## Database Schema

### Key Entities
*[Describe your main database entities and relationships]*

```sql
-- Example: Users table
CREATE TABLE users (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  email VARCHAR(255) UNIQUE NOT NULL,
  username VARCHAR(50) UNIQUE NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Add your actual schema here
```

### Data Access Patterns
- [e.g., Use repository pattern for data access]
- [e.g., Implement caching for frequently accessed data]
- [e.g., Use database transactions for multi-step operations]

## Environment Configuration

### Required Environment Variables
```bash
# Application
NODE_ENV=development|production
PORT=3000

# Database
DATABASE_URL=postgresql://user:pass@host:port/dbname

# Authentication
JWT_SECRET=your-secret-key
JWT_EXPIRY=7d

# External Services
API_KEY_SERVICE_NAME=your-api-key

# Add your actual environment variables
```

### Configuration Files
- **Development**: `.env.development`
- **Production**: `.env.production`
- **Testing**: `.env.test`

## Current Challenges & Focus Areas

### Technical Debt
1. [e.g., Legacy authentication system needs refactoring]
2. [e.g., Database queries need optimization]
3. [e.g., Test coverage below 70% target]

### Performance Considerations
- [e.g., Page load time should be under 3 seconds]
- [e.g., API response time under 200ms]
- [e.g., Support 1000 concurrent users]

### Upcoming Features
1. [e.g., Real-time notifications]
2. [e.g., Advanced search functionality]
3. [e.g., Mobile app integration]

## Team Conventions

### Communication
- **Documentation**: [Where and how to document decisions]
- **Code Comments**: [When and how to comment code]
- **Commit Messages**: [Format and conventions]
- **PR Descriptions**: [Required information]

### Definition of Done
- [ ] Code implements the required functionality
- [ ] Unit tests written and passing
- [ ] Integration tests updated if needed
- [ ] Documentation updated
- [ ] Code reviewed and approved
- [ ] No decrease in test coverage
- [ ] Performance impact assessed

## AI Assistant Guidelines

### How AI Should Help
1. **Follow existing patterns** in the codebase
2. **Suggest improvements** that align with our standards
3. **Write tests** for any new functionality
4. **Document complex logic** thoroughly
5. **Consider performance** implications
6. **Maintain consistency** with existing code style

### What to Avoid
- Don't introduce new dependencies without discussion
- Don't change established patterns without justification
- Don't skip error handling or validation
- Don't ignore security considerations
- Don't write overly complex solutions

## Additional Context

*[Add any other project-specific information that would help AI assistants understand your project better, such as:]*

- Business domain knowledge
- Integration points with other systems
- Compliance requirements
- Performance benchmarks
- User experience priorities
- Technical constraints
- Historical decisions and their rationale

---

*Last Updated: [Date]*  
*Maintained by: [Team/Person Name]*

*This document should be updated whenever significant architectural decisions are made or development practices change.*
