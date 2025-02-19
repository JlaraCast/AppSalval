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
        public async Task<FormularioDto?> GetFormularioById(int id)
        {
            try
            {
                var url = $"{BaseUrl}/{id}"; // Construir la URL completa con el ID
                var formulario = await _httpClient.GetFromJsonAsync<FormularioDto>(url);

                if (formulario == null)
                {
                    Console.WriteLine($"⚠️ Formulario con ID {id} no encontrado.");
                }

                return formulario;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"❌ Error HTTP en GetFormularioById: {httpEx.Message}");
                return null;
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

                var url = $"{BaseUrl}/{formulario.IdFormulario}"; // Construir la URL completa con el ID
                var response = await _httpClient.PutAsync(url, jsonContent);

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
        // ✅ Método para crear un nuevo formulario en Somee
        public async Task<int> CreateFormulario(FormularioDto formulario)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(formulario), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(BaseUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var createdFormulario = JsonSerializer.Deserialize<FormularioDto>(jsonResponse);

                    if (createdFormulario != null && createdFormulario.IdFormulario > 0) // 🔹 Validamos que el ID sea válido
                    {
                        Console.WriteLine($"✅ Formulario creado con ID: {createdFormulario.IdFormulario}");
                        return createdFormulario.IdFormulario;
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Error: El formulario se creó, pero el ID recibido es inválido.");
                        return 0;  // ⬅️ Indicar que algo salió mal
                    }
                }
                else
                {
                    Console.WriteLine($"⚠️ Error en API: {response.StatusCode}");
                    return -1;  // ⬅️ Indicar un fallo en la API
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en CreateFormulario: {ex.Message}");
                return -2;  // ⬅️ Indicar error crítico
            }
        }







    }
}