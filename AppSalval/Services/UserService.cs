using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class UserService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://savalapi.somee.com/api/";

    /// <summary>
    /// Constructor de la clase UserService.
    /// Inicializa una nueva instancia de HttpClient con la URL base.
    /// </summary>
    public UserService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    /// <summary>
    /// Agrega un nuevo usuario.
    /// </summary>
    /// <param name="newUser">El objeto Usuario que se va a agregar.</param>
    /// <returns>True si el usuario se agregó correctamente, de lo contrario false.</returns>
    public async Task<bool> AddUserAsync(Usuario newUser)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Usuario", newUser);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error al agregar usuario: {errorMessage}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Obtiene la lista de usuarios.
    /// </summary>
    /// <returns>Una lista de objetos Usuario.</returns>
    public async Task<List<Usuario>> GetUsersAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<Usuario>>("Usuario");
        }
        catch
        {
            return new List<Usuario>();
        }
    }

    /// <summary>
    /// Elimina un usuario por su ID.
    /// </summary>
    /// <param name="userId">El ID del usuario que se va a eliminar.</param>
    /// <returns>True si el usuario se eliminó correctamente, de lo contrario false.</returns>
    public async Task<bool> DeleteUserAsync(int userId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Usuario/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error al eliminar usuario: {errorMessage}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Actualiza un usuario existente.
    /// </summary>
    /// <param name="usuario">El objeto Usuario con los datos actualizados.</param>
    /// <returns>True si el usuario se actualizó correctamente, de lo contrario false.</returns>
    public async Task<bool> UpdateUserAsync(Usuario usuario)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"Usuario/{usuario.IdUsuario}", usuario);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Obtiene un usuario por su ID.
    /// </summary>
    /// <param name="userId">El ID del usuario que se va a obtener.</param>
    /// <returns>El objeto Usuario si se encuentra, de lo contrario null.</returns>
    public async Task<Usuario> GetUserByIdAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Usuario/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Usuario>();
            }

            return null; // Si no se encuentra el usuario, devolvemos null
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            return null; // En caso de error en la conexión, devolvemos null
        }
    }

    /// <summary>
    /// Cambia la contraseña de un usuario.
    /// </summary>
    /// <param name="userId">El ID del usuario cuya contraseña se va a cambiar.</param>
    /// <param name="nuevaContrasena">La nueva contraseña.</param>
    /// <returns>True si la contraseña se cambió correctamente, de lo contrario false.</returns>
    public async Task<bool> ChangePasswordAsync(int userId, string nuevaContrasena)
    {
        try
        {
            // Crear el objeto que contiene los datos de la contraseña
            var data = new
            {
                us = userId,
                nuevaContrasena = nuevaContrasena
            };

            // Hacer la solicitud POST para cambiar la contraseña
            var response = await _httpClient.PostAsJsonAsync("Usuario/cambiarContrasena", data);

            // Verificar si la respuesta es exitosa
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error al cambiar la contraseña: {errorMessage}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            return false;
        }
    }
}
