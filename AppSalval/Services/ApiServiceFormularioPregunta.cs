using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.Models_Api;

namespace AppSalval.Services
{
    public class ApiServiceFormularioPregunta
    {
        private readonly HttpClient _httpClient;

        public ApiServiceFormularioPregunta()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://savalapi.somee.com/api/FormularioPregunta"); // ⚠️ Asegúrate de que esta URL es correcta
        }

        public async Task<List<FormularioPreguntaDto>> GetPreguntasByFormulario(int idFormulario)
        {
            try
            {
                var response = await _httpClient.GetAsync($"formulario/{idFormulario}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<FormularioPreguntaDto>>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetPreguntasByFormulario: {ex.Message}");
                return null;
            }
        }
    }
}
