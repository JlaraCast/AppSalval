using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{

    public class RecomService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://savalapi.somee.com/api/Recomendacion";

        public RecomService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        public async Task<bool> AddRecomendacionAsync(Recomendacion nuevaRecomendacion)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Recomendacion", nuevaRecomendacion);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al agregar recomendación: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Recomendacion>> GetRecomendacionesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Recomendacion>>("Recomendacion") ?? new List<Recomendacion>();
            }
            catch
            {
                return new List<Recomendacion>();
            }
        }

        public async Task<bool> DeleteRecomendacionAsync(Recomendacion recomendacion)
        {
            try
            {
                var url = $"Recomendacion/{recomendacion.IdRecomendacion}";
                Console.WriteLine($"[DELETE] Llamando a: {url}");

                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al eliminar recomendación: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateRecomendacionAsync(Recomendacion recomendacion)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Recomendacion/{recomendacion.IdRecomendacion}", recomendacion);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        // ✅ Método para obtener una recomendación por su ID
        public async Task<Recomendacion> GetRecomendacionByIdAsync(int idRecomendacion)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Recomendacion/{idRecomendacion}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return System.Text.Json.JsonSerializer.Deserialize<Recomendacion>(json, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API (Recomendacion): {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetRecomendacionByIdAsync: {ex.Message}");
                return null;
            }
        }

    }
}
