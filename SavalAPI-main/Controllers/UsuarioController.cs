using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavalAPI.Data;
using SavalAPI.Models;

namespace SavalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        }

        // Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            try
            {
                // Buscar el usuario con el Rol asociado
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.IdUsuario == id);

                // Verificar si el usuario existe
                if (usuario == null)
                {
                    return NotFound(new { message = "El usuario especificado no existe." });
                }

                // Devolver el usuario encontrado
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        // Crear un nuevo usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                // Validar si el rol existe en la base de datos
                var existingRol = await _context.Roles.FindAsync(usuario.IdRol);
                if (existingRol == null)
                {
                    return BadRequest(new { message = "El rol especificado no existe en la base de datos." });
                }

                // Asignar el objeto Rol existente basado en el IdRol
                usuario.Rol = existingRol;

                // Agregar el usuario a la base de datos
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            try
            {
                // Verificar si el usuario existe
                var existingUsuario = await _context.Usuarios.FindAsync(id);
                if (existingUsuario == null)
                {
                    return NotFound("El usuario especificado no existe.");
                }

                // Validar que el IdRol proporcionado existe en la base de datos
                var existingRol = await _context.Roles.FindAsync(usuario.IdRol);
                if (existingRol == null)
                {
                    return BadRequest("El rol especificado no existe en la base de datos.");
                }

                // Actualizar los campos del usuario existente
                existingUsuario.Correo = usuario.Correo;
                existingUsuario.Contraseña = usuario.Contraseña;
                existingUsuario.IdRol = usuario.IdRol;

                // Opcional: Asociar el objeto Rol (solo para devolverlo en la respuesta)
                existingUsuario.Rol = existingRol;

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuario actualizado con éxito." }); // Respuesta sin contenido al actualizar con éxito
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                // Buscar el usuario en la base de datos
                var usuario = await _context.Usuarios.FindAsync(id);

                // Verificar si el usuario existe
                if (usuario == null)
                {
                    return NotFound(new { message = "El usuario especificado no existe." });
                }

                // Eliminar el usuario
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                // Respuesta de éxito personalizada
                return Ok(new { message = "Usuario eliminado con éxito." });
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] string correo, [FromQuery] string contraseña)
        {
            try
            {
                // Buscar el usuario por correo
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Correo == correo);

                if (usuario == null || usuario.Contraseña != contraseña)
                {
                    return Unauthorized(new { message = "Correo o contraseña incorrectos." });
                }

                // Construir la respuesta con el rol del usuario
                return Ok(new
                {
                    message = "Inicio de sesión exitoso.",
                    usuario = new
                    {
                        idUsuario = usuario.IdUsuario,
                        correo = usuario.Correo,
                        idRol = usuario.IdRol,
                        rol = usuario.Rol.NombreRol // Retornar el rol aquí
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

    }
}
