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
    public class ApiServiceRespuestas
    {
        // Cliente HTTP para conectar con la API
        private readonly HttpClient _httpClient;

        // Constructor: Configura la URL base de la API
        public ApiServiceRespuestas()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://savalapi.somee.com/api/Respuesta"); // ⚠️ Cambia la URL si es diferente
        }

        // ✅ Método para obtener la lista de respuestas desde la API
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

        // ✅ Método para obtener una respuesta por su ID
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

        // ✅ Método para editar una respuesta existente
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
