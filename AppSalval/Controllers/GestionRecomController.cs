using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Controllers
{
    public class GestionRecomController
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://savalapi.somee.com/api/Recomendacion";

        public GestionRecomController()
        {
            _httpClient = new HttpClient();
        }

        // Obtener lista de recomendaciones
        public async Task<List<Recomendacion>> GetRecomendacionesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Recomendacion>>($"{BaseUrl}");
        }

        // Agregar una recomendación
        public async Task<bool> AddRecomendacionAsync(Recomendacion nuevaRecomendacion)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", nuevaRecomendacion);
            return response.IsSuccessStatusCode;
        }

        // Eliminar una recomendación
        public async Task<bool> DeleteRecomendacionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}{id}");
            return response.IsSuccessStatusCode;
        }

        // Método para obtener y eliminar una recomendación al mismo tiempo
        public async Task<(bool isDeleted, List<Recomendacion> recomendaciones)> DeleteAndRefreshRecomendacionesAsync(int id)
        {
            bool isDeleted = await DeleteRecomendacionAsync(id);
            List<Recomendacion> recomendaciones = isDeleted ? await GetRecomendacionesAsync() : null;
            return (isDeleted, recomendaciones);
        }
    }

}
