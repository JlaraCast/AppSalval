using System;
using System.Collections.Generic;
using System.Net.Http;
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

        /// <summary>
        /// Obtiene las preguntas de un formulario según su ID.
        /// </summary>
        public async Task<List<FormularioPreguntaIdDto>> GetPreguntasByFormulario(int idFormulario)
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
                return JsonSerializer.Deserialize<List<FormularioPreguntaIdDto>>(json, new JsonSerializerOptions
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
