using System.ComponentModel.DataAnnotations;

namespace EventPlatform.API.Models
{
    /// <summary>
    /// Modelo que representa un evento en la plataforma
    /// </summary>
    public class Event
    {
        /// <summary>
        /// ID único del evento (auto-generado)
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <summary>
        /// Título del evento (máximo 100 caracteres)
        /// </summary>
        /// <example>Conferencia de Tecnología 2024</example>
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "El título no puede exceder 100 caracteres")]
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Fecha y hora del evento
        /// </summary>
        /// <example>2025-07-15T14:00:00</example>
        [Required(ErrorMessage = "La fecha y hora son requeridas")]
        public DateTime DateTime { get; set; }
        
        /// <summary>
        /// Ubicación del evento (máximo 200 caracteres)
        /// </summary>
        /// <example>Centro de Convenciones</example>
        [Required(ErrorMessage = "La ubicación es requerida")]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder 200 caracteres")]
        public string Location { get; set; } = string.Empty;
        
        /// <summary>
        /// Descripción detallada del evento (máximo 1000 caracteres)
        /// </summary>
        /// <example>Una conferencia sobre las últimas tendencias en tecnología</example>
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Estado actual del evento
        /// </summary>
        /// <example>Upcoming</example>
        [Required(ErrorMessage = "El estado es requerido")]
        public EventStatus Status { get; set; } = EventStatus.Upcoming;
        
        /// <summary>
        /// Fecha y hora de creación del evento (auto-generado)
        /// </summary>
        /// <example>2025-06-20T10:09:12</example>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Fecha y hora de la última actualización del evento (opcional)
        /// </summary>
        /// <example>2025-06-20T15:30:00</example>
        public DateTime? UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// Estados posibles de un evento
    /// </summary>
    public enum EventStatus
    {
        /// <summary>
        /// Evento próximo (0)
        /// </summary>
        Upcoming = 0,
        
        /// <summary>
        /// Asistiendo al evento (1)
        /// </summary>
        Attending = 1,
        
        /// <summary>
        /// Tal vez asistir al evento (2)
        /// </summary>
        Maybe = 2,
        
        /// <summary>
        /// Rechazado el evento (3)
        /// </summary>
        Declined = 3
    }
} 