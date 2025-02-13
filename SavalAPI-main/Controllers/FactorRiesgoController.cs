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
    public class FactorRiesgoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FactorRiesgoController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los factores de riesgo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FactorRiesgo>>> GetFactoresRiesgo()
        {
            try
            {
                var factores = await _context.FactoresRiesgo.ToListAsync();
                return Ok(factores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Obtener solo los factores de riesgo habilitados
        [HttpGet("habilitados")]
        public async Task<ActionResult<IEnumerable<FactorRiesgo>>> GetFactoresRiesgoHabilitados()
        {
            try
            {
                var factores = await _context.FactoresRiesgo
                    .Where(f => f.Habilitado) // Filtrar solo habilitados
                    .ToListAsync();
                return Ok(factores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Obtener un factor de riesgo por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FactorRiesgo>> GetFactorRiesgo(int id)
        {
            try
            {
                var factor = await _context.FactoresRiesgo.FindAsync(id);

                if (factor == null)
                {
                    return NotFound(new { message = "Factor de riesgo no encontrado." });
                }

                return Ok(factor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Obtener un factor de riesgo por ID (Solo si está habilitado)
        [HttpGet("habilitado/{id}")]
        public async Task<ActionResult<FactorRiesgo>> GetFactorRiesgoHabilitado(int id)
        {
            try
            {
                var factor = await _context.FactoresRiesgo
                    .Where(f => f.Habilitado)
                    .FirstOrDefaultAsync(f => f.IdFactor == id);

                if (factor == null)
                {
                    return NotFound(new { message = "Factor de riesgo no encontrado o está deshabilitado." });
                }

                return Ok(factor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Crear un nuevo factor de riesgo
        [HttpPost]
        public async Task<ActionResult<FactorRiesgo>> PostFactorRiesgo(FactorRiesgo factor)
        {
            try
            {
                _context.FactoresRiesgo.Add(factor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetFactorRiesgo), new { id = factor.IdFactor }, factor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Editar un factor de riesgo
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactorRiesgo(int id, FactorRiesgo factor)
        {
            try
            {
                if (id != factor.IdFactor)
                {
                    return BadRequest(new { message = "El ID no coincide." });
                }

                _context.Entry(factor).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Factor de riesgo actualizado con éxito." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FactoresRiesgo.Any(f => f.IdFactor == id))
                {
                    return NotFound(new { message = "Factor de riesgo no encontrado." });
                }
                return StatusCode(500, new { message = "Error de concurrencia en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Soft delete: Marcar como deshabilitado en lugar de eliminar
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteFactorRiesgo(int id)
        {
            try
            {
                var factor = await _context.FactoresRiesgo.FindAsync(id);

                if (factor == null)
                {
                    return NotFound(new { message = "Factor de riesgo no encontrado." });
                }

                factor.Habilitado = false; // Deshabilitar en lugar de eliminar
                await _context.SaveChangesAsync();

                return Ok(new { message = "Factor de riesgo deshabilitado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
