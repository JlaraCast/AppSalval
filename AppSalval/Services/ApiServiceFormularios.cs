using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppSalval.Models_Api;

namespace AppSalval.Services
{
    public class ApiServiceFormularios
    {
        // Cliente HTTP para conectar con la API en la nube (Somee)
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://savalapi.somee.com/api/Formulario"; // 📌 URL de la API en Somee

        public ApiServiceFormularios()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // ✅ Método para obtener la lista de formularios desde la API en Somee
        public async Task<List<FormularioDto>> GetFormularios()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<FormularioDto>>("");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetFormularios: {ex.Message}");
                return null;
            }
        }

        // ✅ Método para obtener un formulario por su ID desde Somee
        public async Task<FormularioDto> GetFormularioById(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<FormularioDto>($"{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetFormularioById: {ex.Message}");
                return null;
            }
        }

        // ✅ Método para editar un formulario existente en Somee
        public async Task<bool> EditFormulario(FormularioDto formulario)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(formulario), Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{formulario.IdFormulario}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Formulario con ID {formulario.IdFormulario} actualizado correctamente.");
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
                Console.WriteLine($"❌ Error en EditFormulario: {ex.Message}");
                return false;
            }
        }
    }
}
