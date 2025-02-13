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
    public class FormularioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FormularioController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los formularios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Formulario>>> GetFormularios()
        {
            try
            {
                var formularios = await _context.Formularios.ToListAsync();
                return Ok(formularios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Obtener un formulario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Formulario>> GetFormulario(int id)
        {
            try
            {
                var formulario = await _context.Formularios.FindAsync(id);

                if (formulario == null)
                {
                    return NotFound(new { message = "Formulario no encontrado." });
                }

                return Ok(formulario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Crear un nuevo formulario
        [HttpPost]
        public async Task<ActionResult<Formulario>> PostFormulario(Formulario formulario)
        {
            try
            {
                _context.Formularios.Add(formulario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetFormulario), new { id = formulario.IdFormulario }, formulario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // Editar un formulario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormulario(int id, Formulario formulario)
        {
            try
            {
                if (id != formulario.IdFormulario)
                {
                    return BadRequest(new { message = "El ID no coincide." });
                }

                _context.Entry(formulario).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Formulario actualizado con éxito." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Formularios.Any(f => f.IdFormulario == id))
                {
                    return NotFound(new { message = "Formulario no encontrado." });
                }
                return StatusCode(500, new { message = "Error de concurrencia en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        //  Eliminar un formulario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormulario(int id)
        {
            try
            {
                var formulario = await _context.Formularios.FindAsync(id);

                if (formulario == null)
                {
                    return NotFound(new { message = "Formulario no encontrado." });
                }

                _context.Formularios.Remove(formulario);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Formulario eliminado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
