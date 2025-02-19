using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Services
{
    class ApiServiceFormularioPregunta
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://savalapi.somee.com/api/";

        public ApiServiceFormularioPregunta()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        public async Task<bool> AddFormularioPreguntaAsync(FormularioPreguntaDtoS newPregunta)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("FormularioPregunta", newPregunta);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al agregar pregunta: {errorMessage}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// Obtiene las preguntas de los formularios pero aun no sirve porque falta en la api 
        /// </summary>
        /// <returns></returns>
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

        
    }
}
