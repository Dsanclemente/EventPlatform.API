# 🎉 Event Platform API

API completa para la gestión de eventos con documentación interactiva de Swagger.

## 🚀 Características

- ✅ **CRUD completo** de eventos
- ✅ **Filtros avanzados** por título, ubicación y fechas
- ✅ **Generación automática** de descripciones con IA
- ✅ **Documentación interactiva** con Swagger UI
- ✅ **Validaciones robustas** con mensajes de error claros
- ✅ **Base de datos MySQL** con Entity Framework Core
- ✅ **CORS configurado** para aplicaciones frontend

## 📋 Requisitos Previos

- .NET 9.0 SDK
- MySQL Server
- Visual Studio Code o Visual Studio

## 🛠️ Instalación

### 1. Clonar el repositorio
```bash
git clone <repository-url>
cd EventPlatform.API
```

### 2. Configurar la base de datos
Ejecuta el script SQL en tu servidor MySQL:
```sql
-- Crear la base de datos
CREATE DATABASE IF NOT EXISTS TodoAppDb;
USE TodoAppDb;

-- Crear la tabla Events
CREATE TABLE IF NOT EXISTS `Events` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(100) NOT NULL,
    `DateTime` datetime(6) NOT NULL,
    `Location` varchar(200) NOT NULL,
    `Description` varchar(1000) NULL,
    `Status` varchar(20) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);
```

### 3. Configurar la conexión
Edita `EventPlatform.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;port=3306;database=TodoAppDb;user=root;password=tu_password"
  }
}
```

### 4. Ejecutar la aplicación
```bash
dotnet restore
dotnet build
dotnet run --project EventPlatform.API
```

## 🌐 Acceso a la API

- **API Base URL:** http://localhost:5130
- **Swagger UI:** http://localhost:5130/swagger
- **OpenAPI JSON:** http://localhost:5130/openapi.json

## 📚 Endpoints Disponibles

### 🔍 GET /api/events
Obtiene todos los eventos con filtros opcionales.

**Parámetros de consulta:**
- `title` (opcional): Filtrar por título
- `location` (opcional): Filtrar por ubicación
- `dateFrom` (opcional): Filtrar desde fecha
- `dateTo` (opcional): Filtrar hasta fecha

**Ejemplo:**
```bash
curl "http://localhost:5130/api/events?title=conferencia&dateFrom=2025-07-01"
```

### 🔍 GET /api/events/{id}
Obtiene un evento específico por ID.

**Ejemplo:**
```bash
curl http://localhost:5130/api/events/1
```

### ➕ POST /api/events
Crea un nuevo evento.

**Request Body:**
```json
{
  "title": "Conferencia de Inteligencia Artificial 2024",
  "dateTime": "2025-07-15T14:00:00",
  "location": "Centro de Convenciones - Auditorio Principal",
  "description": "Una conferencia emocionante sobre las últimas tendencias en IA y machine learning.",
  "status": 0
}
```

**Ejemplo:**
```bash
curl -X POST http://localhost:5130/api/events \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Conferencia de Inteligencia Artificial 2024",
    "dateTime": "2025-07-15T14:00:00",
    "location": "Centro de Convenciones - Auditorio Principal",
    "description": "Una conferencia emocionante sobre las últimas tendencias en IA y machine learning.",
    "status": 0
  }'
```

### ✏️ PUT /api/events/{id}
Actualiza un evento existente.

**Request Body:**
```json
{
  "id": 1,
  "title": "Conferencia de Tecnología 2024 - Actualizada",
  "dateTime": "2025-07-15T15:30:00",
  "location": "Centro de Convenciones - Sala A",
  "description": "Una conferencia actualizada sobre las últimas tendencias en tecnología.",
  "status": 1
}
```

**Ejemplo:**
```bash
curl -X PUT http://localhost:5130/api/events/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "title": "Conferencia de Tecnología 2024 - Actualizada",
    "dateTime": "2025-07-15T15:30:00",
    "location": "Centro de Convenciones - Sala A",
    "description": "Una conferencia actualizada sobre las últimas tendencias en tecnología.",
    "status": 1
  }'
```

### 🔄 PATCH /api/events/{id}/status
Actualiza solo el estado de un evento.

**Request Body:**
```json
{
  "status": 2
}
```

**Ejemplo:**
```bash
curl -X PATCH http://localhost:5130/api/events/1/status \
  -H "Content-Type: application/json" \
  -d '{"status": 2}'
```

### 🗑️ DELETE /api/events/{id}
Elimina un evento.

**Ejemplo:**
```bash
curl -X DELETE http://localhost:5130/api/events/1
```

### 🤖 POST /api/events/generate-description
Genera una descripción automática para un evento.

**Request Body:**
```json
{
  "topic": "Desarrollo Web Moderno"
}
```

**Ejemplo:**
```bash
curl -X POST http://localhost:5130/api/events/generate-description \
  -H "Content-Type: application/json" \
  -d '{"topic": "Desarrollo Web Moderno"}'
```

## 📊 Códigos de Estado de Eventos

| Código | Estado | Descripción |
|--------|--------|-------------|
| 0 | Upcoming | Evento próximo |
| 1 | Attending | Asistiendo al evento |
| 2 | Maybe | Tal vez asistir |
| 3 | Declined | Rechazado |

## 🔧 Códigos de Respuesta HTTP

| Código | Descripción |
|--------|-------------|
| 200 | OK - Operación exitosa |
| 201 | Created - Recurso creado |
| 204 | No Content - Operación exitosa sin contenido |
| 400 | Bad Request - Datos inválidos |
| 404 | Not Found - Recurso no encontrado |
| 500 | Internal Server Error - Error del servidor |

## 🛡️ Validaciones

La API incluye validaciones robustas:

- **Título:** Requerido, máximo 100 caracteres
- **Fecha y hora:** Requerido, formato ISO 8601
- **Ubicación:** Requerido, máximo 200 caracteres
- **Descripción:** Opcional, máximo 1000 caracteres
- **Estado:** Requerido, valores válidos: 0, 1, 2, 3

## 🎯 Ejemplos de Uso Completos

### Crear y gestionar eventos
```bash
# 1. Crear un evento
curl -X POST http://localhost:5130/api/events \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Workshop de Angular",
    "dateTime": "2025-07-20T10:00:00",
    "location": "Universidad Local",
    "description": "Taller práctico sobre Angular 17+",
    "status": 0
  }'

# 2. Obtener todos los eventos
curl http://localhost:5130/api/events

# 3. Actualizar el estado
curl -X PATCH http://localhost:5130/api/events/1/status \
  -H "Content-Type: application/json" \
  -d '{"status": 1}'

# 4. Generar descripción automática
curl -X POST http://localhost:5130/api/events/generate-description \
  -H "Content-Type: application/json" \
  -d '{"topic": "Machine Learning"}'
```

## 🔍 Filtros Avanzados

### Filtrar por título
```bash
curl "http://localhost:5130/api/events?title=conferencia"
```

### Filtrar por ubicación
```bash
curl "http://localhost:5130/api/events?location=centro"
```

### Filtrar por rango de fechas
```bash
curl "http://localhost:5130/api/events?dateFrom=2025-07-01&dateTo=2025-07-31"
```

### Combinar filtros
```bash
curl "http://localhost:5130/api/events?title=tecnología&location=convenciones&dateFrom=2025-07-01"
```

## 🚀 Desarrollo

### Estructura del Proyecto
```
EventPlatform.API/
├── Controllers/
│   └── EventsController.cs      # Controlador principal
├── Data/
│   └── ApplicationDbContext.cs  # Contexto de Entity Framework
├── Models/
│   └── Event.cs                 # Modelo de datos
├── Program.cs                   # Configuración de la aplicación
└── appsettings.json            # Configuración de conexión
```

### Tecnologías Utilizadas
- **.NET 9.0** - Framework de desarrollo
- **Entity Framework Core** - ORM para base de datos
- **MySQL** - Base de datos
- **Swagger/OpenAPI** - Documentación de API
- **ASP.NET Core** - Framework web

## 🤝 Contribuir

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.

## 📞 Soporte

- **Email:** dev@eventplatform.com
- **Documentación:** http://localhost:5130/swagger
- **Issues:** [GitHub Issues](https://github.com/your-repo/issues)

---

¡Disfruta usando la Event Platform API! 🎉 