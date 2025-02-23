using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class UserService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://savalapi.somee.com/api/";

    public UserService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

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

    public async Task<bool> ChangePasswordAsync(int userId, string nuevaContrasena)
    {
        try
        {
            // Crear el objeto que contiene los datos de la contraseña
            var data = new
            {
                us= userId,
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