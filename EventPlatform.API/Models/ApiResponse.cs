using System.ComponentModel.DataAnnotations;

namespace EventPlatform.API.Models
{
    /// <summary>
    /// Respuesta estándar de la API para operaciones exitosas
    /// </summary>
    /// <typeparam name="T">Tipo de datos de la respuesta</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la operación
        /// </summary>
        /// <example>Operación completada exitosamente</example>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Datos de la respuesta (puede ser null en caso de error)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Código de estado HTTP
        /// </summary>
        /// <example>200</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// Timestamp de la respuesta
        /// </summary>
        /// <example>2025-06-20T10:09:12</example>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Crea una respuesta exitosa
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operación completada exitosamente", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Crea una respuesta de error
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Respuesta estándar para operaciones sin datos de retorno
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la operación
        /// </summary>
        /// <example>Evento actualizado exitosamente</example>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Código de estado HTTP
        /// </summary>
        /// <example>204</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// Timestamp de la respuesta
        /// </summary>
        /// <example>2025-06-20T10:09:12</example>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Crea una respuesta exitosa sin datos
        /// </summary>
        public static ApiResponse SuccessResponse(string message = "Operación completada exitosamente", int statusCode = 204)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Crea una respuesta de error sin datos
        /// </summary>
        public static ApiResponse ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Respuesta para operaciones de eliminación
    /// </summary>
    public class DeleteResponse
    {
        /// <summary>
        /// Indica si la eliminación fue exitosa
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la operación
        /// </summary>
        /// <example>Evento eliminado exitosamente</example>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// ID del elemento eliminado
        /// </summary>
        /// <example>1</example>
        public int DeletedId { get; set; }

        /// <summary>
        /// Código de estado HTTP
        /// </summary>
        /// <example>200</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// Timestamp de la respuesta
        /// </summary>
        /// <example>2025-06-20T10:09:12</example>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Crea una respuesta exitosa de eliminación
        /// </summary>
        public static DeleteResponse SuccessResponse(int deletedId, string message = "Evento eliminado exitosamente")
        {
            return new DeleteResponse
            {
                Success = true,
                Message = message,
                DeletedId = deletedId,
                StatusCode = 200,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Crea una respuesta de error de eliminación
        /// </summary>
        public static DeleteResponse ErrorResponse(string message, int statusCode = 400)
        {
            return new DeleteResponse
            {
                Success = false,
                Message = message,
                DeletedId = 0,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Respuesta para operaciones de actualización de estado
    /// </summary>
    public class StatusUpdateResponse
    {
        /// <summary>
        /// Indica si la actualización fue exitosa
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la operación
        /// </summary>
        /// <example>Estado del evento actualizado exitosamente</example>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// ID del evento actualizado
        /// </summary>
        /// <example>1</example>
        public int EventId { get; set; }

        /// <summary>
        /// Estado anterior del evento
        /// </summary>
        /// <example>Upcoming</example>
        public EventStatus PreviousStatus { get; set; }

        /// <summary>
        /// Nuevo estado del evento
        /// </summary>
        /// <example>Attending</example>
        public EventStatus NewStatus { get; set; }

        /// <summary>
        /// Código de estado HTTP
        /// </summary>
        /// <example>200</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// Timestamp de la respuesta
        /// </summary>
        /// <example>2025-06-20T10:09:12</example>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Crea una respuesta exitosa de actualización de estado
        /// </summary>
        public static StatusUpdateResponse SuccessResponse(int eventId, EventStatus previousStatus, EventStatus newStatus, string message = "Estado del evento actualizado exitosamente")
        {
            return new StatusUpdateResponse
            {
                Success = true,
                Message = message,
                EventId = eventId,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                StatusCode = 200,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Crea una respuesta de error de actualización de estado
        /// </summary>
        public static StatusUpdateResponse ErrorResponse(string message, int statusCode = 400)
        {
            return new StatusUpdateResponse
            {
                Success = false,
                Message = message,
                EventId = 0,
                PreviousStatus = EventStatus.Upcoming,
                NewStatus = EventStatus.Upcoming,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };
        }
    }
} 