using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    /// <summary>
    /// Servicio para interactuar con la API de ReglaOpcion.
    /// </summary>
    public class ApiServiceReglaOpcion
    {
        // Cliente HTTP para conectar con la API
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor: Configura la URL base de la API.
        /// </summary>
        public ApiServiceReglaOpcion()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://savalapi.somee.com/api/ReglaOpcion"); // ⚠️ Cambia la URL si es diferente
        }

        /// <summary>
        /// Obtiene la lista de reglas de opción desde la API.
        /// </summary>
        /// <returns>Lista de objetos ReglaOpcionDto.</returns>
        public async Task<List<ReglaOpcionDto>> GetReglaOpciones()
        {
            try
            {
                var response = await _httpClient.GetAsync("ReglaOpcion");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<List<ReglaOpcionDto>>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetReglaOpciones: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene una lista de reglas de opción por el ID de la regla.
        /// </summary>
        /// <param name="idRegla">ID de la regla.</param>
        /// <returns>Lista de objetos ReglaOpcionDto.</returns>
        public async Task<List<ReglaOpcionDto>> GetReglaOpcionByIdRegla(int idRegla)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ReglaOpcion/{idRegla}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Para depuración

                    var reglasOpcion = JsonSerializer.Deserialize<List<ReglaOpcionDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignorar mayúsculas/minúsculas en los nombres de propiedades
                    });

                    return reglasOpcion; // Devolver la lista deserializada
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetReglaOpcionByIdRegla: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene una lista de reglas de opción por el ID de la opción.
        /// </summary>
        /// <param name="idOpcion">ID de la opción.</param>
        /// <returns>Lista de objetos ReglaOpcionDto.</returns>
        public async Task<List<ReglaOpcionDto>> GetReglaOpcionByOpcionId(int idOpcion)
        {
            try
            {
                var response = await _httpClient.GetAsync("ReglaOpcion");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Para depuración

                    var reglasOpcion = JsonSerializer.Deserialize<List<ReglaOpcionDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignorar mayúsculas/minúsculas en los nombres de propiedades
                    });

                    var reglasFiltradas = reglasOpcion?.Where(r => r.IdOpcion == idOpcion).ToList();

                    return reglasFiltradas; // Devolver la lista filtrada
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetReglaOpcionByOpcionId: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Edita una regla de opción existente.
        /// </summary>
        /// <param name="reglaOpcion">Objeto ReglaOpcionDto con los datos a editar.</param>
        /// <returns>True si la edición fue exitosa, false en caso contrario.</returns>
        public async Task<bool> EditReglaOpcion(ReglaOpcionDto reglaOpcion)
        {
            try
            {
                var existingReglaOpcion = await GetReglaOpcionByIdRegla(reglaOpcion.IdRegla);
                if (existingReglaOpcion == null)
                {
                    Console.WriteLine($"⚠️ Regla de opción con ID {reglaOpcion.IdRegla} no encontrada.");
                    return false;
                }

                var json = JsonSerializer.Serialize(reglaOpcion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"ReglaOpcion/{reglaOpcion.IdRegla}", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Regla de opción con ID {reglaOpcion.IdRegla} actualizada correctamente.");
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
                Console.WriteLine($"❌ Error en EditReglaOpcion: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina una regla de opción.
        /// </summary>
        /// <param name="idReglaOpcion">ID de la regla de opción a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, false en caso contrario.</returns>
        public async Task<bool> DeleteReglaOpcionAsync(int idReglaOpcion)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"ReglaOpcion/{idReglaOpcion}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al eliminar regla de opción: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Agrega una nueva regla de opción.
        /// </summary>
        /// <param name="nuevaReglaOpcion">Objeto ReglaOpcionDto con los datos de la nueva regla de opción.</param>
        /// <returns>Objeto ReglaOpcionDto agregado, null en caso de error.</returns>
        public async Task<ReglaOpcionDto> AddReglaOpcion(ReglaOpcionDto nuevaReglaOpcion)
        {
            try
            {
                var json = JsonSerializer.Serialize(nuevaReglaOpcion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("ReglaOpcion", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var addedReglaOpcion = JsonSerializer.Deserialize<ReglaOpcionDto>(responseData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    Console.WriteLine($"✅ Nueva regla de opción agregada correctamente.");

                    return addedReglaOpcion;
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en AddReglaOpcion: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene una regla de opción por el ID de la opción.
        /// </summary>
        /// <param name="idOpcion">ID de la opción.</param>
        /// <returns>Objeto ReglaOpcionDto.</returns>
        public async Task<ReglaOpcionDto> GetReglaOpcionByOpcionIdOpcion(int idOpcion)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ReglaOpcion/Opcion/{idOpcion}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<ReglaOpcionDto>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetReglaOpcionByOpcionId: {ex.Message}");
                return null;
            }
        }
    }
}
