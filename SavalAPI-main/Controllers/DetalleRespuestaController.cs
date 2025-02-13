using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavalAPI.Data;
using SavalAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SavalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleRespuestaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DetalleRespuestaController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ OBTENER TODOS LOS DETALLES DE RESPUESTA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleRespuesta>>> GetDetallesRespuestas()
        {
            try
            {
                var detalles = await _context.DetallesRespuestas
                    .ToListAsync();

                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ OBTENER UN DETALLE RESPUESTA POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleRespuesta>> GetDetalleRespuesta(int id)
        {
            try
            {
                var detalle = await _context.DetallesRespuestas
                    .FirstOrDefaultAsync(d => d.IdDetalleRespuesta == id);

                if (detalle == null)
                    return NotFound(new { message = "Detalle de respuesta no encontrado." });

                return Ok(detalle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ OBTENER DETALLES DE RESPUESTA POR ID DE RESPUESTA (Para obtener todas las opciones seleccionadas en una respuesta)
        [HttpGet("respuesta/{idRespuesta}")]
        public async Task<ActionResult<IEnumerable<DetalleRespuesta>>> GetDetallesPorRespuesta(int idRespuesta)
        {
            try
            {
                var detalles = await _context.DetallesRespuestas
                    .Where(d => d.IdRespuesta == idRespuesta)
                    .ToListAsync();

                if (!detalles.Any())
                    return NotFound(new { message = "No hay detalles para esta respuesta." });

                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ CREAR UN NUEVO DETALLE RESPUESTA
        [HttpPost]
        public async Task<ActionResult<DetalleRespuesta>> PostDetalleRespuesta(DetalleRespuesta detalleRespuesta)
        {
            try
            {
                // Validar que la respuesta exista
                var respuestaExistente = await _context.Respuestas.FindAsync(detalleRespuesta.IdRespuesta);
                if (respuestaExistente == null)
                    return BadRequest(new { message = "La respuesta especificada no existe." });

                // Validar que la pregunta exista
                var preguntaExistente = await _context.Preguntas.FindAsync(detalleRespuesta.IdPregunta);
                if (preguntaExistente == null)
                    return BadRequest(new { message = "La pregunta especificada no existe." });

                // Validar que la opción de respuesta exista
                var opcionExistente = await _context.OpcionesRespuestas.FindAsync(detalleRespuesta.IdOpcion);
                if (opcionExistente == null)
                    return BadRequest(new { message = "La opción de respuesta especificada no existe." });

                _context.DetallesRespuestas.Add(detalleRespuesta);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDetalleRespuesta), new { id = detalleRespuesta.IdDetalleRespuesta }, detalleRespuesta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ ACTUALIZAR UN DETALLE RESPUESTA
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleRespuesta(int id, DetalleRespuesta detalleRespuesta)
        {
            try
            {
                if (id != detalleRespuesta.IdDetalleRespuesta)
                    return BadRequest(new { message = "El ID del detalle de respuesta no coincide." });

                // Validar existencia del detalle de respuesta
                var detalleExistente = await _context.DetallesRespuestas.FindAsync(id);
                if (detalleExistente == null)
                    return NotFound(new { message = "Detalle de respuesta no encontrado." });

                // Validar existencia de la nueva opción de respuesta si fue modificada
                var opcionExistente = await _context.OpcionesRespuestas.FindAsync(detalleRespuesta.IdOpcion);
                if (opcionExistente == null)
                    return BadRequest(new { message = "La nueva opción de respuesta no existe." });

                _context.Entry(detalleRespuesta).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Detalle de respuesta actualizado con éxito." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DetallesRespuestas.Any(d => d.IdDetalleRespuesta == id))
                    return NotFound(new { message = "Detalle de respuesta no encontrado." });

                return StatusCode(500, new { message = "Error de concurrencia en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ ELIMINAR UN DETALLE RESPUESTA
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleRespuesta(int id)
        {
            try
            {
                var detalle = await _context.DetallesRespuestas.FindAsync(id);

                if (detalle == null)
                    return NotFound(new { message = "Detalle de respuesta no encontrado." });

                _context.DetallesRespuestas.Remove(detalle);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Detalle de respuesta eliminado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
