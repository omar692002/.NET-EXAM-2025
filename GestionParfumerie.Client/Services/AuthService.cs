using GestionParfumerie.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace GestionParfumerie.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly NavigationManager _navManager;
        private const string TOKEN_KEY = "authToken";

        // Ajoutez NavigationManager dans le constructeur
        public AuthService(HttpClient http, IJSRuntime js, NavigationManager navManager)
        {
            _http = http;
            _js = js;
            _navManager = navManager;
        }

        // ==========================================
        //  REGISTER
        // ==========================================
        public async Task<AuthResult> Register(RegisterDTO model)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Auth/register", model);

                if (response.IsSuccessStatusCode)
                {
                    return new AuthResult { Success = true };
                }

                var error = await response.Content.ReadAsStringAsync();
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = error
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = $"Erreur: {ex.Message}"
                };
            }
        }

        // ==========================================
        //  LOGIN
        // ==========================================
        public async Task<AuthResult> Login(string email, string password)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Auth/login",
                    new { Email = email, Password = password });

                if (!response.IsSuccessStatusCode)
                {
                    return new AuthResult
                    {
                        Success = false,
                        ErrorMessage = "Email ou mot de passe incorrect"
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (string.IsNullOrEmpty(result?.Token))
                {
                    return new AuthResult
                    {
                        Success = false,
                        ErrorMessage = "Token invalide"
                    };
                }

                // Sauvegarder le token
                await _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, result.Token);

                // Configurer le header Authorization
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.Token);

                return new AuthResult { Success = true };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = $"Erreur de connexion: {ex.Message}"
                };
            }
        }

        // ==========================================
        //  LOGOUT
        // ==========================================
        public async Task Logout()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            _http.DefaultRequestHeaders.Authorization = null;

            // Rediriger vers la page de login
            _navManager.NavigateTo("/login");
        }

        // ==========================================
        //  GET TOKEN
        // ==========================================
        public async Task<string?> GetTokenAsync()
        {
            try
            {
                return await _js.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
            }
            catch
            {
                return null;
            }
        }

        // ==========================================
        //  GET CLAIMS PRINCIPAL
        // ==========================================
        public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync()
        {
            var token = await GetTokenAsync();

            if (string.IsNullOrEmpty(token))
                return new ClaimsPrincipal(new ClaimsIdentity());

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                // Vérifier l'expiration
                if (jwt.ValidTo < DateTime.UtcNow)
                {
                    await Logout();
                    return new ClaimsPrincipal(new ClaimsIdentity());
                }

                var claims = jwt.Claims;
                var identity = new ClaimsIdentity(claims, "jwt");

                return new ClaimsPrincipal(identity);
            }
            catch
            {
                await Logout();
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
        }

        // ==========================================
        //  INITIALIZE (Restaurer le token au démarrage)
        // ==========================================
        public async Task InitializeAsync()
        {
            var token = await GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token);
                    if (jwt.ValidTo > DateTime.UtcNow)
                    {
                        _http.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);
                    }
                    else
                    {
                        await Logout();
                    }
                }
                catch
                {
                    await Logout();
                }
            }
        }
    }

    // ==========================================
    //  MODELS
    // ==========================================
    public class LoginResponse
    {
        public string Token { get; set; } = "";
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = "";
    }
}