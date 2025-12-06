using System.Net.Http.Json;
using GestionParfumerie.Application.DTOs;

namespace GestionParfumerie.Client.Services
{
    public class BrandsService
    {
        private readonly HttpClient _http;

        public BrandsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<BrandsDTO>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<BrandsDTO>>("api/brands");
        }

        public async Task<BrandsDTO> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<BrandsDTO>($"api/brands/{id}");
        }

        public async Task<BrandsDTO> CreateAsync(BrandsDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/brands", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BrandsDTO>();
        }

        public async Task UpdateAsync(int id, BrandsDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/brands/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/brands/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
