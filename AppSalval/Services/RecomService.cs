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

        /// <summary>
        /// Constructor de la clase RecomService.
        /// Inicializa el HttpClient con la URL base de la API.
        /// </summary>
        public RecomService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        /// <summary>
        /// Agrega una nueva recomendación a través de una solicitud POST a la API.
        /// </summary>
        /// <param name="nuevaRecomendacion">La nueva recomendación a agregar.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario false.</returns>
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

        /// <summary>
        /// Obtiene la lista de recomendaciones a través de una solicitud GET a la API.
        /// </summary>
        /// <returns>Una lista de recomendaciones.</returns>
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

        /// <summary>
        /// Elimina una recomendación a través de una solicitud DELETE a la API.
        /// </summary>
        /// <param name="recomendacion">La recomendación a eliminar.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario false.</returns>
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

        /// <summary>
        /// Actualiza una recomendación a través de una solicitud PUT a la API.
        /// </summary>
        /// <param name="recomendacion">La recomendación a actualizar.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario false.</returns>
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

        /// <summary>
        /// Obtiene una recomendación por su ID a través de una solicitud GET a la API.
        /// </summary>
        /// <param name="idRecomendacion">El ID de la recomendación a obtener.</param>
        /// <returns>La recomendación obtenida o null si no se encuentra.</returns>
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
