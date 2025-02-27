using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    /// <summary>
    /// Servicio para manejar las operaciones relacionadas con FormularioPregunta en la API.
    /// </summary>
    class ApiServiceFormularioPregunta
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://savalapi.somee.com/api/";

        /// <summary>
        /// Constructor que inicializa el HttpClient con la URL base de la API.
        /// </summary>
        public ApiServiceFormularioPregunta()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        /// <summary>
        /// Agrega una nueva relación Formulario-Pregunta a la API.
        /// </summary>
        /// <param name="formularioPregunta">Objeto FormularioPreguntaDtoS que contiene los datos de la relación.</param>
        /// <returns>Devuelve true si la operación fue exitosa, de lo contrario false.</returns>
        public async Task<bool> AddFormularioPreguntaAsync(FormularioPreguntaDtoS formularioPregunta)
        {
            try
            {
                string json = JsonSerializer.Serialize(formularioPregunta);
                Console.WriteLine($"📤 Enviando a API: {json}"); // Verificar que los datos sean correctos

                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}FormularioPregunta", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("✅ Relación Formulario-Pregunta guardada correctamente.");
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
                Console.WriteLine($"❌ Error en AddFormularioPreguntaAsync: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtiene todas las relaciones Formulario-Pregunta desde la API.
        /// </summary>
        /// <returns>Lista de objetos FormularioPreguntaDtoS.</returns>
        public async Task<List<FormularioPreguntaDtoS>> GetFormularioPreguntasAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<FormularioPreguntaDtoS>>("FormularioPregunta");
            }
            catch
            {
                return new List<FormularioPreguntaDtoS>();
            }
        }

        /// <summary>
        /// Elimina una relación Formulario-Pregunta de la API.
        /// </summary>
        /// <param name="FormulariopreguntaId">ID de la relación Formulario-Pregunta a eliminar.</param>
        /// <returns>Devuelve true si la operación fue exitosa, de lo contrario false.</returns>
        public async Task<bool> DeleteFormularioPreguntaAsync(int FormulariopreguntaId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"FormularioPregunta/{FormulariopreguntaId}");

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
        /// Obtiene las relaciones Formulario-Pregunta por ID de formulario desde la API.
        /// </summary>
        /// <param name="formularioId">ID del formulario.</param>
        /// <returns>Lista de objetos FormularioPreguntaDtoS.</returns>
        public async Task<List<FormularioPreguntaDtoS>> GetFormularioPreguntasByFormularioIdAsync(int formularioId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<FormularioPreguntaDtoS>>($"FormularioPregunta/Formulario/{formularioId}");
            }
            catch
            {
                return new List<FormularioPreguntaDtoS>();
            }
        }

        /// <summary>
        /// Obtiene las relaciones Formulario-Pregunta por ID de pregunta desde la API.
        /// </summary>
        /// <param name="preguntaId">ID de la pregunta.</param>
        /// <returns>Lista de objetos FormularioPreguntaDtoS.</returns>
        public async Task<List<FormularioPreguntaDtoS>> GetFormularioPreguntasByPreguntaIdAsync(int preguntaId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<FormularioPreguntaDtoS>>($"FormularioPregunta/Pregunta/{preguntaId}");
            }
            catch
            {
                return new List<FormularioPreguntaDtoS>();
            }
        }

        /// <summary>
        /// Obtiene las preguntas asociadas a un formulario específico desde la API.
        /// </summary>
        /// <param name="idFormulario">ID del formulario.</param>
        /// <returns>Lista de objetos FormularioPreguntaDto.</returns>
        public async Task<List<FormularioPreguntaDto>> GetPreguntasByFormulario(int idFormulario)
        {
            if (idFormulario <= 0)
            {
                Debug.WriteLine("⚠️ ID de formulario inválido.");
                return null;
            }

            try
            {
                var response = await _httpClient.GetAsync($"FormularioPregunta/formulario/{idFormulario}").ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonSerializer.Deserialize<List<FormularioPreguntaDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error en GetPreguntasByFormulario: {ex.Message}");
                return null;
            }
        }
    }
}
