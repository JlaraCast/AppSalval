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
    public class RespuestaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RespuestaController(AppDbContext context)
        {
            _context = context;
        }

        // OBTENER TODAS LAS RESPUESTAS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Respuesta>>> GetRespuestas()
        {
            try
            {
                var respuestas = await _context.Respuestas
                   // .Include(r => r.Formulario)
                    //.Include(r => r.Encuestado)
                    .ToListAsync();

                return Ok(respuestas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // OBTENER UNA RESPUESTA POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Respuesta>> GetRespuesta(int id)
        {
            try
            {
                var respuesta = await _context.Respuestas
                    //.Include(r => r.Formulario)
                    //.Include(r => r.Encuestado)
                    .FirstOrDefaultAsync(r => r.IdRespuesta == id);

                if (respuesta == null)
                    return NotFound(new { message = "Respuesta no encontrada." });

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // CREAR UNA NUEVA RESPUESTA
        [HttpPost]
        public async Task<ActionResult<Respuesta>> PostRespuesta(Respuesta respuesta)
        {
            try
            {
                // Validar existencia del Formulario
                var formularioExistente = await _context.Formularios.FindAsync(respuesta.IdFormulario);
                if (formularioExistente == null)
                    return BadRequest(new { message = "El formulario especificado no existe." });

                // Validar si el Encuestado es opcional o existe
                if (!string.IsNullOrEmpty(respuesta.IdentificacionEncuestado))
                {
                    var encuestadoExistente = await _context.Encuestados.FindAsync(respuesta.IdentificacionEncuestado);
                    if (encuestadoExistente == null)
                        return BadRequest(new { message = "El encuestado especificado no existe." });
                }

                // Asignar la fecha de la respuesta automáticamente
                respuesta.FechaRespuesta = DateTime.UtcNow;

                _context.Respuestas.Add(respuesta);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRespuesta), new { id = respuesta.IdRespuesta }, respuesta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        //  ACTUALIZAR UNA RESPUESTA
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRespuesta(int id, Respuesta respuesta)
        {
            try
            {
                if (id != respuesta.IdRespuesta)
                    return BadRequest(new { message = "El ID de la respuesta no coincide." });

                // Validar existencia del Formulario
                var formularioExistente = await _context.Formularios.FindAsync(respuesta.IdFormulario);
                if (formularioExistente == null)
                    return BadRequest(new { message = "El formulario especificado no existe." });

                // Validar si el Encuestado es opcional o existe
                if (!string.IsNullOrEmpty(respuesta.IdentificacionEncuestado))
                {
                    var encuestadoExistente = await _context.Encuestados.FindAsync(respuesta.IdentificacionEncuestado);
                    if (encuestadoExistente == null)
                        return BadRequest(new { message = "El encuestado especificado no existe." });
                }

                _context.Entry(respuesta).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Respuesta actualizada con éxito." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Respuestas.Any(r => r.IdRespuesta == id))
                    return NotFound(new { message = "Respuesta no encontrada." });

                return StatusCode(500, new { message = "Error de concurrencia en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
        [HttpGet("encuestado/{identificacion}/formulario/{idFormulario}")]
        public async Task<ActionResult<IEnumerable<Respuesta>>> GetRespuestasPorEncuestadoYFormulario(string identificacion, int idFormulario)
        {
            try
            {
                var respuestas = await _context.Respuestas
                    .Where(r => r.IdentificacionEncuestado == identificacion && r.IdFormulario == idFormulario)
                    .Include(r => r.Formulario)
                  
                    .ToListAsync();

                if (!respuestas.Any())
                    return NotFound(new { message = "No hay respuestas para este encuestado en el formulario especificado." });

                return Ok(respuestas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        //  ELIMINAR UNA RESPUESTA
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRespuesta(int id)
        {
            try
            {
                var respuesta = await _context.Respuestas.FindAsync(id);

                if (respuesta == null)
                    return NotFound(new { message = "Respuesta no encontrada." });

                _context.Respuestas.Remove(respuesta);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Respuesta eliminada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
