using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    public class ApiServiceOpcionRespuesta
    {
        // Cliente HTTP para conectar con la API
        private readonly HttpClient _httpClient;

        // Constructor: Configura la URL base de la API
        public ApiServiceOpcionRespuesta()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://savalapi.somee.com/api/OpcionRespuesta"); // ⚠️ Cambia la URL si es diferente
        }

        // ✅ Método para obtener la lista de opciones de respuesta desde la API
        public async Task<List<OpcionRespuestaDto>> GetOpcionRespuestas()
        {
            try
            {
                var response = await _httpClient.GetAsync("OpcionRespuesta");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<List<OpcionRespuestaDto>>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetOpcionRespuestas: {ex.Message}");
                return null;
            }
        }


        // ✅ Método para obtener una opción de respuesta por su ID
        public async Task<List<OpcionRespuestaDto>> GetOpcionRespuestaById(int id)
        {
            try
            {
                List<OpcionRespuestaDto> opcionesRespuesta = await GetOpcionRespuestas();

                // Filtrar la lista en busca de coincidencias con el id de pregunta
                List<OpcionRespuestaDto> opcionesFiltradas = opcionesRespuesta?.Where(o => o.IdPregunta == id).ToList();

                return opcionesFiltradas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetOpcionRespuestaById: {ex.Message}");
                return null;
            }
        }
        

        // ✅ Método para editar una opción de respuesta existente
        public async Task<bool> EditOpcionRespuesta(OpcionRespuestaDto opcionRespuesta)
        {
            try
            {
                // Verificar si la opción de respuesta existe
                var existingOpcionRespuesta = await GetOpcionRespuestaById(opcionRespuesta.IdOpcion);
                if (existingOpcionRespuesta == null)
                {
                    Console.WriteLine($"⚠️ Opción de respuesta con ID {opcionRespuesta.IdOpcion} no encontrada.");
                    return false;
                }

                // Convertir la opción de respuesta a JSON
                var json = JsonSerializer.Serialize(opcionRespuesta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT a la API
                var response = await _httpClient.PutAsync($"OpcionRespuesta/{opcionRespuesta.IdOpcion}", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Opción de respuesta con ID {opcionRespuesta.IdOpcion} actualizada correctamente.");
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
                Console.WriteLine($"❌ Error en EditOpcionRespuesta: {ex.Message}");
                return false;
            }
        }

        // ✅ Método para eliminar una opción de respuesta
        public async Task<bool> DeleteOpcionRespuestaAsync(int idOpcionRespuesta)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"OpcionRespuesta/{idOpcionRespuesta}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al eliminar opción de respuesta: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }
        // ✅ Método para agregar una nueva opción de respuesta
        public async Task<OpcionRespuestaDto> AddOpcionRespuesta(OpcionRespuestaDto nuevaOpcionRespuesta)
        {
            try
            {
                // Convertir la nueva opción de respuesta a JSON
                var json = JsonSerializer.Serialize(nuevaOpcionRespuesta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine($"entro");
                // Enviar la solicitud POST a la API
                var response = await _httpClient.PostAsync("OpcionRespuesta", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var addedOpcionRespuesta = JsonSerializer.Deserialize<OpcionRespuestaDto>(responseData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    Console.WriteLine($"✅ Nueva opción de respuesta agregada correctamente.");

                    // Agregar una nueva línea para modificar la opción a gusto
                    Console.WriteLine($"📝 Modifique la nueva opción de respuesta: {addedOpcionRespuesta.NombreOpcion}");

                    return addedOpcionRespuesta;
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en AddOpcionRespuesta: {ex.Message}");
                return null;
            }
        }
        public async Task<List<OpcionRespuestaDto>> GetOpcionRespuestasByPreguntaId(int idPregunta)
        {
            try
            {
                var response = await _httpClient.GetAsync($"OpcionRespuesta/pregunta/{idPregunta}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<List<OpcionRespuestaDto>>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetOpcionRespuestasByPreguntaId: {ex.Message}");
                return null;
            }
        }
    }
}
