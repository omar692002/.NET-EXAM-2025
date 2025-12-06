using GestionParfumerie.Application.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
namespace GestionParfumerie.Client.Services
{
    public class ProduitsService
    {
        private readonly HttpClient _http;

        public ProduitsService(HttpClient http)
        {
            _http = http;
        }

        // GET /api/produits
        public async Task<List<ProduitDTO>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<ProduitDTO>>("api/produits");
        }

        // GET /api/produits/{id}
        public async Task<ProduitDTO> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ProduitDTO>($"api/produits/{id}");
        }

        // POST /api/produits
        public async Task<ProduitDTO> CreateAsync(ProduitCreateUpdateDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/produits", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProduitDTO>();
        }

        // PUT /api/produits/{id}
        public async Task UpdateAsync(int id, ProduitCreateUpdateDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/produits/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        // DELETE /api/produits/{id}
        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/produits/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    throw new Exception(errorResponse?.Message ?? "Erreur lors de la suppression du produit.");
                }
                catch (JsonException)
                {
                    throw new Exception($"Erreur lors de la suppression du produit. Status: {response.StatusCode}");
                }
            }
        }

        public async Task<List<CategorieDTO>> GetAllCategoriesAsync()
        {
            return await _http.GetFromJsonAsync<List<CategorieDTO>>("api/categories") ?? new List<CategorieDTO>();
        }

       
        public async Task<List<BrandsDTO>> GetAllFournisseursAsync()
        {
            return await _http.GetFromJsonAsync<List<BrandsDTO>>("api/brands") ?? new List<BrandsDTO>();
        }
        public async Task<List<FamilleOlfactiveDTO>> GetAllFamillesAsync()
        {
            return await _http.GetFromJsonAsync<List<FamilleOlfactiveDTO>>("api/famillesolfactives") ?? new List<FamilleOlfactiveDTO>();
        }


    }
}
