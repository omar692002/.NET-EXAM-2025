namespace GestionParfumerie.Client.Helpers
{
    
        public static class ImageHelper
        {
            private static string? _apiBaseUrl;

            public static void Initialize(string apiBaseUrl)
            {
                _apiBaseUrl = apiBaseUrl;
            }

            public static string GetImageUrl(string? photoPath)
            {
                if (string.IsNullOrEmpty(photoPath)) return "";
                if (photoPath.StartsWith("http")) return photoPath;
                return $"{_apiBaseUrl}{photoPath}";
            }
        }
    
}
