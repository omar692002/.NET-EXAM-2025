using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GestionParfumerie.Client.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;

        public JwtAuthenticationStateProvider(AuthService authService)
        {
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await _authService.GetClaimsPrincipalAsync();
            return new AuthenticationState(user);
        }

        // Notifier après le login
        public void NotifyUserAuthentication()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        // Notifier après le logout
        public void NotifyUserLogout()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}