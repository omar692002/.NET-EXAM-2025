using AutoMapper;
using GestionParfumerie.Application.Interfaces;
using GestionParfumerie.Controllers;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace GestionParfumerie.Tests.Tests.Unitaire
{public class CategoriesController_Create_Tests
{
    private readonly Mock<ICategoriesRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CategoriesController _controller;

    public CategoriesController_Create_Tests()
    {
        _repoMock = new Mock<ICategoriesRepository>();
        _mapperMock = new Mock<IMapper>();
        _controller = new CategoriesController(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Create_Appelle_AddAsync()
    {
        var dto = new CategorieDTO { Nom = "Test" };
        var entity = new Categorie { Id = 10, Nom = "Test", Produits = new List<Produit>() };

        _mapperMock.Setup(m => m.Map<Categorie>(dto)).Returns(entity);
        _repoMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);

        await _controller.Create(dto);

        _repoMock.Verify(r => r.AddAsync(entity), Times.Once);
    }

    [Fact]
    public async Task Create_Retourne_OK_Avec_DTO()
    {
        var dto = new CategorieDTO { Nom = "Test" };
        var entity = new Categorie { Id = 10, Nom = "Test", Produits = new List<Produit>() };

        _mapperMock.Setup(m => m.Map<Categorie>(dto)).Returns(entity);
        _mapperMock.Setup(m => m.Map<CategorieDTO>(entity)).Returns(new CategorieDTO { Id = 10, Nom = "Test" });
        _repoMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);

        var result = await _controller.Create(dto);

        var ok = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<CategorieDTO>(ok.Value);

        Assert.Equal(10, returnedDto.Id);
        Assert.Equal("Test", returnedDto.Nom);
    }
}
}