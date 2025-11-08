using AutoMapper;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionParfumerie.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProduitsController : ControllerBase
    {
        private readonly IProduitRepository _repo;
        private readonly IMapper _mapper;

        public ProduitsController(IProduitRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produits = await _repo.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<ProduitDto>>(produits);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produit = await _repo.GetByIdAsync(id);
            if (produit == null) return NotFound();
            return Ok(_mapper.Map<ProduitDto>(produit));
        }
    }
}
