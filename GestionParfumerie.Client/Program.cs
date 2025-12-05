using GestionParfumerie.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace GestionParfumerie.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5155/")
            });

            // Services d'authentification
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<JwtAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<JwtAuthenticationStateProvider>());

            var host = builder.Build();

            //  UTILISE LA MÉTHODE InitializeAsync AU LIEU
            var authService = host.Services.GetRequiredService<AuthService>();
            await authService.InitializeAsync();

            await host.RunAsync();
        }
    }
}