using AutoMapper;
using GestionParfumerie.Controllers;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using GestionParfumerie.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

using Microsoft.EntityFrameworkCore.InMemory;

namespace GestionParfumerie.Tests.Integration
{
    public class BrandsIntegrationTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly BrandsRepository _repo;
        private readonly IMapper _mapper;
        private readonly BrandsController _controller;

        public BrandsIntegrationTests()
        {
            // --- CONFIG DB IN-MEMORY avec un nom unique par test ---
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Création de la base de données
            _context.Database.EnsureCreated();

            _repo = new BrandsRepository(_context);

            // --- CONFIG AUTOMAPPER ---
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BrandsDTO, Brand>().ReverseMap();
            });

            _mapper = config.CreateMapper();

            // --- CONTROLLER ---
            _controller = new BrandsController(_repo, _mapper);
        }

        // Nettoyage après chaque test
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // ========================================
        // 1 — CREATE WORKS (Integration Test)
        // ========================================
        [Fact]
        public async Task CreateBrand_Should_Add_To_Database()
        {
            // Arrange
            var dto = new BrandsDTO
            {
                Nom = "BrandTest",
                Email = "brand@test.com",
                Adresse = "Adresse 123"
            };

            // Act
            var result = await _controller.Create(dto) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);

            var created = result.Value as BrandsDTO;
            Assert.NotNull(created);

            var brandInDb = await _context.Fournisseurs.FindAsync(created.Id);
            Assert.NotNull(brandInDb);
            Assert.Equal("BrandTest", brandInDb.Nom);
        }

        // ========================================
        // 2 — GET ALL RETURNS LIST
        // ========================================
        [Fact]
        public async Task GetAll_Should_Return_All_Brands()
        {
            // Arrange
            _context.Fournisseurs.Add(new Brand
            {
                Nom = "B1",
                Email = "b1@test.com",
                Adresse = "Adr1"
            });
            _context.Fournisseurs.Add(new Brand
            {
                Nom = "B2",
                Email = "b2@test.com",
                Adresse = "Adr2"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var list = result.Value as IEnumerable<BrandsDTO>;
            Assert.NotNull(list);
            Assert.Equal(2, list.Count());
        }

        // ========================================
        // 3 — DELETE SHOULD FAIL IF PRODUCTS EXIST
        // ========================================
        [Fact]
        public async Task DeleteBrand_Should_ReturnBadRequest_If_ProductsExist()
        {
            // Arrange
            var brand = new Brand
            {
                Nom = "BrandProd",
                Email = "bp@test.com",
                Adresse = "AdrBP",
                Produits = new List<Produit> { new Produit { Nom = "P1" } }
            };

            _context.Fournisseurs.Add(brand);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(brand.Id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}