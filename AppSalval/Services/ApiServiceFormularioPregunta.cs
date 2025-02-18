using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.DTOS_API; // Asegúrate de que el DTO está en este namespace

namespace AppSalval.Services
{
    public class ApiServiceFormularioPregunta
    {
        private readonly HttpClient _httpClient;

        public ApiServiceFormularioPregunta()
        {
            // 🔹 Configuración correcta de la URL base
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://savalapi.somee.com/api/")
            };
        }

        /// <summary>
        /// Obtiene las preguntas de un formulario según su ID.
        /// </summary>
        /// <param name="idFormulario">ID del formulario</param>
        /// <returns>Lista de preguntas asociadas al formulario</returns>
        public async Task<List<FormularioPreguntaDto>> GetPreguntasByFormulario(int idFormulario)
        {
            if (idFormulario <= 0)
            {
                Console.WriteLine("⚠️ ID de formulario inválido.");
                return null;
            }

            try
            {
                // 🔹 URL corregida para coincidir con la API de Swagger
                var response = await _httpClient.GetAsync($"FormularioPregunta/formulario/{idFormulario}").ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonSerializer.Deserialize<List<FormularioPreguntaDto>>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetPreguntasByFormulario: {ex.Message}");
                return null;
            }
        }
    }
}
