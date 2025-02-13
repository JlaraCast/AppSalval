using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavalAPI.Data;
using SavalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SavalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreguntaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PreguntaController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las preguntas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pregunta>>> GetPreguntas()
        {
            try
            {
                var preguntas = await _context.Preguntas
                    //.Include(p => p.Opciones) // Cargar opciones de respuesta
                  //  .Include(p => p.Formularios) // Cargar formularios asociados
                    .ToListAsync();
                return Ok(preguntas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Obtener una pregunta por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Pregunta>> GetPregunta(int id)
        {
            try
            {
                var pregunta = await _context.Preguntas
                   // .Include(p => p.Opciones)
                    .FirstOrDefaultAsync(p => p.IdPregunta == id);

                if (pregunta == null)
                {
                    return NotFound(new { message = "Pregunta no encontrada." });
                }

                return Ok(pregunta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Crear una nueva pregunta
        [HttpPost]
        public async Task<ActionResult<Pregunta>> PostPregunta(Pregunta pregunta)
        {
            try
            {
                _context.Preguntas.Add(pregunta);
                await _context.SaveChangesAsync();

                // Retornar solo los datos esenciales, sin relaciones innecesarias
                var response = new
                {
                    idPregunta = pregunta.IdPregunta,
                    tipoPregunta = pregunta.TipoPregunta,
                    textoPregunta = pregunta.TextoPregunta
                };

                return CreatedAtAction(nameof(GetPregunta), new { id = pregunta.IdPregunta }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        // Editar una pregunta
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPregunta(int id, Pregunta pregunta)
        {
            try
            {
                if (id != pregunta.IdPregunta)
                {
                    return BadRequest(new { message = "El ID no coincide." });
                }

                _context.Entry(pregunta).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Pregunta actualizada con éxito." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Preguntas.Any(p => p.IdPregunta == id))
                {
                    return NotFound(new { message = "Pregunta no encontrada." });
                }
                return StatusCode(500, new { message = "Error de concurrencia en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // OBTENER TODAS LAS OPCIONES DE UNA PREGUNTA
        [HttpGet("{id}/opciones")]
        public async Task<ActionResult<IEnumerable<OpcionRespuesta>>> GetOpcionesByPregunta(int id)
        {
            try
            {
                // Verificar si la pregunta existe
                var preguntaExistente = await _context.Preguntas.FindAsync(id);
                if (preguntaExistente == null)
                    return NotFound(new { message = "La pregunta especificada no existe." });

                // Obtener todas las opciones asociadas a la pregunta
                var opciones = await _context.OpcionesRespuestas
                    .Where(o => o.IdPregunta == id)
                    .ToListAsync();

                return Ok(opciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        
        // Eliminar una pregunta y sus opciones asociadas
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePregunta(int id)
        {
            try
            {
                // Buscar la pregunta en la base de datos
                var pregunta = await _context.Preguntas
                    .Include(p => p.Opciones) // Cargar las opciones asociadas
                    .FirstOrDefaultAsync(p => p.IdPregunta == id);

                if (pregunta == null)
                {
                    return NotFound(new { message = "Pregunta no encontrada." });
                }

                // Eliminar primero las opciones asociadas
                _context.OpcionesRespuestas.RemoveRange(pregunta.Opciones);

                // Luego eliminar la pregunta
                _context.Preguntas.Remove(pregunta);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Pregunta y sus opciones eliminadas con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

    }
}
