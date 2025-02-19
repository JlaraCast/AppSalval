using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API;
using System.Diagnostics;

namespace AppSalval.Services
{
    public class ApiServiceFormularioPregunta
    {
        private readonly HttpClient _httpClient;

        public ApiServiceFormularioPregunta()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://savalapi.somee.com/api/")
            };
        }

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
        /// Obtiene las preguntas de un formulario según su ID.
        /// </summary>
        public async Task<List<FormularioPreguntaDto>> GetPreguntasByFormulario(int idFormulario)
        {
            if (idFormulario <= 0)
            {
                Debug.WriteLine("⚠️ ID de formulario inválido.");
                return null;
            }

            try
            {
                return await _httpClient.GetFromJsonAsync<List<FormularioPreguntaDtoS>>("FormularioPregunta");
            }
            catch
            {
                return new List<FormularioPreguntaDtoS>();
            }
        }

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
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }
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
        /// Obtiene las opciones de respuesta de una pregunta según su ID.
        /// </summary>
        public async Task<List<OpcionRespuestaDto>> GetOpcionesByPregunta(int idPregunta)
        {
            if (idPregunta <= 0)
            {
                Debug.WriteLine("⚠️ ID de pregunta inválido.");
                return null;
            }

            try
            {
                var response = await _httpClient.GetAsync($"FormularioPregunta/pregunta/{idPregunta}").ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonSerializer.Deserialize<List<OpcionRespuestaDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error en GetOpcionesByPregunta: {ex.Message}");
                return null;
            }
        }
    }
}
