using System.Net.Http.Json;
using GestionParfumerie.Application.DTOs;

namespace GestionParfumerie.Client.Services
{
    public class CommandesService
    {
        private readonly HttpClient _http;

        public CommandesService(HttpClient http)
        {
            _http = http;
        }

        // Admin : toutes les commandes
        public async Task<List<CommandeDTO>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<CommandeDTO>>("api/commandes/all");
        }

        // Admin : changer statut
        public async Task UpdateStatutAsync(int id, string statut)
        {
            var response = await _http.PutAsJsonAsync($"api/commandes/{id}/status", statut);
            response.EnsureSuccessStatusCode();
        }
    }
}
