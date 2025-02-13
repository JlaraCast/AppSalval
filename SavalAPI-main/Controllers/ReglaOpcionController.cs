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
    public class ReglaOpcionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReglaOpcionController(AppDbContext context)
        {
            _context = context;
        }

        // OBTENER TODAS LAS REGLAS OPCIÓN
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReglaOpcion>>> GetReglasOpciones()
        {
            try
            {
                return await _context.ReglasOpciones
                   // .Include(ro => ro.Opcion)
                   // .Include(ro => ro.Recomendacion)
                  //  .Include(ro => ro.FactorRiesgo)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // OBTENER UNA REGLA OPCIÓN POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ReglaOpcion>> GetReglaOpcion(int id)
        {
            try
            {
                var reglaOpcion = await _context.ReglasOpciones
                    .Include(ro => ro.Opcion)
                    .Include(ro => ro.Recomendacion)
                    .Include(ro => ro.FactorRiesgo)
                    .FirstOrDefaultAsync(ro => ro.IdRegla == id);

                if (reglaOpcion == null)
                    return NotFound(new { message = "ReglaOpcion no encontrada." });

                return Ok(reglaOpcion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        [HttpGet("opcion/{idOpcion}/recomendaciones")]
        public async Task<ActionResult<IEnumerable<Recomendacion>>> GetRecomendacionesPorOpcion(int idOpcion)
        {
            try
            {
                // Obtener todas las recomendaciones asociadas a la opción de respuesta
                var recomendaciones = await _context.ReglasOpciones
                    .Where(ro => ro.IdOpcion == idOpcion && ro.IdRecomendacion != null)
                    .Select(ro => ro.Recomendacion)
                    .ToListAsync();

                if (!recomendaciones.Any())
                {
                    return NotFound(new { message = "No hay recomendaciones asociadas a esta opción." });
                }

                return Ok(recomendaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // CREAR UNA NUEVA REGLA OPCIÓN
        [HttpPost]
        public async Task<ActionResult<ReglaOpcion>> PostReglaOpcion(ReglaOpcion reglaOpcion)
        {
            try
            {
                // Validar que la opción de respuesta existe
                var opcionExistente = await _context.OpcionesRespuestas.FindAsync(reglaOpcion.IdOpcion);
                if (opcionExistente == null)
                    return BadRequest(new { message = "La opción de respuesta especificada no existe." });

                // Validar si se envió IdRecomendacion y que la recomendación exista
                if (reglaOpcion.IdRecomendacion.HasValue)
                {
                    var recomendacionExistente = await _context.Recomendaciones.FindAsync(reglaOpcion.IdRecomendacion);
                    if (recomendacionExistente == null)
                        return BadRequest(new { message = "La recomendación especificada no existe." });
                }

                // Validar si se envió IdFactorRiesgo y que el factor de riesgo exista
                if (reglaOpcion.IdFactorRiesgo.HasValue)
                {
                    var factorRiesgoExistente = await _context.FactoresRiesgo.FindAsync(reglaOpcion.IdFactorRiesgo);
                    if (factorRiesgoExistente == null)
                        return BadRequest(new { message = "El factor de riesgo especificado no existe." });
                }

                // Agregar la regla a la base de datos
                _context.ReglasOpciones.Add(reglaOpcion);
                await _context.SaveChangesAsync();

                // Responder solo con los datos esenciales
                var response = new
                {
                    idRegla = reglaOpcion.IdRegla,
                    idOpcion = reglaOpcion.IdOpcion,
                    idRecomendacion = reglaOpcion.IdRecomendacion,
                    idFactorRiesgo = reglaOpcion.IdFactorRiesgo,
                    condicion = reglaOpcion.Condicion
                };

                return CreatedAtAction(nameof(GetReglaOpcion), new { id = reglaOpcion.IdRegla }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        // ACTUALIZAR UNA REGLA OPCIÓN
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReglaOpcion(int id, ReglaOpcion reglaOpcion)
        {
            try
            {
                if (id != reglaOpcion.IdRegla)
                    return BadRequest(new { message = "El ID de la regla opción no coincide." });

                // Validar existencia de la regla opción
                var reglaExistente = await _context.ReglasOpciones.FindAsync(id);
                if (reglaExistente == null)
                    return NotFound(new { message = "ReglaOpcion no encontrada." });

                // Validar que la opción de respuesta existe
                var opcionExistente = await _context.OpcionesRespuestas.FindAsync(reglaOpcion.IdOpcion);
                if (opcionExistente == null)
                    return BadRequest(new { message = "La opción de respuesta especificada no existe." });

                // Validar recomendación y factor de riesgo (si se especifican)
                if (reglaOpcion.IdRecomendacion.HasValue)
                {
                    var recomendacionExistente = await _context.Recomendaciones.FindAsync(reglaOpcion.IdRecomendacion);
                    if (recomendacionExistente == null)
                        return BadRequest(new { message = "La recomendación especificada no existe." });
                }

                if (reglaOpcion.IdFactorRiesgo.HasValue)
                {
                    var factorRiesgoExistente = await _context.FactoresRiesgo.FindAsync(reglaOpcion.IdFactorRiesgo);
                    if (factorRiesgoExistente == null)
                        return BadRequest(new { message = "El factor de riesgo especificado no existe." });
                }

                _context.Entry(reglaOpcion).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Regla opción actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ELIMINAR UNA REGLA OPCIÓN
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReglaOpcion(int id)
        {
            try
            {
                var reglaOpcion = await _context.ReglasOpciones.FindAsync(id);

                if (reglaOpcion == null)
                    return NotFound(new { message = "ReglaOpcion no encontrada." });

                _context.ReglasOpciones.Remove(reglaOpcion);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Regla opción eliminada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
