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

        /// <summary>
        /// Obtiene la lista de formularios desde la API en Somee.
        /// </summary>
        /// <returns>Lista de objetos FormularioDto.</returns>
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

        /// <summary>
        /// Obtiene un formulario por su ID desde Somee.
        /// </summary>
        /// <param name="id">ID del formulario a obtener.</param>
        /// <returns>Objeto FormularioDto si se encuentra, de lo contrario null.</returns>
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

        /// <summary>
        /// Edita un formulario existente en Somee.
        /// </summary>
        /// <param name="formulario">Objeto FormularioDto con los datos actualizados.</param>
        /// <returns>True si la edición fue exitosa, de lo contrario false.</returns>
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

        /// <summary>
        /// Crea un nuevo formulario en Somee.
        /// </summary>
        /// <param name="formulario">Objeto FormularioDto con los datos del nuevo formulario.</param>
        /// <returns>ID del formulario creado si es exitoso, de lo contrario un valor negativo indicando el error.</returns>
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