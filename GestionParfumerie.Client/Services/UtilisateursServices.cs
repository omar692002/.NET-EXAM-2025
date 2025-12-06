using Blazored.LocalStorage;
using GestionParfumerie.Application.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestionParfumerie.Client.Services
{
    public class UtilisateursService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public UtilisateursService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ==========================================
        // Récupérer tous les utilisateurs
        // ==========================================
        public async Task<List<UtilisateurDTO>> GetAllAsync()
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.GetAsync("api/utilisateurs");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<UtilisateurDTO>>()
                    ?? new List<UtilisateurDTO>();
            }

            return new List<UtilisateurDTO>();
        }

        // ==========================================
        // Récupérer un utilisateur par ID
        // ==========================================
        public async Task<UtilisateurDTO?> GetByIdAsync(string id)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.GetAsync($"api/utilisateurs/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UtilisateurDTO>();
            }

            return null;
        }

        // ==========================================
        // Rechercher des utilisateurs
        // ==========================================
        public async Task<List<UtilisateurDTO>> SearchAsync(string? email = null, string? nom = null)
        {
            await AddAuthorizationHeader();

            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(email))
                queryParams.Add($"email={Uri.EscapeDataString(email)}");
            if (!string.IsNullOrWhiteSpace(nom))
                queryParams.Add($"nom={Uri.EscapeDataString(nom)}");

            var query = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
            var response = await _httpClient.GetAsync($"api/utilisateurs/search{query}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<UtilisateurDTO>>()
                    ?? new List<UtilisateurDTO>();
            }

            return new List<UtilisateurDTO>();
        }

        // ==========================================
        // Récupérer les commandes d'un utilisateur
        // ==========================================
        public async Task<List<CommandeDTO>> GetCommandesAsync(string userId)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.GetAsync($"api/utilisateurs/{userId}/commandes");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CommandeDTO>>()
                    ?? new List<CommandeDTO>();
            }

            return new List<CommandeDTO>();
        }

        // ==========================================
        // Récupérer les détails d'une commande
        // ==========================================
        public async Task<CommandeDTO?> GetCommandeDetailsAsync(string userId, int commandeId)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.GetAsync($"api/utilisateurs/{userId}/commandes/{commandeId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CommandeDTO>();
            }

            return null;
        }

        // ==========================================
        // Récupérer les statistiques d'un utilisateur
        // ==========================================
        public async Task<UtilisateurStatsDTO?> GetStatsAsync(string userId)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.GetAsync($"api/utilisateurs/{userId}/stats");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UtilisateurStatsDTO>();
            }

            return null;
        }
    }
}