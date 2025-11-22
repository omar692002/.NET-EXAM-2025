using AutoMapper;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Application.Interfaces;
using GestionParfumerie.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionParfumerie.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository _repo;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoriesRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repo.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<CategorieDTO>>(categories);
            return Ok(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategorieDTO dto)
        {
            var categorie = _mapper.Map<Categorie>(dto);
            await _repo.AddAsync(categorie);
            return Ok(_mapper.Map<CategorieDTO>(categorie));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategorieDTO dto)
        {
            var categorie = await _repo.GetByIdAsync(id);
            if (categorie == null) return NotFound();

            categorie.Nom = dto.Nom;
            categorie.Description = dto.Description;

            await _repo.UpdateAsync(categorie);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }

}
