using System.Net.Http.Json;
using GestionParfumerie.Application.DTOs;
namespace GestionParfumerie.Client.Services
{
    public class CategoriesService
    {
        private readonly HttpClient _http;

        public CategoriesService(HttpClient http)
        {
            _http = http;
        }

        // GET /api/categories
        public async Task<List<CategorieDTO>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<CategorieDTO>>("api/categories");
        }

        // POST /api/categories
        public async Task<CategorieDTO> CreateAsync(CategorieDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/categories", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CategorieDTO>();
        }

        // PUT /api/categories/{id}
        public async Task UpdateAsync(int id, CategorieDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/categories/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        // DELETE /api/categories/{id}
        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/categories/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
