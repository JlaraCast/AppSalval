using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class UsuarioController
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://savalapi.somee.com/api/Usuario";

    public UsuarioController()
    {
        _httpClient = new HttpClient();
    }

    // Obtener lista de usuarios
    public async Task<List<Usuario>> GetUsuariosAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<Usuario>>(BaseUrl);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
            return null;
        }
    }

    // Agregar un usuario
    public async Task<bool> AddUsuarioAsync(Usuario nuevoUsuario)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, nuevoUsuario);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error al agregar usuario: {ex.Message}");
            return false;
        }
    }

    // Eliminar un usuario
    public async Task<bool> DeleteUsuarioAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
            return false;
        }
    }

    // Método para obtener y eliminar un usuario al mismo tiempo
    public async Task<(bool isDeleted, List<Usuario> usuarios)> DeleteAndRefreshUsuariosAsync(int id)
    {
        bool isDeleted = await DeleteUsuarioAsync(id);
        List<Usuario> usuarios = isDeleted ? await GetUsuariosAsync() : null;
        return (isDeleted, usuarios);
    }
}