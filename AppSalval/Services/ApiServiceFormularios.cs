using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.Models_Api;

namespace AppSaval.Services
{
    public class ApiServiceFormularios
    {
        // Cliente HTTP para conectar con la API
        private readonly HttpClient _httpClient;

        // Constructor: Configura la URL base de la API
        public ApiServiceFormularios()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:5001/api/Formulario"); // ⚠️ Cambia la URL si es diferente
        }



        // ✅ Método para obtener la lista de formularios desde la API
        public async Task<List<FormularioDto>> GetFormularios()
        {
            try
            {
                var response = await _httpClient.GetAsync("Formulario");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"📢 Datos de la API: {json}"); // 🔹 Agrega esto para ver los datos en la consola

                    return JsonSerializer.Deserialize<List<FormularioDto>>(json, new JsonSerializerOptions
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
                Console.WriteLine($"❌ Error en GetFormularios: {ex.Message}");
                return null;
            }
        }

    }
}
