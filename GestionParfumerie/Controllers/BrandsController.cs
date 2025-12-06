using AutoMapper;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionParfumerie.Application.Interfaces;
namespace GestionParfumerie.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandsRepository _repo;
        private readonly IMapper _mapper;

        public BrandsController(IBrandsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _repo.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<BrandsDTO>>(brands);
            return Ok(dto);
        }

        // GET BY ID
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _repo.GetByIdAsync(id);
            if (brand == null)
                return NotFound();

            return Ok(_mapper.Map<BrandsDTO>(brand));
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(BrandsDTO dto)
        {
            var brand = _mapper.Map<Brand>(dto);
            await _repo.AddAsync(brand);

            return CreatedAtAction(nameof(GetById), new { id = brand.Id },
                _mapper.Map<BrandsDTO>(brand));
        }

        // UPDATE
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, BrandsDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            _mapper.Map(dto, existing); 

            await _repo.UpdateAsync(existing);
            return NoContent();
        }

        // DELETE (avec règle métier)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _repo.GetByIdAsync(id);
            if (brand == null)
                return NotFound();

            if (brand.Produits.Any())
                return BadRequest("Impossible de supprimer un brand ayant des produits.");

            await _repo.DeleteAsync(id);
            return Ok("Brand supprimé avec succès");
        }
    }

}
