using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavalAPI.Data;
using SavalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SavalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpcionRespuestaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OpcionRespuestaController(AppDbContext context)
        {
            _context = context;
        }

        // OBTENER TODAS LAS OPCIONES RESPUESTA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OpcionRespuesta>>> GetOpcionesRespuesta()
        {
            try
            {
                return await _context.OpcionesRespuestas
                    //.Include(o => o.Pregunta)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        //  OBTENER UNA OPCIÓN RESPUESTA POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OpcionRespuesta>> GetOpcionRespuesta(int id)
        {
            try
            {
                var opcionRespuesta = await _context.OpcionesRespuestas
                    //.Include(o => o.Pregunta)
                    .FirstOrDefaultAsync(o => o.IdOpcion == id);

                if (opcionRespuesta == null)
                    return NotFound(new { message = "Opción de respuesta no encontrada." });

                return Ok(opcionRespuesta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // CREAR UNA NUEVA OPCIÓN RESPUESTA
        [HttpPost]
        public async Task<ActionResult<OpcionRespuesta>> PostOpcionRespuesta(OpcionRespuesta opcionRespuesta)
        {
            try
            {
                // Validar que la pregunta existe antes de asignarla
                var preguntaExistente = await _context.Preguntas.FindAsync(opcionRespuesta.IdPregunta);
                if (preguntaExistente == null)
                    return BadRequest(new { message = "La pregunta especificada no existe." });

                _context.OpcionesRespuestas.Add(opcionRespuesta);
                await _context.SaveChangesAsync();

                // Devolver solo los datos esenciales
                var response = new
                {
                    idOpcion = opcionRespuesta.IdOpcion,
                    nombreOpcion = opcionRespuesta.NombreOpcion,
                    idPregunta = opcionRespuesta.IdPregunta
                };

                return CreatedAtAction(nameof(GetOpcionRespuesta), new { id = opcionRespuesta.IdOpcion }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // OBTENER TODAS LAS OPCIONES QUE PERTENECEN A UNA PREGUNTA POR IdPregunta
        [HttpGet("por-pregunta/{idPregunta}")]
        public async Task<ActionResult<IEnumerable<OpcionRespuesta>>> GetOpcionesPorPregunta(int idPregunta)
        {
            try
            {
                var opciones = await _context.OpcionesRespuestas
                    .Where(o => o.IdPregunta == idPregunta)
                    .ToListAsync();

                if (!opciones.Any())
                    return NotFound(new { message = "No se encontraron opciones para la pregunta especificada." });

                return Ok(opciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        //  ELIMINAR UNA OPCIÓN RESPUESTA
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOpcionRespuesta(int id)
        {
            try
            {
                var opcionRespuesta = await _context.OpcionesRespuestas.FindAsync(id);

                if (opcionRespuesta == null)
                    return NotFound(new { message = "Opción de respuesta no encontrada." });

                _context.OpcionesRespuestas.Remove(opcionRespuesta);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Opción de respuesta eliminada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
