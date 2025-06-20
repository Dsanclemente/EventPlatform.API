# EventPlatform API

A modern .NET 9 Web API for event management with Entity Framework Core and MySQL.

## 🚀 Features

### 🎯 Core Features
- **RESTful API**: Complete CRUD operations for events
- **Advanced Filtering**: Filter events by title, location, and date ranges
- **AI-Powered Descriptions**: Generate event descriptions using AI
- **Real-time Status Management**: Update event status with immediate response
- **Comprehensive Validation**: Server-side validation with clear error messages

### 🔧 Technical Features
- **Entity Framework Core**: Modern ORM with code-first approach
- **MySQL Database**: Reliable and scalable database solution
- **Swagger Documentation**: Interactive API documentation
- **CORS Support**: Cross-origin resource sharing for frontend integration
- **Standardized Responses**: Consistent API response format

## 🛠️ Technology Stack

- **Framework**: .NET 9 Web API
- **ORM**: Entity Framework Core 9
- **Database**: MySQL 8.0+
- **Documentation**: Swagger/OpenAPI 3.0
- **Validation**: Data Annotations and Fluent Validation
- **Logging**: Built-in .NET logging

## 📁 Project Structure

```
EventPlatform.API/
├── Controllers/
│   └── EventsController.cs          # Main API controller
├── Data/
│   └── ApplicationDbContext.cs      # Entity Framework context
├── Models/
│   ├── Event.cs                     # Event entity model
│   └── ApiResponse.cs               # Standardized response model
├── Program.cs                       # Application entry point
├── appsettings.json                 # Configuration
└── EventPlatform.API.csproj         # Project file
```

## 🚀 Getting Started

### Prerequisites

- **.NET 9 SDK**
- **MySQL 8.0+**
- **Visual Studio Code** or **Visual Studio**

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd EventPlatform.API
   ```

2. **Configure the database**
   ```bash
   # Create MySQL database
   mysql -u root -p
   CREATE DATABASE eventplatform;
   USE eventplatform;
   
   # Run the setup script
   mysql -u root -p eventplatform < ../database_setup.sql
   ```

3. **Configure connection string**
   ```json
   // appsettings.json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=eventplatform;User=your_user;Password=your_password;"
     }
   }
   ```

4. **Run the application**
   ```bash
   dotnet restore
   dotnet run
   ```

5. **Access the API**
   - API Base URL: `http://localhost:5130`
   - Swagger UI: `http://localhost:5130/swagger`

## 📚 API Documentation

### Base URL
```
http://localhost:5130/api/events
```

### Endpoints

#### GET /api/events
Get all events with optional filtering

**Query Parameters**:
- `title` (string, optional): Filter by title
- `location` (string, optional): Filter by location
- `dateFrom` (date, optional): Filter from date
- `dateTo` (date, optional): Filter to date

**Response**:
```json
{
  "success": true,
  "message": "Events retrieved successfully",
  "data": [
    {
      "id": 1,
      "title": "Event Title",
      "dateTime": "2024-01-15T10:00:00.000Z",
      "location": "Event Location",
      "description": "Event description",
      "status": 0,
      "createdAt": "2024-01-01T00:00:00.000Z",
      "updatedAt": null
    }
  ]
}
```

#### GET /api/events/{id}
Get a specific event by ID

**Response**:
```json
{
  "success": true,
  "message": "Event retrieved successfully",
  "data": {
    "id": 1,
    "title": "Event Title",
    "dateTime": "2024-01-15T10:00:00.000Z",
    "location": "Event Location",
    "description": "Event description",
    "status": 0,
    "createdAt": "2024-01-01T00:00:00.000Z",
    "updatedAt": null
  }
}
```

#### POST /api/events
Create a new event

**Request Body**:
```json
{
  "title": "Event Title",
  "dateTime": "2024-01-15T10:00:00.000Z",
  "location": "Event Location",
  "description": "Event description",
  "status": 0
}
```

**Response**:
```json
{
  "success": true,
  "message": "Event created successfully",
  "data": {
    "id": 1,
    "title": "Event Title",
    "dateTime": "2024-01-15T10:00:00.000Z",
    "location": "Event Location",
    "description": "Event description",
    "status": 0,
    "createdAt": "2024-01-01T00:00:00.000Z",
    "updatedAt": null
  }
}
```

#### PUT /api/events/{id}
Update an existing event

**Request Body**:
```json
{
  "id": 1,
  "title": "Updated Title",
  "dateTime": "2024-01-15T10:00:00.000Z",
  "location": "Updated Location",
  "description": "Updated description",
  "status": 1
}
```

#### PATCH /api/events/{id}/status
Update event status

**Request Body**:
```json
{
  "status": 1
}
```

#### DELETE /api/events/{id}
Delete an event

**Response**:
```json
{
  "success": true,
  "message": "Event deleted successfully",
  "data": null
}
```

#### POST /api/events/generate-description
Generate AI-powered description

**Request Body**:
```json
{
  "topic": "Event topic"
}
```

**Response**:
```json
{
  "success": true,
  "message": "Description generated successfully",
  "data": "Generated description text"
}
```

### Event Status Values
- `0`: Upcoming
- `1`: Attending
- `2`: Maybe
- `3`: Declined

### HTTP Status Codes
- `200`: OK - Operation successful
- `201`: Created - Resource created successfully
- `204`: No Content - Operation successful without content
- `400`: Bad Request - Invalid data
- `404`: Not Found - Resource not found
- `500`: Internal Server Error - Server error

## 🗄️ Database Schema

### Events Table
```sql
CREATE TABLE `Events` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(100) NOT NULL,
    `DateTime` datetime(6) NOT NULL,
    `Location` varchar(200) NOT NULL,
    `Description` varchar(1000) NULL,
    `Status` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);
```

## 🔧 Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=eventplatform;User=your_user;Password=your_password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### CORS Configuration
```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## 🧪 Testing

### Unit Tests
```bash
dotnet test
```

### Integration Tests
```bash
dotnet test --filter "Category=Integration"
```

### API Testing with Swagger
1. Navigate to `http://localhost:5130/swagger`
2. Use the interactive documentation to test endpoints
3. View request/response examples

## 📦 Deployment

### Build for Production
```bash
dotnet publish -c Release -o ./publish
```

### Docker Deployment
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

### Environment Variables
```bash
# Production environment variables
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__DefaultConnection="your_production_connection_string"
```

## 🔍 Development

### Code Style
- **C# Coding Conventions**: Follow Microsoft C# coding conventions
- **SOLID Principles**: Apply SOLID design principles
- **Clean Architecture**: Separate concerns and dependencies
- **Async/Await**: Use async programming patterns

### Database Migrations
```bash
# Add migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Logging
```csharp
// Use structured logging
_logger.LogInformation("Event {EventId} created successfully", event.Id);
```

## 🐛 Troubleshooting

### Common Issues

#### Database Connection
- Verify MySQL service is running
- Check connection string format
- Ensure database exists

#### CORS Issues
- Verify CORS policy configuration
- Check frontend origin in CORS settings

#### Build Errors
```bash
dotnet clean
dotnet restore
dotnet build
```

## 📚 Resources

- [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [Swagger Documentation](https://swagger.io/docs/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📝 License

This project is licensed under the MIT License.

---

**Built with ❤️ using .NET 9** 