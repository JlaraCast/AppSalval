using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    public class ApiServicePregunta
    {
        // Cliente HTTP para conectar con la API
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor: Configura la URL base de la API
        /// </summary>
        public ApiServicePregunta()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://savalapi.somee.com/api/Pregunta"); // ⚠️ Cambia la URL si es diferente
        }

        /// <summary>
        /// Obtiene la lista de preguntas desde la API
        /// </summary>
        /// <returns>Lista de preguntas</returns>
        public async Task<List<PreguntaDto>> GetPreguntas()
        {
            try
            {
                var response = await _httpClient.GetAsync("Pregunta");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<List<PreguntaDto>>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetPreguntas: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene una pregunta por su ID
        /// </summary>
        /// <param name="id">ID de la pregunta</param>
        /// <returns>Pregunta correspondiente al ID</returns>
        public async Task<PreguntaDto> GetPreguntaById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Pregunta/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<PreguntaDto>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetPreguntaById: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Edita una pregunta existente
        /// </summary>
        /// <param name="pregunta">Objeto PreguntaDto con los datos a editar</param>
        /// <returns>True si la edición fue exitosa, false en caso contrario</returns>
        public async Task<bool> EditPregunta(PreguntaDto pregunta)
        {
            try
            {
                // Verificar si la pregunta existe
                var existingPregunta = await GetPreguntaById(pregunta.IdPregunta);
                if (existingPregunta == null)
                {
                    Console.WriteLine($"⚠️ Pregunta con ID {pregunta.IdPregunta} no encontrada.");
                    return false;
                }

                // Convertir la pregunta a JSON
                var json = JsonSerializer.Serialize(pregunta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT a la API
                var response = await _httpClient.PutAsync($"Pregunta/{pregunta.IdPregunta}", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Pregunta con ID {pregunta.IdPregunta} actualizada correctamente.");
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
                Console.WriteLine($"❌ Error en EditPregunta: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina una pregunta por su ID
        /// </summary>
        /// <param name="idPregunta">ID de la pregunta a eliminar</param>
        /// <returns>True si la eliminación fue exitosa, false en caso contrario</returns>
        public async Task<bool> DeletePreguntaAsync(int idPregunta)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Pregunta/{idPregunta}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al eliminar pregunta: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Agrega una nueva pregunta
        /// </summary>
        /// <param name="nuevaPregunta">Objeto PreguntaDto con los datos de la nueva pregunta</param>
        /// <returns>True si la adición fue exitosa, false en caso contrario</returns>
        public async Task<bool> AddPregunta(PreguntaDto nuevaPregunta)
        {
            try
            {
                // Convertir la nueva pregunta a JSON
                var json = JsonSerializer.Serialize(nuevaPregunta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST a la API
                var response = await _httpClient.PostAsync("Pregunta", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Pregunta agregada correctamente.");
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
                Console.WriteLine($"❌ Error en AddPregunta: {ex.Message}");
                return false;
            }
        }
    }
}
