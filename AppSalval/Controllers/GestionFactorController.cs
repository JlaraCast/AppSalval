using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.Controllers
{
    public class GestionFactorController
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://savalapi.somee.com/api/FactorRiesgo";

        public GestionFactorController()
        {
            _httpClient = new HttpClient();
        }

        // Obtener lista de factores de riesgo
        public async Task<List<FactorRiesgo>> GetFactoresAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<FactorRiesgo>>(BaseUrl);
        }

        // Agregar un factor de riesgo
        public async Task<bool> AddFactorAsync(FactorRiesgo nuevoFactor)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, nuevoFactor);
            return response.IsSuccessStatusCode;
        }

        // Actualizar un factor de riesgo
        public async Task<bool> UpdateFactorAsync(FactorRiesgo factor)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}{factor.IdFactor}", factor);
            return response.IsSuccessStatusCode;
        }

        // Eliminar un factor de riesgo (soft delete)
        public async Task<bool> DeleteFactorAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}{id}");
            return response.IsSuccessStatusCode;
        }

        // Obtener y eliminar un factor de riesgo al mismo tiempo
        public async Task<(bool isDeleted, List<FactorRiesgo> factores)> DeleteAndRefreshFactoresAsync(int id)
        {
            bool isDeleted = await DeleteFactorAsync(id);
            List<FactorRiesgo> factores = isDeleted ? await GetFactoresAsync() : null;
            return (isDeleted, factores);
        }
    }
}
