using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventPlatform.API.Data;
using EventPlatform.API.Models;
using System.ComponentModel.DataAnnotations;

namespace EventPlatform.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de eventos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los eventos con filtros opcionales
        /// </summary>
        /// <param name="title">Filtrar por título del evento (búsqueda parcial)</param>
        /// <param name="location">Filtrar por ubicación del evento (búsqueda parcial)</param>
        /// <param name="dateFrom">Filtrar eventos desde esta fecha (formato: YYYY-MM-DD)</param>
        /// <param name="dateTo">Filtrar eventos hasta esta fecha (formato: YYYY-MM-DD)</param>
        /// <returns>Lista de eventos ordenados por fecha</returns>
        /// <response code="200">Lista de eventos obtenida exitosamente</response>
        /// <response code="400">Parámetros de filtro inválidos</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Event>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents(
            [FromQuery] string? title,
            [FromQuery] string? location,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo)
        {
            var query = _context.Events.AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(e => e.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(e => e.Location.Contains(location));
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(e => e.DateTime >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(e => e.DateTime <= dateTo.Value);
            }

            return await query.OrderBy(e => e.DateTime).ToListAsync();
        }

        /// <summary>
        /// Obtiene un evento específico por su ID
        /// </summary>
        /// <param name="id">ID único del evento</param>
        /// <returns>Evento encontrado</returns>
        /// <response code="200">Evento encontrado exitosamente</response>
        /// <response code="404">Evento no encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Event), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound(new { message = $"Evento con ID {id} no encontrado" });
            }

            return @event;
        }

        /// <summary>
        /// Crea un nuevo evento
        /// </summary>
        /// <param name="eventRequest">Datos del evento a crear</param>
        /// <returns>Evento creado con su ID asignado</returns>
        /// <response code="201">Evento creado exitosamente</response>
        /// <response code="400">Datos del evento inválidos</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Event>), 201)]
        [ProducesResponseType(typeof(ApiResponse<Event>), 400)]
        public async Task<ActionResult<ApiResponse<Event>>> CreateEvent([FromBody] CreateEventRequest eventRequest)
        {
            try
            {
                var newEvent = new Event
                {
                    Title = eventRequest.Title,
                    DateTime = eventRequest.DateTime,
                    Location = eventRequest.Location,
                    Description = eventRequest.Description,
                    Status = eventRequest.Status,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                var response = ApiResponse<Event>.SuccessResponse(
                    newEvent, 
                    "Evento creado exitosamente", 
                    201
                );

                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<Event>.ErrorResponse(
                    $"Error al crear el evento: {ex.Message}", 
                    400
                );
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Actualiza un evento existente
        /// </summary>
        /// <param name="id">ID del evento a actualizar</param>
        /// <param name="eventRequest">Datos actualizados del evento</param>
        /// <returns>Respuesta de la operación</returns>
        /// <response code="200">Evento actualizado exitosamente</response>
        /// <response code="400">Datos del evento inválidos</response>
        /// <response code="404">Evento no encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ApiResponse>> UpdateEvent(int id, [FromBody] UpdateEventRequest eventRequest)
        {
            try
            {
                if (id != eventRequest.Id)
                {
                    var errorResponse = ApiResponse.ErrorResponse(
                        "El ID en la URL no coincide con el ID en el cuerpo de la petición", 
                        400
                    );
                    return BadRequest(errorResponse);
                }

                var existingEvent = await _context.Events.FindAsync(id);
                if (existingEvent == null)
                {
                    var errorResponse = ApiResponse.ErrorResponse(
                        $"Evento con ID {id} no encontrado", 
                        404
                    );
                    return NotFound(errorResponse);
                }

                existingEvent.Title = eventRequest.Title;
                existingEvent.DateTime = eventRequest.DateTime;
                existingEvent.Location = eventRequest.Location;
                existingEvent.Description = eventRequest.Description;
                existingEvent.Status = eventRequest.Status;
                existingEvent.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var successResponse = ApiResponse.SuccessResponse(
                    "Evento actualizado exitosamente", 
                    200
                );
                return Ok(successResponse);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    var errorResponse = ApiResponse.ErrorResponse(
                        $"Evento con ID {id} no encontrado", 
                        404
                    );
                    return NotFound(errorResponse);
                }
                else
                {
                    var errorResponse = ApiResponse.ErrorResponse(
                        "Error de concurrencia al actualizar el evento", 
                        500
                    );
                    return StatusCode(500, errorResponse);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse.ErrorResponse(
                    $"Error al actualizar el evento: {ex.Message}", 
                    400
                );
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Actualiza solo el estado de un evento
        /// </summary>
        /// <param name="id">ID del evento</param>
        /// <param name="statusRequest">Nuevo estado del evento</param>
        /// <returns>Respuesta con información del cambio de estado</returns>
        /// <response code="200">Estado actualizado exitosamente</response>
        /// <response code="404">Evento no encontrado</response>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(StatusUpdateResponse), 200)]
        [ProducesResponseType(typeof(StatusUpdateResponse), 404)]
        public async Task<ActionResult<StatusUpdateResponse>> UpdateEventStatus(int id, [FromBody] UpdateStatusRequest statusRequest)
        {
            try
            {
                var @event = await _context.Events.FindAsync(id);
                if (@event == null)
                {
                    var errorResponse = StatusUpdateResponse.ErrorResponse(
                        $"Evento con ID {id} no encontrado", 
                        404
                    );
                    return NotFound(errorResponse);
                }

                var previousStatus = @event.Status;
                @event.Status = statusRequest.Status;
                @event.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var successResponse = StatusUpdateResponse.SuccessResponse(
                    id, 
                    previousStatus, 
                    statusRequest.Status
                );
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StatusUpdateResponse.ErrorResponse(
                    $"Error al actualizar el estado del evento: {ex.Message}", 
                    400
                );
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Elimina un evento
        /// </summary>
        /// <param name="id">ID del evento a eliminar</param>
        /// <returns>Respuesta con información de la eliminación</returns>
        /// <response code="200">Evento eliminado exitosamente</response>
        /// <response code="404">Evento no encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteResponse), 200)]
        [ProducesResponseType(typeof(DeleteResponse), 404)]
        public async Task<ActionResult<DeleteResponse>> DeleteEvent(int id)
        {
            try
            {
                var @event = await _context.Events.FindAsync(id);
                if (@event == null)
                {
                    var errorResponse = DeleteResponse.ErrorResponse(
                        $"Evento con ID {id} no encontrado", 
                        404
                    );
                    return NotFound(errorResponse);
                }

                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();

                var successResponse = DeleteResponse.SuccessResponse(id);
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = DeleteResponse.ErrorResponse(
                    $"Error al eliminar el evento: {ex.Message}", 
                    400
                );
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Genera una descripción automática para un evento basada en un tema
        /// </summary>
        /// <param name="request">Tema para generar la descripción</param>
        /// <returns>Descripción generada automáticamente</returns>
        /// <response code="200">Descripción generada exitosamente</response>
        /// <response code="400">Tema inválido</response>
        [HttpPost("generate-description")]
        [ProducesResponseType(typeof(ApiResponse<GenerateDescriptionResponse>), 200)]
        [ProducesResponseType(typeof(ApiResponse<GenerateDescriptionResponse>), 400)]
        public ActionResult<ApiResponse<GenerateDescriptionResponse>> GenerateDescription([FromBody] GenerateDescriptionRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Topic))
                {
                    var errorResponse = ApiResponse<GenerateDescriptionResponse>.ErrorResponse(
                        "El tema es requerido", 
                        400
                    );
                    return BadRequest(errorResponse);
                }

                // Simulación de IA generativa
                var descriptions = new[]
                {
                    $"Un evento emocionante sobre {request.Topic} que no te puedes perder. Únete a nosotros para explorar las últimas innovaciones y tendencias en este campo dinámico.",
                    $"Descubre las últimas tendencias en {request.Topic} en este evento único. Una oportunidad perfecta para aprender de expertos y conectar con profesionales del sector.",
                    $"Conecta con expertos en {request.Topic} y expande tu red profesional. Este evento te brindará insights valiosos y oportunidades de networking.",
                    $"Aprende de los mejores en {request.Topic} en un ambiente colaborativo. Talleres prácticos, charlas inspiradoras y mucho más te esperan.",
                    $"Explora nuevas oportunidades en {request.Topic} con profesionales del sector. Un evento diseñado para impulsar tu carrera y conocimientos."
                };

                var random = new Random();
                var generatedDescription = descriptions[random.Next(descriptions.Length)];

                var response = new GenerateDescriptionResponse { Description = generatedDescription };
                var successResponse = ApiResponse<GenerateDescriptionResponse>.SuccessResponse(
                    response, 
                    "Descripción generada exitosamente", 
                    200
                );

                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<GenerateDescriptionResponse>.ErrorResponse(
                    $"Error al generar la descripción: {ex.Message}", 
                    400
                );
                return BadRequest(errorResponse);
            }
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }

    /// <summary>
    /// Request para crear un nuevo evento
    /// </summary>
    public class CreateEventRequest
    {
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
        /// Descripción del evento (máximo 1000 caracteres)
        /// </summary>
        /// <example>Una conferencia sobre las últimas tendencias en tecnología</example>
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Estado del evento
        /// </summary>
        /// <example>0</example>
        [Required(ErrorMessage = "El estado es requerido")]
        public EventStatus Status { get; set; } = EventStatus.Upcoming;
    }

    /// <summary>
    /// Request para actualizar un evento existente
    /// </summary>
    public class UpdateEventRequest
    {
        /// <summary>
        /// ID del evento
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "El ID es requerido")]
        public int Id { get; set; }

        /// <summary>
        /// Título del evento (máximo 100 caracteres)
        /// </summary>
        /// <example>Conferencia de Tecnología 2024 - Actualizada</example>
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "El título no puede exceder 100 caracteres")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora del evento
        /// </summary>
        /// <example>2025-07-15T15:30:00</example>
        [Required(ErrorMessage = "La fecha y hora son requeridas")]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Ubicación del evento (máximo 200 caracteres)
        /// </summary>
        /// <example>Centro de Convenciones - Sala A</example>
        [Required(ErrorMessage = "La ubicación es requerida")]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder 200 caracteres")]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del evento (máximo 1000 caracteres)
        /// </summary>
        /// <example>Una conferencia actualizada sobre las últimas tendencias en tecnología</example>
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Estado del evento
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "El estado es requerido")]
        public EventStatus Status { get; set; } = EventStatus.Upcoming;
    }

    /// <summary>
    /// Request para actualizar el estado de un evento
    /// </summary>
    public class UpdateStatusRequest
    {
        /// <summary>
        /// Nuevo estado del evento
        /// </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "El estado es requerido")]
        public EventStatus Status { get; set; }
    }

    /// <summary>
    /// Request para generar descripción automática
    /// </summary>
    public class GenerateDescriptionRequest
    {
        /// <summary>
        /// Tema para generar la descripción
        /// </summary>
        /// <example>Desarrollo Web</example>
        [Required(ErrorMessage = "El tema es requerido")]
        public string Topic { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response de descripción generada
    /// </summary>
    public class GenerateDescriptionResponse
    {
        /// <summary>
        /// Descripción generada automáticamente
        /// </summary>
        /// <example>Un evento emocionante sobre Desarrollo Web que no te puedes perder.</example>
        public string Description { get; set; } = string.Empty;
    }
} 