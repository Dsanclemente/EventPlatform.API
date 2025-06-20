# EventPlatform Technical Documentation

Comprehensive technical documentation for the EventPlatform full-stack application.

## 🏗️ System Architecture

### Overview
EventPlatform is a modern full-stack web application built with a microservices-inspired architecture:

```
┌─────────────────┐    HTTP/REST    ┌─────────────────┐
│   Angular 18    │ ◄─────────────► │   .NET 9 API    │
│   Frontend      │                 │   Backend       │
└─────────────────┘                 └─────────────────┘
                                              │
                                              │ Entity Framework
                                              ▼
                                    ┌─────────────────┐
                                    │   MySQL 8.0+    │
                                    │   Database      │
                                    └─────────────────┘
```

### Technology Stack

#### Frontend (Angular 18)
- **Framework**: Angular 18 with Server-Side Rendering (SSR)
- **Language**: TypeScript 5.0+
- **Styling**: SCSS with modern CSS Grid and Flexbox
- **State Management**: Angular Services and Reactive Forms
- **HTTP Client**: Angular HttpClient with interceptors
- **Routing**: Angular Router with lazy loading support

#### Backend (.NET 9)
- **Framework**: .NET 9 Web API
- **Language**: C# 12
- **ORM**: Entity Framework Core 9
- **Database**: MySQL 8.0+
- **Documentation**: Swagger/OpenAPI 3.0
- **Validation**: Data Annotations and Fluent Validation

## 📊 Data Model

### Event Entity
```csharp
public class Event
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public DateTime DateTime { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public int Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

### Event Status Enumeration
```csharp
public enum EventStatus
{
    Upcoming = 0,
    Attending = 1,
    Maybe = 2,
    Declined = 3
}
```

### API Response Model
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
}
```

## 🔌 API Endpoints

### Base Configuration
- **Base URL**: `http://localhost:5130/api/events`
- **Content-Type**: `application/json`
- **Authentication**: None (public API)

### Endpoint Specifications

#### 1. GET /api/events
**Purpose**: Retrieve all events with optional filtering

**Query Parameters**:
```typescript
interface EventFilters {
  title?: string;      // Filter by title (case-insensitive)
  location?: string;   // Filter by location (case-insensitive)
  dateFrom?: string;   // Filter from date (ISO 8601)
  dateTo?: string;     // Filter to date (ISO 8601)
}
```

**Response**:
```typescript
interface ApiResponse<Event[]> {
  success: boolean;
  message: string;
  data: Event[];
  errors: string[];
}
```

**Example Request**:
```bash
GET /api/events?title=conference&dateFrom=2024-01-01&dateTo=2024-12-31
```

#### 2. GET /api/events/{id}
**Purpose**: Retrieve a specific event by ID

**Path Parameters**:
- `id` (int): Event ID

**Response**: Single Event object

**Error Handling**:
- 404: Event not found
- 400: Invalid ID format

#### 3. POST /api/events
**Purpose**: Create a new event

**Request Body**:
```typescript
interface CreateEventRequest {
  title: string;           // Required, max 100 chars
  dateTime: string;        // Required, ISO 8601 format
  location: string;        // Required, max 200 chars
  description?: string;    // Optional, max 1000 chars
  status: number;          // Required, 0-3
}
```

**Validation Rules**:
- Title: Required, 1-100 characters
- DateTime: Required, valid future date
- Location: Required, 1-200 characters
- Description: Optional, max 1000 characters
- Status: Required, valid enum value (0-3)

#### 4. PUT /api/events/{id}
**Purpose**: Update an existing event

**Request Body**: Same as POST with additional `id` field

**Validation**: Same as POST + ID existence check

#### 5. PATCH /api/events/{id}/status
**Purpose**: Update only the event status

**Request Body**:
```typescript
interface UpdateStatusRequest {
  status: number;  // Required, 0-3
}
```

#### 6. DELETE /api/events/{id}
**Purpose**: Delete an event

**Response**: Success message with no data

#### 7. POST /api/events/generate-description
**Purpose**: Generate AI-powered description

**Request Body**:
```typescript
interface GenerateDescriptionRequest {
  topic: string;  // Required, event topic
}
```

**Response**: Generated description text

## 🎨 Frontend Architecture

### Component Structure
```
app/
├── components/
│   └── events/
│       ├── event-list/
│       │   ├── event-list.component.ts
│       │   ├── event-list.component.html
│       │   └── event-list.component.scss
│       ├── event-detail/
│       │   ├── event-detail.component.ts
│       │   ├── event-detail.component.html
│       │   └── event-detail.component.scss
│       └── event-form/
│           ├── event-form.component.ts
│           ├── event-form.component.html
│           └── event-form.component.scss
├── services/
│   └── event.service.ts
├── models/
│   └── event.model.ts
└── app.routes.ts
```

### Service Layer

#### EventService
```typescript
@Injectable({
  providedIn: 'root'
})
export class EventService {
  private apiUrl = 'http://localhost:5130/api/events';

  // CRUD Operations
  getEvents(filters?: EventFilters): Observable<ApiResponse<Event[]>>
  getEvent(id: number): Observable<ApiResponse<Event>>
  createEvent(event: CreateEventRequest): Observable<ApiResponse<Event>>
  updateEvent(id: number, event: UpdateEventRequest): Observable<ApiResponse<Event>>
  deleteEvent(id: number): Observable<ApiResponse<void>>
  
  // Status Management
  updateEventStatus(id: number, status: number): Observable<ApiResponse<Event>>
  
  // AI Features
  generateDescription(topic: string): Observable<ApiResponse<string>>
}
```

### Component Communication

#### Event Flow
1. **EventListComponent** → **EventService** → **API**
2. **EventDetailComponent** → **EventService** → **API**
3. **EventFormComponent** → **EventService** → **API**

#### Data Flow
```
User Action → Component → Service → HTTP Request → API → Database
     ↑                                                           ↓
     └────────────── Response ← Service ← HTTP Response ←────────┘
```

## 🎨 UI/UX Design System

### Color Palette
```scss
// Primary Colors
$primary-color: #3b82f6;
$primary-dark: #1d4ed8;
$primary-light: #60a5fa;

// Secondary Colors
$secondary-color: #10b981;
$secondary-dark: #059669;
$secondary-light: #34d399;

// Status Colors
$status-upcoming: #f59e0b;
$status-attending: #10b981;
$status-maybe: #3b82f6;
$status-declined: #ef4444;

// Neutral Colors
$background-color: #f8fafc;
$surface-color: #ffffff;
$text-primary: #1e293b;
$text-secondary: #64748b;
```

### Typography
```scss
// Font Families
$font-family-primary: 'Inter', -apple-system, BlinkMacSystemFont, sans-serif;
$font-family-mono: 'JetBrains Mono', 'Fira Code', monospace;

// Font Sizes
$font-size-xs: 0.75rem;    // 12px
$font-size-sm: 0.875rem;   // 14px
$font-size-base: 1rem;     // 16px
$font-size-lg: 1.125rem;   // 18px
$font-size-xl: 1.25rem;    // 20px
$font-size-2xl: 1.5rem;    // 24px
$font-size-3xl: 1.875rem;  // 30px
```

### Spacing System
```scss
// Spacing Scale
$spacing-1: 0.25rem;   // 4px
$spacing-2: 0.5rem;    // 8px
$spacing-3: 0.75rem;   // 12px
$spacing-4: 1rem;      // 16px
$spacing-6: 1.5rem;    // 24px
$spacing-8: 2rem;      // 32px
$spacing-12: 3rem;     // 48px
$spacing-16: 4rem;     // 64px
```

### Breakpoints
```scss
// Responsive Breakpoints
$breakpoint-sm: 640px;   // Mobile
$breakpoint-md: 768px;   // Tablet
$breakpoint-lg: 1024px;  // Desktop
$breakpoint-xl: 1280px;  // Large Desktop
```

## 🔧 Configuration Management

### Environment Configuration

#### Development
```json
// appsettings.Development.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=eventplatform_dev;User=dev_user;Password=dev_password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:4200"]
  }
}
```

#### Production
```json
// appsettings.Production.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=eventplatform;User=prod_user;Password=prod_password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Cors": {
    "AllowedOrigins": ["https://yourdomain.com"]
  }
}
```

### Frontend Configuration
```typescript
// environment.ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5130/api/events',
  appName: 'EventPlatform'
};

// environment.prod.ts
export const environment = {
  production: true,
  apiUrl: 'https://api.yourdomain.com/api/events',
  appName: 'EventPlatform'
};
```

## 🧪 Testing Strategy

### Backend Testing

#### Unit Tests
```csharp
[TestClass]
public class EventsControllerTests
{
    [TestMethod]
    public async Task GetEvents_ReturnsAllEvents()
    {
        // Arrange
        var controller = new EventsController(mockService);
        
        // Act
        var result = await controller.GetEvents();
        
        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
    }
}
```

#### Integration Tests
```csharp
[TestClass]
public class EventsIntegrationTests
{
    [TestMethod]
    public async Task CreateEvent_ValidData_ReturnsCreatedEvent()
    {
        // Arrange
        var client = CreateTestClient();
        var eventData = new CreateEventRequest { /* ... */ };
        
        // Act
        var response = await client.PostAsJsonAsync("/api/events", eventData);
        
        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
    }
}
```

### Frontend Testing

#### Unit Tests
```typescript
describe('EventService', () => {
  let service: EventService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EventService]
    });
    service = TestBed.inject(EventService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should retrieve events', () => {
    const mockEvents: Event[] = [/* ... */];
    
    service.getEvents().subscribe(events => {
      expect(events.data).toEqual(mockEvents);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}`);
    expect(req.request.method).toBe('GET');
    req.flush({ success: true, data: mockEvents, message: 'Success' });
  });
});
```

#### Component Tests
```typescript
describe('EventListComponent', () => {
  let component: EventListComponent;
  let fixture: ComponentFixture<EventListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EventListComponent],
      providers: [
        { provide: EventService, useClass: MockEventService }
      ]
    }).compileComponents();
  });

  it('should display events', () => {
    // Test implementation
  });
});
```

## 🔒 Security Considerations

### API Security
- **CORS Configuration**: Properly configured for frontend domain
- **Input Validation**: Server-side validation for all inputs
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **Rate Limiting**: Consider implementing for production

### Frontend Security
- **XSS Prevention**: Angular's built-in XSS protection
- **CSRF Protection**: Not required for this API (stateless)
- **Input Sanitization**: Angular's automatic sanitization

## 📊 Performance Optimization

### Backend Optimization
- **Database Indexing**: Proper indexes on frequently queried fields
- **Query Optimization**: Efficient Entity Framework queries
- **Caching**: Consider Redis for frequently accessed data
- **Connection Pooling**: MySQL connection pooling

### Frontend Optimization
- **Lazy Loading**: Route-based code splitting
- **Bundle Optimization**: Tree shaking and minification
- **Image Optimization**: WebP format and lazy loading
- **Caching**: Browser caching strategies

## 🚀 Deployment

### Backend Deployment

#### Docker Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["EventPlatform.API/EventPlatform.API.csproj", "EventPlatform.API/"]
RUN dotnet restore "EventPlatform.API/EventPlatform.API.csproj"
COPY . .
WORKDIR "/src/EventPlatform.API"
RUN dotnet build "EventPlatform.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventPlatform.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventPlatform.API.dll"]
```

#### Azure Deployment
```yaml
# azure-pipelines.yml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

steps:
- task: DotNetCoreCLI@2
  inputs:
    command: 'build'
    projects: '**/*.csproj'
    arguments: '--configuration Release'

- task: DotNetCoreCLI@2
  inputs:
    command: 'publish'
    projects: '**/*.csproj'
    arguments: '--configuration Release --output $(Build.ArtifactStagingDirectory)'
    publishWebProjects: true

- task: PublishBuildArtifacts@1
  inputs:
    pathToPublish: '$(Build.ArtifactStagingDirectory)'
    artifactName: 'drop'
```

### Frontend Deployment

#### Vercel Deployment
```json
// vercel.json
{
  "version": 2,
  "builds": [
    {
      "src": "package.json",
      "use": "@vercel/static-build",
      "config": {
        "distDir": "dist/event-platform-frontend"
      }
    }
  ],
  "routes": [
    {
      "src": "/(.*)",
      "dest": "/index.html"
    }
  ]
}
```

#### Netlify Deployment
```toml
# netlify.toml
[build]
  publish = "dist/event-platform-frontend"
  command = "ng build --configuration production"

[[redirects]]
  from = "/*"
  to = "/index.html"
  status = 200
```

## 🔍 Monitoring and Logging

### Backend Logging
```csharp
// Program.cs
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventLog();

// Controller logging
_logger.LogInformation("Event {EventId} created successfully", event.Id);
_logger.LogWarning("Event {EventId} has past date", event.Id);
_logger.LogError("Failed to create event: {Error}", ex.Message);
```

### Frontend Logging
```typescript
// Custom logging service
@Injectable({
  providedIn: 'root'
})
export class LoggingService {
  log(message: string, level: 'info' | 'warn' | 'error' = 'info') {
    const timestamp = new Date().toISOString();
    console.log(`[${timestamp}] ${level.toUpperCase()}: ${message}`);
    
    // Send to external logging service in production
    if (environment.production) {
      // Send to logging service
    }
  }
}
```

## 🗺️ Future Enhancements

### Planned Features
1. **User Authentication**: JWT-based authentication
2. **Event Categories**: Categorization and tagging system
3. **Email Notifications**: Event reminders and updates
4. **Calendar Integration**: Google Calendar, Outlook integration
5. **Real-time Updates**: SignalR for live updates
6. **Mobile App**: React Native mobile application

### Technical Improvements
1. **Caching Layer**: Redis for performance optimization
2. **API Rate Limiting**: Protect against abuse
3. **Database Optimization**: Advanced indexing and query optimization
4. **CDN Integration**: Static asset delivery optimization
5. **Progressive Web App**: PWA features for mobile experience

---

**Documentation Version**: 1.0.0  
**Last Updated**: January 2024  
**Maintained by**: EventPlatform Development Team 