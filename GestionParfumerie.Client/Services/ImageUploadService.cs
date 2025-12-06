using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestionParfumerie.Client.Services
{
    public class ImageUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public ImageUploadService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = "https://localhost:5155";
        }

        public async Task<string?> UploadImageAsync(IBrowserFile file)
        {
            try
            {
                if (file.Size > MaxFileSize)
                {
                    throw new Exception($"Le fichier est trop volumineux. Taille max : {MaxFileSize / 1024 / 1024} MB");
                }
                using var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "file", file.Name);

                var response = await _httpClient.PostAsync("api/images/upload", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ImageUploadResponse>();
                    // ✅ Retourner SEULEMENT le chemin relatif (comme dans votre BDD)
                    return result?.Path;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'upload : {ex.Message}");
                throw;
            }
        }
        public async Task DeleteImageAsync(string? photoPath)
        {
            if (string.IsNullOrEmpty(photoPath)) return;

            try
            {
                // Extraire le chemin relatif si c'est une URL complète
                var relativePath = photoPath.Replace(_apiBaseUrl, "");
                await _httpClient.DeleteAsync($"api/images/delete?photoPath={Uri.EscapeDataString(relativePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression : {ex.Message}");
            }
        }

        public string GetFullImageUrl(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return "";
            if (relativePath.StartsWith("http")) return relativePath;
            return $"{_apiBaseUrl}{relativePath}";
        }

        private class ImageUploadResponse
        {
            public string Path { get; set; } = string.Empty;
        }
    }
}