using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    public class FactorService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://savalapi.somee.com/api/FactorRiesgo";

        public FactorService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        // Agregar un factor de riesgo
        public async Task<bool> AddFactorAsync(FactorRiesgo nuevoFactor)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("FactorRiesgo", nuevoFactor);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al agregar factor de riesgo: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        // Obtener la lista de factores de riesgo
        public async Task<List<FactorRiesgo>> GetFactoresAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<FactorRiesgo>>("FactorRiesgo") ?? new List<FactorRiesgo>();
            }
            catch
            {
                return new List<FactorRiesgo>();
            }
        }

        // Eliminar (deshabilitar) un factor de riesgo (Soft Delete)
        public async Task<bool> DeleteFactorAsync(FactorRiesgo factor)
        {
            try
            {
                var url = $"FactorRiesgo/{factor.IdFactor}";
                Console.WriteLine($"[DELETE] Llamando a: {url}");

                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al eliminar factor de riesgo: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        // Actualizar un factor de riesgo
        public async Task<bool> UpdateFactorAsync(FactorRiesgo factor)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"FactorRiesgo/{factor.IdFactor}", factor);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        // ✅ Método para obtener un factor de riesgo por su ID
        public async Task<FactorRiesgo> GetFactorByIdAsync(int idFactor)
        {
            try
            {
                var response = await _httpClient.GetAsync($"FactorRiesgo/{idFactor}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return System.Text.Json.JsonSerializer.Deserialize<FactorRiesgo>(json, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API (FactorRiesgo): {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetFactorByIdAsync: {ex.Message}");
                return null;
            }
        }

    }
}
