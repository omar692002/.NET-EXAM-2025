using AutoMapper;
using GestionParfumerie.Application.Interfaces;
using GestionParfumerie.Controllers;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
namespace GestionParfumerie.Tests.Tests.Unitaire { 
public class FamilleOlfactivesController_Tests
{
    private readonly Mock<IFamilleOlfactivesRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FamilleOlfactivesController _controller;

    public FamilleOlfactivesController_Tests()
    {
        _repoMock = new Mock<IFamilleOlfactivesRepository>();
        _mapperMock = new Mock<IMapper>();
        _controller = new FamilleOlfactivesController(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAll_Should_Return_Empty_List_When_No_Data()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<FamilleOlfactive>());

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        var dtos = Assert.IsAssignableFrom<IEnumerable<FamilleOlfactiveDTO>>(ok.Value);

        Assert.Empty(dtos);
    }

    [Fact]
    public async Task Create_Should_Return_DTO_With_Assigned_ID()
    {
        var dto = new FamilleOlfactiveDTO { Nom = "Floral" };
        var entity = new FamilleOlfactive { Id = 5, Nom = "Floral" };

        _mapperMock.Setup(m => m.Map<FamilleOlfactive>(dto)).Returns(entity);
        _repoMock.Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<FamilleOlfactiveDTO>(entity))
                   .Returns(new FamilleOlfactiveDTO { Id = 5, Nom = "Floral" });

        var result = await _controller.Create(dto);

        var ok = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<FamilleOlfactiveDTO>(ok.Value);

        Assert.Equal(5, returnedDto.Id);
        Assert.Equal("Floral", returnedDto.Nom);
    }

    [Fact]
    public async Task Delete_Should_Call_Repo_And_Return_NoContent()
    {
        _repoMock.Setup(r => r.DeleteAsync(10)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(10);

        _repoMock.Verify(r => r.DeleteAsync(10), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }
}
}