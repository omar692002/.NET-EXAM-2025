using Blazored.LocalStorage;
using GestionParfumerie.Application.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestionParfumerie.Client.Services
{
    public class AdminDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AdminDashboardService(HttpClient httpClient, ILocalStorageService localStorage)
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

        public async Task<AdminDashboardStatsDTO?> GetDashboardStatsAsync()
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.GetAsync("api/admindashboard/stats");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AdminDashboardStatsDTO>();
            }

            return null;
        }
    }
}