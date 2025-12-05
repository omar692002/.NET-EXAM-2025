using System.Net.Http.Json;
using GestionParfumerie.Application.DTOs;

namespace GestionParfumerie.Client.Services
{
    public class FournisseursService
    {
        private readonly HttpClient _http;

        public FournisseursService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<FournisseurDTO>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<FournisseurDTO>>("api/fournisseurs");
        }

        public async Task<FournisseurDTO> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<FournisseurDTO>($"api/fournisseurs/{id}");
        }

        public async Task<FournisseurDTO> CreateAsync(FournisseurDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/fournisseurs", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FournisseurDTO>();
        }

        public async Task UpdateAsync(int id, FournisseurDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/fournisseurs/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/fournisseurs/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
