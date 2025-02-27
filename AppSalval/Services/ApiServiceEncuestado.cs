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
    /// Servicio para interactuar con la API de Encuestados.
    /// </summary>
    public class ApiServiceEncuestado
    {
        // Cliente HTTP para conectar con la API
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor: Configura la URL base de la API.
        /// </summary>
        public ApiServiceEncuestado()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://savalapi.somee.com/api/Encuestado"); // ⚠️ Cambia la URL si es diferente
        }

        /// <summary>
        /// Obtiene un encuestado por su ID.
        /// </summary>
        /// <param name="idEncuestado">ID del encuestado.</param>
        /// <returns>EncuestadoDto</returns>
        public async Task<EncuestadoDto> GetEncuestado(string idEncuestado)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Encuestado/{idEncuestado}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<EncuestadoDto>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetEncuestado: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene todos los encuestados.
        /// </summary>
        /// <returns>Lista de EncuestadoDto</returns>
        public async Task<List<EncuestadoDto>> GetEncuestados()
        {
            try
            {
                var response = await _httpClient.GetAsync("Encuestado");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<List<EncuestadoDto>>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetEncuestados: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Edita un encuestado existente.
        /// </summary>
        /// <param name="encuestado">Objeto encuestado con los datos actualizados.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario False.</returns>
        public async Task<bool> EditEncuestado(EncuestadoDto encuestado)
        {
            try
            {
                // Verificar si el encuestado existe
                var existingEncuestado = await GetEncuestado(encuestado.Identificacion);
                if (existingEncuestado == null)
                {
                    Console.WriteLine($"⚠️ Encuestado con ID {encuestado.Identificacion} no encontrado.");
                    return false;
                }

                // Convertir el encuestado a JSON
                var json = JsonSerializer.Serialize(encuestado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT a la API
                var response = await _httpClient.PutAsync($"Encuestado/{encuestado.Identificacion}", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Encuestado con ID {encuestado.Identificacion} actualizado correctamente.");
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
                Console.WriteLine($"❌ Error en EditEncuestado: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina un encuestado por su ID.
        /// </summary>
        /// <param name="idEncuestado">ID del encuestado a eliminar.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario False.</returns>
        public async Task<bool> DeleteEncuestadoAsync(int idEncuestado)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Encuestado/{idEncuestado}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al eliminar encuestado: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Agrega un nuevo encuestado.
        /// </summary>
        /// <param name="encuestado">Objeto encuestado a agregar.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario False.</returns>
        public async Task<bool> AddEncuestado(EncuestadoDto encuestado)
        {
            try
            {
                // Convertir el objeto encuestado a JSON
                var json = JsonSerializer.Serialize(encuestado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST a la API
                var response = await _httpClient.PostAsync("Encuestado", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Encuestado con ID {encuestado.Identificacion} agregado correctamente.");
                    return true;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"⚠️ Error al agregar encuestado: {response.StatusCode}, {errorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en AddEncuestado: {ex.Message}");
                return false;
            }
        }

    }
}