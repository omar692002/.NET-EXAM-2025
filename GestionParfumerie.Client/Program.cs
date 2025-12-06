using GestionParfumerie.Client.Helpers;
using GestionParfumerie.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

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

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<JwtAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<JwtAuthenticationStateProvider>());

            builder.Services.AddScoped<CategoriesService>();
            builder.Services.AddScoped<FamillesOlfactivesService>();
            builder.Services.AddScoped<ProduitsService>();
            builder.Services.AddScoped<BrandsService>();
            builder.Services.AddScoped<CommandesService>();
            builder.Services.AddScoped<ImageUploadService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<UtilisateursService>();
            builder.Services.AddScoped<AdminDashboardService>();

            ImageHelper.Initialize("http://localhost:5155/");



            var host = builder.Build();

            var authService = host.Services.GetRequiredService<AuthService>();
            await authService.InitializeAsync();

            await host.RunAsync();
        }
    }
}