using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API;
using AppSalval.Models_Api;
namespace AppSalval.Services
{
    /// <summary>
    /// Servicio para interactuar con la API de respuestas.
    /// </summary>
    public class ApiServiceRespuestas
    {
        // Cliente HTTP para conectar con la API
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor: Configura la URL base de la API.
        /// </summary>
        public ApiServiceRespuestas()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://savalapi.somee.com/api/Respuesta"); // ⚠️ Cambia la URL si es diferente
        }

        /// <summary>
        /// Obtiene la lista de respuestas desde la API.
        /// </summary>
        /// <returns>Lista de respuestas.</returns>
        public async Task<List<RespuestasDTO>> GetRespuestas()
        {
            try
            {
                var response = await _httpClient.GetAsync("Respuesta");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<List<RespuestasDTO>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetRespuestas: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene una respuesta por su ID.
        /// </summary>
        /// <param name="id">ID de la respuesta.</param>
        /// <returns>Respuesta correspondiente al ID.</returns>
        public async Task<RespuestasDTO> GetRespuestaById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Respuesta/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<RespuestasDTO>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetRespuestaById: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Edita una respuesta existente.
        /// </summary>
        /// <param name="respuesta">Objeto de respuesta a editar.</param>
        /// <returns>Verdadero si la edición fue exitosa, falso en caso contrario.</returns>
        public async Task<bool> EditRespuesta(RespuestasDTO respuesta)
        {
            try
            {
                // Verificar si la respuesta existe
                var existingRespuesta = await GetRespuestaById(respuesta.IdRespuesta);
                if (existingRespuesta == null)
                {
                    Console.WriteLine($"⚠️ Respuesta con ID {respuesta.IdRespuesta} no encontrada.");
                    return false;
                }

                // Convertir la respuesta a JSON
                var json = JsonSerializer.Serialize(respuesta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT a la API
                var response = await _httpClient.PutAsync($"Respuesta/{respuesta.IdRespuesta}", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Respuesta con ID {respuesta.IdRespuesta} actualizada correctamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en EditRespuesta: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina una respuesta por su ID.
        /// </summary>
        /// <param name="idRespuesta">ID de la respuesta a eliminar.</param>
        /// <returns>Verdadero si la eliminación fue exitosa, falso en caso contrario.</returns>
        public async Task<bool> DeleteRespuestaAsync(int idRespuesta)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Respuesta/{idRespuesta}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al eliminar respuesta: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }
    }
}
