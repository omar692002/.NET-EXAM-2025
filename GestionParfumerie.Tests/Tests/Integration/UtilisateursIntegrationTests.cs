using AutoMapper;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Application.Services;
using GestionParfumerie.Controllers;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GestionParfumerie.Tests.Integration
{
    public class UtilisateursIntegrationTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Mock<ICommandeService> _mockCommandeService;
        private readonly IMapper _mapper;
        private readonly UtilisateursController _controller;

        public UtilisateursIntegrationTests()
        {
            // --- CONFIG DB IN-MEMORY ---
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            // --- CONFIG IDENTITY RÉEL ---
            var userStore = new UserStore<ApplicationUser>(_context);
            _userManager = new UserManager<ApplicationUser>(
                userStore,
                null,
                new PasswordHasher<ApplicationUser>(),
                null, null, null, null, null, null
            );

            var roleStore = new RoleStore<IdentityRole>(_context);
            _roleManager = new RoleManager<IdentityRole>(
                roleStore, null, null, null, null
            );

            // --- CONFIG AUTOMAPPER ---
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Commande, CommandeDTO>().ReverseMap();
                cfg.CreateMap<ApplicationUser, UtilisateurDTO>().ReverseMap();
                cfg.CreateMap<LigneCommande, LigneCommandeDTO>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            // --- MOCK SERVICE ---
            _mockCommandeService = new Mock<ICommandeService>();

            // --- CONTROLLER ---
            _controller = new UtilisateursController(
                _userManager,
                _mockCommandeService.Object,
                _mapper
            );

            // Seed des données de test
            SeedTestData().Wait();
        }

        private async Task SeedTestData()
        {
            // Créer des rôles
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("User"));

            // Créer des utilisateurs
            var user1 = new ApplicationUser
            {
                Id = "user1",
                UserName = "user1@test.com",
                Email = "user1@test.com",
                Nom = "Dupont",
                Prenom = "Jean",
                DateNaissance = new DateTime(1990, 1, 1),
                Adresse = "123 Rue Test",
                PhoneNumber = "0123456789"
            };

            var user2 = new ApplicationUser
            {
                Id = "user2",
                UserName = "user2@test.com",
                Email = "user2@test.com",
                Nom = "Martin",
                Prenom = "Marie",
                DateNaissance = new DateTime(1995, 5, 15),
                Adresse = "456 Avenue Test",
                PhoneNumber = "0987654321"
            };

            await _userManager.CreateAsync(user1, "Password123!");
            await _userManager.CreateAsync(user2, "Password123!");

            await _userManager.AddToRoleAsync(user1, "User");
            await _userManager.AddToRoleAsync(user2, "Admin");
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // ========================================
        // TEST 1 — GET ALL USERS RETURNS LIST
        // ========================================
        [Fact]
        public async Task GetAllUtilisateurs_Should_Return_All_Users_With_Roles()
        {
            // Act
            var result = await _controller.GetAllUtilisateurs() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var users = result.Value as List<UtilisateurDTO>;
            Assert.NotNull(users);
            Assert.Equal(2, users.Count);
            Assert.Contains(users, u => u.Email == "user1@test.com");
            Assert.Contains(users, u => u.Email == "user2@test.com");
        }

        // ========================================
        // TEST 2 — GET USER BY ID SUCCESS
        // ========================================
        [Fact]
        public async Task GetUtilisateurById_Should_Return_User_When_Exists()
        {
            // Act
            var result = await _controller.GetUtilisateurById("user1") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var userDto = result.Value as UtilisateurDTO;
            Assert.NotNull(userDto);
            Assert.Equal("user1@test.com", userDto.Email);
            Assert.Equal("Dupont", userDto.Nom);
            Assert.Equal("Jean", userDto.Prenom);
        }

        // ========================================
        // TEST 3 — GET USER BY ID NOT FOUND
        // ========================================
        [Fact]
        public async Task GetUtilisateurById_Should_Return_NotFound_When_User_DoesNotExist()
        {
            // Act
            var result = await _controller.GetUtilisateurById("nonexistent");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // ========================================
        // TEST 4 — GET USER COMMANDES
        // ========================================
        [Fact]
        public async Task GetCommandesUtilisateur_Should_Return_User_Orders()
        {
            // Arrange
            var commandesDto = new List<CommandeDTO>
            {
                new CommandeDTO
                {
                    Id = 1,
                    UtilisateurId = "user1",
                    DateCommande = DateTime.Now,
                    Total = 100,
                    Statut = "EnCours"
                },
                new CommandeDTO
                {
                    Id = 2,
                    UtilisateurId = "user1",
                    DateCommande = DateTime.Now,
                    Total = 200,
                    Statut = "Livrée"
                }
            };

            _mockCommandeService
                .Setup(s => s.GetAllByUtilisateurIdAsync("user1"))
                .ReturnsAsync((IEnumerable<CommandeDTO>)commandesDto);

            // Act
            var result = await _controller.GetCommandesUtilisateur("user1") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var commandes = result.Value as IEnumerable<CommandeDTO>;
            Assert.NotNull(commandes);
            Assert.Equal(2, commandes.Count());
        }

        // ========================================
        // TEST 5 — GET USER COMMANDES NOT FOUND
        // ========================================
        [Fact]
        public async Task GetCommandesUtilisateur_Should_Return_NotFound_When_User_DoesNotExist()
        {
            // Act
            var result = await _controller.GetCommandesUtilisateur("nonexistent");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // ========================================
        // TEST 6 — GET COMMANDE DETAILS SUCCESS
        // ========================================
        [Fact]
        public async Task GetCommandeDetails_Should_Return_Commande_When_Belongs_To_User()
        {
            // Arrange
            var commandeDto = new CommandeDTO
            {
                Id = 1,
                UtilisateurId = "user1",
                DateCommande = DateTime.Now,
                Total = 150,
                Statut = "EnCours",
                LigneCommandes = new List<LigneCommandeDTO>()
            };

            _mockCommandeService
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(commandeDto);

            // Act
            var result = await _controller.GetCommandeDetails("user1", 1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var commande = result.Value as CommandeDTO;
            Assert.NotNull(commande);
            Assert.Equal(1, commande.Id);
            Assert.Equal("user1", commande.UtilisateurId);
        }

        // ========================================
        // TEST 7 — GET COMMANDE DETAILS WRONG USER
        // ========================================
        [Fact]
        public async Task GetCommandeDetails_Should_Return_BadRequest_When_Order_BelongsToAnotherUser()
        {
            // Arrange
            var commandeDto = new CommandeDTO
            {
                Id = 1,
                UtilisateurId = "user2",  // Appartient à user2
                DateCommande = DateTime.Now,
                Total = 150,
                Statut = "EnCours"
            };

            _mockCommandeService
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(commandeDto);

            // Act
            var result = await _controller.GetCommandeDetails("user1", 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // ========================================
        // TEST 8 — GET USER STATS WITH ENUMS
        // ========================================
        [Fact]
        public async Task GetUtilisateurStats_Should_Return_Correct_Statistics()
        {
            // Arrange
            var commandesDto = new List<CommandeDTO>
            {
                new CommandeDTO
                {
                    Id = 1,
                    UtilisateurId = "user1",
                    Total = 100,
                    Statut = "EnCours",
                    DateCommande = DateTime.Now
                },
                new CommandeDTO
                {
                    Id = 2,
                    UtilisateurId = "user1",
                    Total = 200,
                    Statut = "Livrée",
                    DateCommande = DateTime.Now
                },
                new CommandeDTO
                {
                    Id = 3,
                    UtilisateurId = "user1",
                    Total = 150,
                    Statut = "Annulée",
                    DateCommande = DateTime.Now
                }
            };

            _mockCommandeService
                .Setup(s => s.GetAllByUtilisateurIdAsync("user1"))
                .ReturnsAsync((IEnumerable<CommandeDTO>)commandesDto);

            // Act
            var result = await _controller.GetUtilisateurStats("user1") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var stats = result.Value as UtilisateurStatsDTO;
            Assert.NotNull(stats);
            Assert.Equal(3, stats.NombreCommandes);
            Assert.Equal(450, stats.TotalDepense);
            Assert.Equal(1, stats.CommandeEnCours);
            Assert.Equal(1, stats.CommandeLivree);
            Assert.Equal(1, stats.CommandeAnnulee);
        }

        // ========================================
        // TEST 9 — GET STATS USER NOT FOUND
        // ========================================
        [Fact]
        public async Task GetUtilisateurStats_Should_Return_NotFound_When_User_DoesNotExist()
        {
            // Act
            var result = await _controller.GetUtilisateurStats("nonexistent");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // ========================================
        // TEST 10 — SEARCH BY EMAIL
        // ========================================
        [Fact]
        public async Task SearchUtilisateurs_Should_Filter_By_Email()
        {
            // Act
            var result = await _controller.SearchUtilisateurs("user1@test.com", null) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var users = result.Value as List<UtilisateurDTO>;
            Assert.NotNull(users);
            Assert.Single(users);
            Assert.Equal("user1@test.com", users[0].Email);
        }

        // ========================================
        // TEST 11 — SEARCH BY NOM
        // ========================================
        [Fact]
        public async Task SearchUtilisateurs_Should_Filter_By_Nom()
        {
            // Act
            var result = await _controller.SearchUtilisateurs(null, "Dupont") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var users = result.Value as List<UtilisateurDTO>;
            Assert.NotNull(users);
            Assert.Single(users);
            Assert.Equal("Dupont", users[0].Nom);
        }

        // ========================================
        // TEST 12 — SEARCH BY PRENOM
        // ========================================
        [Fact]
        public async Task SearchUtilisateurs_Should_Filter_By_Prenom()
        {
            // Act
            var result = await _controller.SearchUtilisateurs(null, "Marie") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var users = result.Value as List<UtilisateurDTO>;
            Assert.NotNull(users);
            Assert.Single(users);
            Assert.Equal("Marie", users[0].Prenom);
        }

        // ========================================
        // TEST 13 — UPDATE USER SUCCESS
        // ========================================
        [Fact]
        public async Task UpdateUtilisateur_Should_Update_User_Info()
        {
            // Arrange
            var updatedDto = new UtilisateurDTO
            {
                Id = "user1",
                Email = "newemail@test.com",
                Nom = "NewNom",
                Prenom = "NewPrenom",
                PhoneNumber = "1111111111"
            };

            // Act
            var result = await _controller.UpdateUtilisateur("user1", updatedDto);

            // Assert
            Assert.IsType<OkResult>(result);

            // Vérifier que les données ont été mises à jour
            var user = await _userManager.FindByIdAsync("user1");
            Assert.NotNull(user);
            Assert.Equal("newemail@test.com", user.Email);
            Assert.Equal("NewNom", user.Nom);
            Assert.Equal("NewPrenom", user.Prenom);
            Assert.Equal("1111111111", user.PhoneNumber);
        }

        // ========================================
        // TEST 14 — UPDATE USER NOT FOUND
        // ========================================
        [Fact]
        public async Task UpdateUtilisateur_Should_Return_NotFound_When_User_DoesNotExist()
        {
            // Arrange
            var updatedDto = new UtilisateurDTO
            {
                Email = "test@test.com",
                Nom = "Test",
                Prenom = "Test"
            };

            // Act
            var result = await _controller.UpdateUtilisateur("nonexistent", updatedDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // ========================================
        // TEST 15 — SEARCH NO RESULTS
        // ========================================
        [Fact]
        public async Task SearchUtilisateurs_Should_Return_Empty_When_No_Match()
        {
            // Act
            var result = await _controller.SearchUtilisateurs("nonexistent@test.com", null) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var users = result.Value as List<UtilisateurDTO>;
            Assert.NotNull(users);
            Assert.Empty(users);
        }
    }
}