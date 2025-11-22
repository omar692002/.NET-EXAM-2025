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
    public class FournisseursController : ControllerBase
    {
        private readonly IFournisseursRepository _repo;
        private readonly IMapper _mapper;

        public FournisseursController(IFournisseursRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fournisseurs = await _repo.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<FournisseurDTO>>(fournisseurs);
            return Ok(dto);
        }

        // GET BY ID
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fournisseur = await _repo.GetByIdAsync(id);
            if (fournisseur == null)
                return NotFound();

            return Ok(_mapper.Map<FournisseurDTO>(fournisseur));
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(FournisseurDTO dto)
        {
            var fournisseur = _mapper.Map<Fournisseur>(dto);
            await _repo.AddAsync(fournisseur);

            return CreatedAtAction(nameof(GetById), new { id = fournisseur.Id },
                _mapper.Map<FournisseurDTO>(fournisseur));
        }

        // UPDATE
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, FournisseurDTO dto)
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
            var fournisseur = await _repo.GetByIdAsync(id);
            if (fournisseur == null)
                return NotFound();

            if (fournisseur.Produits.Any())
                return BadRequest("Impossible de supprimer un fournisseur ayant des produits.");

            await _repo.DeleteAsync(id);
            return Ok("Fournisseur supprimé avec succès");
        }
    }

}
