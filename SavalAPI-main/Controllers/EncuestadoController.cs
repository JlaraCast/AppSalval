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
    public class EncuestadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EncuestadoController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los encuestados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Encuestado>>> GetEncuestados()
        {
            try
            {
                var encuestados = await _context.Encuestados.ToListAsync();
                return Ok(encuestados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        //  Obtener un encuestado por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Encuestado>> GetEncuestado(string id)
        {
            try
            {
                var encuestado = await _context.Encuestados.FindAsync(id);

                if (encuestado == null)
                {
                    return NotFound(new { message = "Encuestado no encontrado." });
                }

                return Ok(encuestado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Encuestado>> PostEncuestado(Encuestado encuestado)
        {
            try
            {
                // Verificar que la altura no sea 0 para evitar división por cero
                if (encuestado.Altura <= 0)
                {
                    return BadRequest(new { message = "La altura debe ser mayor que 0." });
                }

                // Calcular el IMC antes de guardar
                encuestado.IMC = Math.Round(encuestado.Peso / (encuestado.Altura * encuestado.Altura), 2);

                _context.Encuestados.Add(encuestado);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEncuestado), new { id = encuestado.Identificacion }, encuestado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        // Editar un encuestado (Recalcula IMC si la altura o peso cambian)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEncuestado(string id, Encuestado encuestado)
        {
            try
            {
                if (id != encuestado.Identificacion)
                {
                    return BadRequest(new { message = "El ID no coincide." });
                }

                // Recalcular el IMC antes de guardar
                encuestado.IMC = encuestado.Peso / (encuestado.Altura * encuestado.Altura);

                _context.Entry(encuestado).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Encuestado actualizado con éxito." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Encuestados.Any(e => e.Identificacion == id))
                {
                    return NotFound(new { message = "Encuestado no encontrado." });
                }
                return StatusCode(500, new { message = "Error de concurrencia en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        //  Eliminar un encuestado
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEncuestado(string id)
        {
            try
            {
                var encuestado = await _context.Encuestados.FindAsync(id);

                if (encuestado == null)
                {
                    return NotFound(new { message = "Encuestado no encontrado." });
                }

                _context.Encuestados.Remove(encuestado);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Encuestado eliminado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
