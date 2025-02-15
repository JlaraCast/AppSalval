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
        return await _httpClient.GetFromJsonAsync<List<Usuario>>($"{BaseUrl}/usuarios");
    }

    // Agregar un usuario
    public async Task<bool> AddUsuarioAsync(Usuario nuevoUsuario)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/usuarios", nuevoUsuario);
        return response.IsSuccessStatusCode;
    }

    // Eliminar un usuario
    public async Task<bool> DeleteUsuarioAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/usuarios/{id}");
        return response.IsSuccessStatusCode;
    }

    // Método para obtener y eliminar un usuario al mismo tiempo
    public async Task<(bool isDeleted, List<Usuario> usuarios)> DeleteAndRefreshUsuariosAsync(int id)
    {
        bool isDeleted = await DeleteUsuarioAsync(id);
        List<Usuario> usuarios = isDeleted ? await GetUsuariosAsync() : null;
        return (isDeleted, usuarios);
    }

}
