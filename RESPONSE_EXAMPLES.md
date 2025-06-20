# 📋 Ejemplos de Respuestas de la API

## 🎯 Objetos de Respuesta Estandarizados

La API ahora utiliza objetos de respuesta estandarizados que funcionan tanto para éxito como para error, proporcionando consistencia y mejor experiencia de usuario.

## 📊 Estructura de Respuestas

### ✅ Respuesta Exitosa Genérica
```json
{
  "success": true,
  "message": "Operación completada exitosamente",
  "data": { /* datos específicos */ },
  "statusCode": 200,
  "timestamp": "2025-06-20T10:09:12"
}
```

### ❌ Respuesta de Error Genérica
```json
{
  "success": false,
  "message": "Mensaje de error descriptivo",
  "data": null,
  "statusCode": 400,
  "timestamp": "2025-06-20T10:09:12"
}
```

## 🔧 Endpoints y sus Respuestas

### 1. **POST /api/events** - Crear Evento
**Respuesta Exitosa (201):**
```json
{
  "id": 4,
  "title": "Conferencia de Inteligencia Artificial 2024",
  "dateTime": "2025-07-15T14:00:00",
  "location": "Centro de Convenciones",
  "description": "Una conferencia emocionante sobre IA",
  "status": 0,
  "createdAt": "2025-06-20T10:09:12",
  "updatedAt": null
}
```

**Respuesta de Error (400):**
```json
{
  "message": "Error al crear el evento: El título es requerido"
}
```

### 2. **PUT /api/events/{id}** - Actualizar Evento
**Respuesta Exitosa (200):**
```json
{
  "success": true,
  "message": "Evento actualizado exitosamente",
  "statusCode": 200,
  "timestamp": "2025-06-20T10:09:12"
}
```

**Respuesta de Error (404):**
```json
{
  "success": false,
  "message": "Evento con ID 999 no encontrado",
  "statusCode": 404,
  "timestamp": "2025-06-20T10:09:12"
}
```

### 3. **PATCH /api/events/{id}/status** - Actualizar Estado
**Respuesta Exitosa (200):**
```json
{
  "success": true,
  "message": "Estado del evento actualizado exitosamente",
  "eventId": 1,
  "previousStatus": 0,
  "newStatus": 1,
  "statusCode": 200,
  "timestamp": "2025-06-20T10:09:12"
}
```

**Respuesta de Error (404):**
```json
{
  "success": false,
  "message": "Evento con ID 999 no encontrado",
  "eventId": 0,
  "previousStatus": 0,
  "newStatus": 0,
  "statusCode": 404,
  "timestamp": "2025-06-20T10:09:12"
}
```

### 4. **DELETE /api/events/{id}** - Eliminar Evento
**Respuesta Exitosa (200):**
```json
{
  "success": true,
  "message": "Evento eliminado exitosamente",
  "deletedId": 1,
  "statusCode": 200,
  "timestamp": "2025-06-20T10:09:12"
}
```

**Respuesta de Error (404):**
```json
{
  "success": false,
  "message": "Evento con ID 999 no encontrado",
  "deletedId": 0,
  "statusCode": 404,
  "timestamp": "2025-06-20T10:09:12"
}
```

### 5. **POST /api/events/generate-description** - Generar Descripción
**Respuesta Exitosa (200):**
```json
{
  "success": true,
  "message": "Descripción generada exitosamente",
  "data": {
    "description": "Un evento emocionante sobre Desarrollo Web Moderno que no te puedes perder. Únete a nosotros para explorar las últimas innovaciones y tendencias en este campo dinámico."
  },
  "statusCode": 200,
  "timestamp": "2025-06-20T10:09:12"
}
```

**Respuesta de Error (400):**
```json
{
  "success": false,
  "message": "El tema es requerido",
  "data": null,
  "statusCode": 400,
  "timestamp": "2025-06-20T10:09:12"
}
```

## 🎯 Beneficios de los Objetos de Respuesta Estandarizados

### ✅ **Consistencia**
- Todas las respuestas siguen la misma estructura
- Fácil de procesar en el frontend
- Predictible para los desarrolladores

### ✅ **Información Detallada**
- Mensajes descriptivos en español
- Códigos de estado HTTP incluidos
- Timestamps para auditoría
- Información específica según el tipo de operación

### ✅ **Manejo de Errores Mejorado**
- Errores descriptivos y útiles
- Códigos de estado apropiados
- Información de contexto cuando es relevante

### ✅ **Experiencia de Usuario**
- Respuestas claras y comprensibles
- Información suficiente para debugging
- Consistencia en toda la API

## 🔧 Ejemplos de Uso en el Frontend

### JavaScript/TypeScript
```javascript
// Ejemplo de manejo de respuestas
async function updateEventStatus(eventId, newStatus) {
  try {
    const response = await fetch(`/api/events/${eventId}/status`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ status: newStatus })
    });
    
    const result = await response.json();
    
    if (result.success) {
      console.log(`Estado actualizado: ${result.previousStatus} → ${result.newStatus}`);
      showNotification(result.message, 'success');
    } else {
      showNotification(result.message, 'error');
    }
  } catch (error) {
    console.error('Error:', error);
  }
}
```

### React Hook
```javascript
const useEventOperations = () => {
  const updateEventStatus = async (eventId, newStatus) => {
    const response = await fetch(`/api/events/${eventId}/status`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ status: newStatus })
    });
    
    const result = await response.json();
    return result;
  };
  
  return { updateEventStatus };
};
```

## 📊 Códigos de Estado HTTP Utilizados

| Código | Descripción | Uso |
|--------|-------------|-----|
| **200** | OK | Operaciones exitosas (GET, PUT, PATCH, DELETE) |
| **201** | Created | Recurso creado exitosamente (POST) |
| **400** | Bad Request | Datos inválidos o errores de validación |
| **404** | Not Found | Recurso no encontrado |
| **500** | Internal Server Error | Errores del servidor |

## 🎉 Conclusión

Los nuevos objetos de respuesta estandarizados proporcionan:

1. **Consistencia** en todas las respuestas de la API
2. **Información detallada** para debugging y auditoría
3. **Mejor experiencia de usuario** con mensajes claros
4. **Facilidad de integración** con frontends
5. **Manejo robusto de errores** con contexto apropiado

¡La API ahora es más profesional y fácil de usar! 🚀 