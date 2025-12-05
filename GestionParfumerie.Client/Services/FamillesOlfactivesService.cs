using GestionParfumerie.Application.DTOs;
using System.Net.Http.Json;

namespace GestionParfumerie.Client.Services
{
    public class FamillesOlfactivesService
    {
        private readonly HttpClient _http;

        public FamillesOlfactivesService(HttpClient http)
        {
            _http = http;
        }

        // GET /api/familleolfactives
        public async Task<List<FamilleOlfactiveDTO>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<FamilleOlfactiveDTO>>("api/familleolfactives");
        }

        // POST /api/familleolfactives
        public async Task<FamilleOlfactiveDTO> CreateAsync(FamilleOlfactiveDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/familleolfactives", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FamilleOlfactiveDTO>();
        }

        // PUT /api/familleolfactives/{id}
        public async Task UpdateAsync(int id, FamilleOlfactiveDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/familleolfactives/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        // DELETE /api/familleolfactives/{id}
        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/familleolfactives/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
