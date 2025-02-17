using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    public class ApiServiceReglaOpcion
    {
        // Cliente HTTP para conectar con la API
        private readonly HttpClient _httpClient;

        // Constructor: Configura la URL base de la API
        public ApiServiceReglaOpcion()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://savalapi.somee.com/api/ReglaOpcion"); // ⚠️ Cambia la URL si es diferente
        }

        // ✅ Método para obtener la lista de reglas de opción desde la API
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
        public async Task<List<ReglaOpcionDto>> GetReglaOpcionByIdRegla(int idRegla)
        {
            try
            {
                // Hacer la solicitud a la API
                var response = await _httpClient.GetAsync($"ReglaOpcion/{idRegla}");

                // Verificar si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta como una cadena JSON
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Para depuración

                    // Deserializar el JSON en una lista de ReglaOpcionDto
                    var reglasOpcion = JsonSerializer.Deserialize<List<ReglaOpcionDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignorar mayúsculas/minúsculas en los nombres de propiedades
                    });

                    return reglasOpcion; // Devolver la lista deserializada
                }
                else
                {
                    // Manejar el caso en que la respuesta no sea exitosa
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante el proceso
                Console.WriteLine($"❌ Error en GetReglaOpcionByIdRegla: {ex.Message}");
                return null;
            }
        }

        // ✅ Método para obtener una regla de opción por su ID
        public async Task<List<ReglaOpcionDto>> GetReglaOpcionByOpcionId(int idOpcion)
        {
            try
            {
                // Hacer la solicitud a la API para obtener todas las ReglaOpcion
                var response = await _httpClient.GetAsync("ReglaOpcion");

                // Verificar si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta como una cadena JSON
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Para depuración

                    // Deserializar el JSON en una lista de ReglaOpcionDto
                    var reglasOpcion = JsonSerializer.Deserialize<List<ReglaOpcionDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignorar mayúsculas/minúsculas en los nombres de propiedades
                    });

                    // Filtrar la lista para obtener solo las ReglaOpcion que incluyan el idOpcion
                    var reglasFiltradas = reglasOpcion?.Where(r => r.IdOpcion == idOpcion).ToList();

                    return reglasFiltradas; // Devolver la lista filtrada
                }
                else
                {
                    // Manejar el caso en que la respuesta no sea exitosa
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante el proceso
                Console.WriteLine($"❌ Error en GetReglaOpcionByOpcionId: {ex.Message}");
                return null;
            }
        }


        // ✅ Método para editar una regla de opción existente
        public async Task<bool> EditReglaOpcion(ReglaOpcionDto reglaOpcion)
        {
            try
            {
                // Verificar si la regla de opción existe
                var existingReglaOpcion = await GetReglaOpcionByIdRegla(reglaOpcion.IdRegla);
                if (existingReglaOpcion == null)
                {
                    Console.WriteLine($"⚠️ Regla de opción con ID {reglaOpcion.IdRegla} no encontrada.");
                    return false;
                }

                // Convertir la regla de opción a JSON
                var json = JsonSerializer.Serialize(reglaOpcion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT a la API
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

        // ✅ Método para eliminar una regla de opción
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

        // ✅ Método para agregar una nueva regla de opción
        public async Task<ReglaOpcionDto> AddReglaOpcion(ReglaOpcionDto nuevaReglaOpcion)
        {
            try
            {
                // Convertir la nueva regla de opción a JSON
                var json = JsonSerializer.Serialize(nuevaReglaOpcion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST a la API
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
