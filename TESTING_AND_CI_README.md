# Testing and CI/CD Documentation

Comprehensive guide for testing and continuous integration/deployment for EventPlatform.

## 🧪 Testing Strategy

### Backend Testing (.NET)

#### Unit Tests
- **Location**: `EventPlatform.API.Tests/Controllers/`
- **Framework**: MSTest
- **Coverage**: Controllers, Services, Models
- **Database**: In-memory SQLite for testing

#### Integration Tests
- **Location**: `EventPlatform.API.Tests/Integration/`
- **Framework**: MSTest with WebApplicationFactory
- **Coverage**: Full API endpoints
- **Database**: In-memory database

#### Running Backend Tests
```bash
# Run all tests
cd EventPlatform.API.Tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test category
dotnet test --filter "Category=Integration"

# Run specific test
dotnet test --filter "FullyQualifiedName~GetEvents_ReturnsAllEvents"
```

### Frontend Testing (Angular)

#### Unit Tests
- **Location**: `EventPlatform.Frontend/src/app/`
- **Framework**: Jasmine + Karma
- **Coverage**: Services, Components, Models

#### E2E Tests
- **Location**: `EventPlatform.Frontend/e2e/`
- **Framework**: Playwright (recommended) or Cypress
- **Coverage**: User workflows

#### Running Frontend Tests
```bash
# Run unit tests
cd EventPlatform.Frontend
npm run test

# Run with coverage
npm run test -- --code-coverage

# Run E2E tests
npm run e2e

# Run tests in watch mode
npm run test:watch
```

## 🚀 CI/CD Pipeline

### Backend CI/CD (.NET)

#### Workflow: `.github/workflows/backend-ci.yml`

**Triggers**:
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop`
- Changes in `EventPlatform.API/` directory

**Jobs**:
1. **Build and Test**
   - Restore dependencies
   - Build application
   - Run unit tests with coverage
   - Run integration tests

2. **Security Scan**
   - Check for vulnerable packages
   - Security audit

3. **Docker Build** (main branch only)
   - Build Docker image
   - Push to Docker Hub

4. **Deploy Staging** (main branch only)
   - Deploy to staging environment

5. **Deploy Production** (main branch only)
   - Deploy to production environment

### Frontend CI/CD (Angular)

#### Workflow: `.github/workflows/frontend-ci.yml`

**Triggers**:
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop`
- Changes in `EventPlatform.Frontend/` directory

**Jobs**:
1. **Build and Test**
   - Install dependencies
   - Run linting
   - Run unit tests with coverage
   - Build for production

2. **E2E Tests**
   - Run end-to-end tests

3. **Security Scan**
   - npm audit for vulnerabilities

4. **Docker Build** (main branch only)
   - Build Docker image
   - Push to Docker Hub

5. **Deploy Staging** (main branch only)
   - Deploy to staging environment

6. **Deploy Production** (main branch only)
   - Deploy to production environment

## 🐳 Docker Configuration

### Backend Dockerfile
```dockerfile
# Multi-stage build for .NET 9 API
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# ... (see EventPlatform.API/Dockerfile)
```

### Frontend Dockerfile
```dockerfile
# Multi-stage build for Angular app
FROM node:18-alpine AS base
FROM nginx:alpine
# ... (see EventPlatform.Frontend/Dockerfile)
```

### Docker Compose
```yaml
# Complete development environment
version: '3.8'
services:
  mysql: # Database
  backend: # .NET API
  frontend: # Angular app
  redis: # Caching (future)
```

## 📊 Code Coverage

### Backend Coverage
- **Target**: >80% line coverage
- **Tools**: Coverlet + Codecov
- **Reports**: Generated in `EventPlatform.API.Tests/coverage/`

### Frontend Coverage
- **Target**: >70% line coverage
- **Tools**: Karma + Istanbul
- **Reports**: Generated in `EventPlatform.Frontend/coverage/`

## 🔒 Security Testing

### Backend Security
- **Package Vulnerabilities**: `dotnet list package --vulnerable`
- **Dependency Scanning**: GitHub Dependabot
- **Code Analysis**: SonarQube integration

### Frontend Security
- **Package Vulnerabilities**: `npm audit`
- **Dependency Scanning**: GitHub Dependabot
- **Content Security Policy**: Configured in nginx

## 🧪 Test Data Management

### Backend Test Data
```csharp
// Seed test data in integration tests
private void SeedTestData(ApplicationDbContext context)
{
    var testEvents = new List<Event>
    {
        new Event { /* test data */ }
    };
    context.Events.AddRange(testEvents);
    context.SaveChanges();
}
```

### Frontend Test Data
```typescript
// Mock data for unit tests
const mockEvents: Event[] = [
  {
    id: 1,
    title: 'Test Conference',
    // ... other properties
  }
];
```

## 🚀 Deployment Environments

### Development
- **Backend**: `http://localhost:5130`
- **Frontend**: `http://localhost:4200`
- **Database**: Local MySQL or Docker

### Staging
- **Backend**: `https://api-staging.eventplatform.com`
- **Frontend**: `https://staging.eventplatform.com`
- **Database**: Staging MySQL instance

### Production
- **Backend**: `https://api.eventplatform.com`
- **Frontend**: `https://eventplatform.com`
- **Database**: Production MySQL cluster

## 📋 Testing Checklist

### Before Committing
- [ ] All unit tests pass
- [ ] Integration tests pass
- [ ] Code coverage meets targets
- [ ] No linting errors
- [ ] Security scan passes

### Before Deployment
- [ ] All CI/CD pipelines pass
- [ ] E2E tests pass
- [ ] Performance tests pass
- [ ] Security audit passes
- [ ] Documentation updated

## 🔧 Troubleshooting

### Common Test Issues

#### Backend Tests
```bash
# Clear test cache
dotnet clean
dotnet test --no-build

# Reset test database
dotnet ef database drop --force
dotnet ef database update
```

#### Frontend Tests
```bash
# Clear test cache
npm run test:clean

# Update test dependencies
npm update @types/jasmine @types/karma

# Run tests with verbose output
npm run test -- --verbose
```

### CI/CD Issues

#### Pipeline Failures
1. Check GitHub Actions logs
2. Verify environment variables
3. Test locally with same Node.js/.NET versions
4. Check Docker build context

#### Deployment Issues
1. Verify environment configuration
2. Check database connectivity
3. Validate API endpoints
4. Test frontend-backend integration

## 📚 Additional Resources

- [.NET Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/)
- [Angular Testing Guide](https://angular.dev/guide/testing)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)

---

**Last Updated**: January 2024  
**Maintained by**: EventPlatform Development Team 