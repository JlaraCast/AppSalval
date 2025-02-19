using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    public class ApiServiceOpcionesRespuesta
    {
        private readonly HttpClient _httpClient;

        public ApiServiceOpcionesRespuesta()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://savalapi.somee.com/api/")
            };
        }

        /// <summary>
        /// Obtiene las opciones de respuesta de una pregunta por su ID.
        /// </summary>
        public async Task<List<OpcionRespuestaDto>> GetOpcionesByPregunta(int idPregunta)
        {
            if (idPregunta <= 0)
            {
                Console.WriteLine("⚠️ ID de pregunta inválido.");
                return null;
            }

            try
            {
                var response = await _httpClient.GetAsync($"FormularioPregunta/pregunta/{idPregunta}").ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonSerializer.Deserialize<List<OpcionRespuestaDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"❌ Error de conexión con la API: {httpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetOpcionesByPregunta: {ex.Message}");
                return null;
            }
        }
    }
}
