using GestionParfumerie.Application.Mapping;
using GestionParfumerie.Application.Services;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using GestionParfumerie.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using GestionParfumerie.Application.Interfaces;

namespace GestionParfumerie
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // ==========================================
            //  CONFIGURATION DE CORS
            // ==========================================

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient", policy =>
                {
                    policy.WithOrigins("http://localhost:5266")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // ==========================================
            //  CONFIGURATION DE LA BASE DE DONNÉES
            // ==========================================
            var cnx = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(cnx));

            // ==========================================
            //  CONFIGURATION DE IDENTITY (Utilisateurs)
            // ==========================================
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ==========================================
            //  CONFIGURATION DU JWT
            // ==========================================
            var jwtSecret = builder.Configuration["JWT:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // ==========================================
            //  CONFIGURATION D’AUTOMAPPER
            // ==========================================
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // ==========================================
            //  CONFIGURATION DES REPOSITORIES & SERVICES
            // ==========================================
            builder.Services.AddScoped<ICommandesRepository, CommandesRepository>();
            builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            builder.Services.AddScoped<ICommandesRepository, CommandesRepository>();
            builder.Services.AddScoped<IFournisseursRepository, FournisseursRepository>();
            builder.Services.AddScoped<IFamilleOlfactivesRepository, FamilleOlfactivesRepository>();
            builder.Services.AddScoped<ILigneCommandesRepository, LigneCommandesRepository>();
            builder.Services.AddScoped<ICommandeService, CommandeService>();

            // ==========================================
            //  CONFIGURATION SWAGGER (avec JWT)
            // ==========================================
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Entrez votre token JWT (ex: Bearer {votre_token})"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            var app = builder.Build();

            // ==========================================
            //  SEED DES RÔLES ET ADMIN/CLIENT
            // ==========================================
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Créer les rôles si nécessaire
                string[] roles = { "Admin", "Client" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Créer l'utilisateur admin s'il n'existe pas
                var adminEmail = "admin@admin.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        Nom = "Super",
                        Prenom = "Admin"
                    };

                    await userManager.CreateAsync(adminUser, "Admin123!"); // mot de passe sécurisé
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // ==========================================
            //  PIPELINE HTTP
            // ==========================================
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowBlazorClient");

            app.UseHttpsRedirection();
            app.UseAuthentication(); // doit être avant UseAuthorization
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
