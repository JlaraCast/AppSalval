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
    public class FormularioPreguntaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FormularioPreguntaController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las preguntas de un formulario
        [HttpGet("formulario/{idFormulario}")]
        public async Task<ActionResult<IEnumerable<Pregunta>>> GetPreguntasPorFormulario(int idFormulario)
        {
            try
            {
                var preguntas = await _context.FormulariosPreguntas
                    .Where(pf => pf.IdFormulario == idFormulario)
                    .Select(pf => pf.Pregunta)
                    .ToListAsync();

                if (!preguntas.Any())
                    return NotFound(new { message = "No hay preguntas asociadas a este formulario." });

                return Ok(preguntas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Obtener todos los formularios donde aparece una pregunta
        [HttpGet("pregunta/{idPregunta}")]
        public async Task<ActionResult<IEnumerable<Formulario>>> GetFormulariosPorPregunta(int idPregunta)
        {
            try
            {
                var formularios = await _context.FormulariosPreguntas
                    .Where(pf => pf.IdPregunta == idPregunta)
                    .Select(pf => pf.Formulario)
                    .ToListAsync();

                if (!formularios.Any())
                    return NotFound(new { message = "Esta pregunta no está asociada a ningún formulario." });

                return Ok(formularios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Asignar una pregunta a un formulario (Crear relación)
        [HttpPost]
        public async Task<ActionResult<FormularioPregunta>> PostPreguntaFormulario(FormularioPregunta relacion)
        {
            try
            {
                // Validar que tanto el formulario como la pregunta existan
                var formularioExistente = await _context.Formularios.FindAsync(relacion.IdFormulario);
                var preguntaExistente = await _context.Preguntas.FindAsync(relacion.IdPregunta);

                if (formularioExistente == null || preguntaExistente == null)
                    return BadRequest(new { message = "Formulario o pregunta no encontrados." });

                _context.FormulariosPreguntas.Add(relacion);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPreguntasPorFormulario), new { idFormulario = relacion.IdFormulario }, relacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        //  Eliminar una relación Pregunta-Formulario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreguntaFormulario(int id)
        {
            try
            {
                var relacion = await _context.FormulariosPreguntas.FindAsync(id);
                if (relacion == null)
                    return NotFound(new { message = "Relación no encontrada." });

                _context.FormulariosPreguntas.Remove(relacion);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Relación eliminada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
