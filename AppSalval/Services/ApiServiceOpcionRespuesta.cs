using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    /// <summary>
    /// Servicio para interactuar con la API de opciones de respuesta.
    /// </summary>
    public class ApiServiceOpcionRespuesta
    {
        // Cliente HTTP para conectar con la API
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor: Configura la URL base de la API.
        /// </summary>
        public ApiServiceOpcionRespuesta()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://savalapi.somee.com/api/OpcionRespuesta"); // ⚠️ Cambia la URL si es diferente
        }

        /// <summary>
        /// Obtiene la lista de opciones de respuesta desde la API.
        /// </summary>
        /// <returns>Lista de opciones de respuesta.</returns>
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

        /// <summary>
        /// Obtiene varias opciones de respuesta por su ID de pregunta.
        /// </summary>
        /// <param name="id">ID de la pregunta.</param>
        /// <returns>Lista de opciones de respuesta filtradas por ID de pregunta.</returns>
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

        /// <summary>
        /// Obtiene una opción de respuesta por su ID de opción.
        /// </summary>
        /// <param name="idOpcion">ID de la opción de respuesta.</param>
        /// <returns>Opción de respuesta correspondiente al ID proporcionado.</returns>
        public async Task<OpcionRespuestaDto> GetOpcionRespuestaByIdOpcion(int idOpcion)
        {
            try
            {
                var response = await _httpClient.GetAsync($"OpcionRespuesta/{idOpcion}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<OpcionRespuestaDto>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetOpcionRespuestaByIdOpcion: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Edita una opción de respuesta existente.
        /// </summary>
        /// <param name="opcionRespuesta">Objeto de opción de respuesta a editar.</param>
        /// <returns>Verdadero si la edición fue exitosa, falso en caso contrario.</returns>
        public async Task<bool> EditOpcionRespuesta(OpcionRespuestaDto opcionRespuesta)
        {
            try
            {
                // Verificar si la opción de respuesta existe
                var existingOpcionRespuesta = await GetOpcionRespuestaByIdOpcion(opcionRespuesta.IdOpcion);
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

        /// <summary>
        /// Elimina una opción de respuesta.
        /// </summary>
        /// <param name="idOpcionRespuesta">ID de la opción de respuesta a eliminar.</param>
        /// <returns>Verdadero si la eliminación fue exitosa, falso en caso contrario.</returns>
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

        /// <summary>
        /// Agrega una nueva opción de respuesta.
        /// </summary>
        /// <param name="nuevaOpcionRespuesta">Objeto de la nueva opción de respuesta a agregar.</param>
        /// <returns>Objeto de la opción de respuesta agregada.</returns>
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

        /// <summary>
        /// Obtiene las opciones de respuesta por ID de pregunta.
        /// </summary>
        /// <param name="idPregunta">ID de la pregunta.</param>
        /// <returns>Lista de opciones de respuesta correspondientes al ID de pregunta.</returns>
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

        /// <summary>
        /// Obtiene las opciones de respuesta válidas por ID de pregunta.
        /// </summary>
        /// <param name="idPregunta">ID de la pregunta.</param>
        /// <returns>Lista de opciones de respuesta válidas correspondientes al ID de pregunta.</returns>
        public async Task<List<OpcionRespuestaDto>> GetValidOpcionRespuestasByPreguntaId(int idPregunta)
        {
            try
            {
                Debug.WriteLine($"🔍 Buscando opciones para la pregunta ID: {idPregunta}");

                // Obtener todas las opciones desde la API
                var response = await _httpClient.GetAsync($"OpcionRespuesta");

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"⚠️ Error en API al obtener opciones: {response.StatusCode}");
                    return new List<OpcionRespuestaDto>();
                }

                string json = await response.Content.ReadAsStringAsync();
                var todasLasOpciones = JsonSerializer.Deserialize<List<OpcionRespuestaDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (todasLasOpciones == null || todasLasOpciones.Count == 0)
                {
                    Debug.WriteLine($"⚠️ No se encontraron opciones en la API.");
                    return new List<OpcionRespuestaDto>();
                }

                // Filtrar solo las opciones que correspondan a la pregunta con idPregunta
                var opcionesValidas = todasLasOpciones
                    .Where(o => o.IdPregunta == idPregunta && !string.IsNullOrWhiteSpace(o.NombreOpcion))
                    .ToList();

                if (opcionesValidas.Count == 0)
                {
                    Debug.WriteLine($"⚠️ No se encontraron opciones válidas para la pregunta {idPregunta}");
                }
                else
                {
                    Debug.WriteLine($"✅ Opciones válidas encontradas para la pregunta {idPregunta}: {opcionesValidas.Count}");
                    foreach (var opcion in opcionesValidas)
                    {
                        Debug.WriteLine($"   - Opción: {opcion.NombreOpcion}");
                    }
                }

                return opcionesValidas;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error en GetValidOpcionRespuestasByPreguntaId: {ex.Message}");
                return new List<OpcionRespuestaDto>();
            }
        }
    }
}
