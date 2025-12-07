using AutoMapper;
using GestionParfumerie.Application.Interfaces;
using GestionParfumerie.Controllers;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
namespace GestionParfumerie.Tests.Tests.Unitaire
{
    public class CategoriesController_Update_Tests
    {
        private readonly Mock<ICategoriesRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CategoriesController _controller;

        public CategoriesController_Update_Tests()
        {
            _repoMock = new Mock<ICategoriesRepository>();
            _mapperMock = new Mock<IMapper>();
            _controller = new CategoriesController(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Update_Retourne_NotFound_Si_Inexistant()
        {
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Categorie?)null);

            var dto = new CategorieDTO { Nom = "Test" };

            var result = await _controller.Update(99, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_Modifie_Proprietes()
        {
            var existing = new Categorie { Id = 1, Nom = "Ancien", Description = "Desc", Produits = new List<Produit>() };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var dto = new CategorieDTO { Nom = "Nouveau", Description = "New Desc" };

            await _controller.Update(1, dto);

            Assert.Equal("Nouveau", existing.Nom);
            Assert.Equal("New Desc", existing.Description);
        }

        [Fact]
        public async Task Update_Appelle_UpdateAsync_Une_Fois()
        {
            var cat = new Categorie { Id = 1, Nom = "Test", Produits = new List<Produit>() };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cat);

            var dto = new CategorieDTO { Nom = "X" };

            await _controller.Update(1, dto);

            _repoMock.Verify(r => r.UpdateAsync(cat), Times.Once);
        }
    }
}