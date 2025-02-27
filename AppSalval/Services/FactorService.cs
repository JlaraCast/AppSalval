using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    /// <summary>
    /// Servicio para gestionar los factores de riesgo.
    /// </summary>
    public class FactorService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://savalapi.somee.com/api/FactorRiesgo";

        /// <summary>
        /// Constructor de la clase FactorService.
        /// </summary>
        public FactorService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        /// <summary>
        /// Agrega un nuevo factor de riesgo.
        /// </summary>
        /// <param name="nuevoFactor">El nuevo factor de riesgo a agregar.</param>
        /// <returns>True si el factor fue agregado exitosamente, false en caso contrario.</returns>
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

        /// <summary>
        /// Obtiene la lista de factores de riesgo.
        /// </summary>
        /// <returns>Una lista de factores de riesgo.</returns>
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

        /// <summary>
        /// Elimina (deshabilita) un factor de riesgo (Soft Delete).
        /// </summary>
        /// <param name="factor">El factor de riesgo a eliminar.</param>
        /// <returns>True si el factor fue eliminado exitosamente, false en caso contrario.</returns>
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

        /// <summary>
        /// Actualiza un factor de riesgo.
        /// </summary>
        /// <param name="factor">El factor de riesgo a actualizar.</param>
        /// <returns>True si el factor fue actualizado exitosamente, false en caso contrario.</returns>
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

        /// <summary>
        /// Obtiene un factor de riesgo por su ID.
        /// </summary>
        /// <param name="idFactor">El ID del factor de riesgo.</param>
        /// <returns>El factor de riesgo correspondiente al ID proporcionado.</returns>
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
