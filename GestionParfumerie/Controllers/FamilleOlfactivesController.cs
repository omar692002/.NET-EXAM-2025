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
    public class FamilleOlfactivesController : ControllerBase
    {
        private readonly IFamilleOlfactivesRepository _repo;
        private readonly IMapper _mapper;

        public FamilleOlfactivesController(IFamilleOlfactivesRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var familles = await _repo.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<FamilleOlfactiveDTO>>(familles);
            return Ok(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FamilleOlfactiveDTO dto)
        {
            var famille = _mapper.Map<FamilleOlfactive>(dto);
            await _repo.AddAsync(famille);
            return Ok(_mapper.Map<FamilleOlfactiveDTO>(famille));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FamilleOlfactiveDTO dto)
        {
            var famille = await _repo.GetByIdAsync(id);
            if (famille == null) return NotFound();

            famille.Nom = dto.Nom;
            await _repo.UpdateAsync(famille);
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
