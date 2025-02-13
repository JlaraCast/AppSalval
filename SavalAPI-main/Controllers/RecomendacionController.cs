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
    public class RecomendacionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecomendacionController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las recomendaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recomendacion>>> GetRecomendaciones()
        {
            try
            {
                var recomendaciones = await _context.Recomendaciones.ToListAsync();
                return Ok(recomendaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Obtener una recomendación por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Recomendacion>> GetRecomendacion(int id)
        {
            try
            {
                var recomendacion = await _context.Recomendaciones.FindAsync(id);

                if (recomendacion == null)
                {
                    return NotFound(new { message = "Recomendación no encontrada." });
                }

                return Ok(recomendacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }



        // Obtener todas las recomendaciones habilitadas
        [HttpGet("habilitadas")]
        public async Task<ActionResult<IEnumerable<Recomendacion>>> GetRecomendacionesHabilitadas()
        {
            try
            {
                var recomendaciones = await _context.Recomendaciones
                    .Where(r => r.Habilitado) // 
                    .ToListAsync();

                return Ok(recomendaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Obtener una recomendación por ID (solo si está habilitada)
        [HttpGet("habilitada/{id}")]
        public async Task<ActionResult<Recomendacion>> GetRecomendacionHabilitada(int id)
        {
            try
            {
                var recomendacion = await _context.Recomendaciones
                    .Where(r => r.Habilitado) //
                    .FirstOrDefaultAsync(r => r.IdRecomendacion == id);

                if (recomendacion == null)
                {
                    return NotFound(new { message = "Recomendación no encontrada o está deshabilitada." });
                }

                return Ok(recomendacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        // Crear una nueva recomendación
        [HttpPost]
        public async Task<ActionResult<Recomendacion>> PostRecomendacion(Recomendacion recomendacion)
        {
            try
            {
                _context.Recomendaciones.Add(recomendacion);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRecomendacion), new { id = recomendacion.IdRecomendacion }, recomendacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Editar una recomendación
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecomendacion(int id, Recomendacion recomendacion)
        {
            try
            {
                if (id != recomendacion.IdRecomendacion)
                {
                    return BadRequest(new { message = "El ID no coincide." });
                }

                _context.Entry(recomendacion).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Recomendación actualizada con éxito." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Recomendaciones.Any(r => r.IdRecomendacion == id))
                {
                    return NotFound(new { message = "Recomendación no encontrada." });
                }
                return StatusCode(500, new { message = "Error de concurrencia en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Eliminar una recomendación
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecomendacion(int id)
        {
            try
            {
                var recomendacion = await _context.Recomendaciones.FindAsync(id);

                if (recomendacion == null)
                    return NotFound(new { message = "Recomendación no encontrada." });

                recomendacion.Habilitado = false; //Soft Delete (no se elimina, solo se oculta)
                await _context.SaveChangesAsync();

                return Ok(new { message = "Recomendación deshabilitada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
