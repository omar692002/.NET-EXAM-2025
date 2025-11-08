using GestionParfumerie.Application.Mapping;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using GestionParfumerie.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace GestionParfumerie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            //  CONFIGURATION DES REPOSITORIES
            // ==========================================
            builder.Services.AddScoped<IProduitRepository, ProduitRepository>();

             builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
             builder.Services.AddScoped<ICommandesRepository, CommandesRepository>();
             builder.Services.AddScoped<IFournisseursRepository, FournisseursRepository>();
             builder.Services.AddScoped<IFamilleOlfactivesRepository, FamilleOlfactivesRepository>();

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

            // ==========================================
            //  CONFIGURATION DU PIPELINE HTTP
            // ==========================================
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // doit être avant UseAuthorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
