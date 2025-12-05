using AutoMapper;
using GestionParfumerie.Application.DTOs;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace GestionParfumerie.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProduitsController : ControllerBase
    {
        private readonly IProduitRepository _repo;
  
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public ProduitsController(IProduitRepository repo, IMapper mapper, ApplicationDbContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> GetAll()
        {
            var produits = await _repo.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<ProduitDTO>>(produits);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var produit = await _repo.GetByIdAsync(id);
            if (produit == null) return NotFound();
            return Ok(_mapper.Map<ProduitDTO>(produit));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProduitCreateUpdateDTO model)
        {
            var produit = _mapper.Map<Produit>(model);

            await _repo.AddAsync(produit);

            var result = _mapper.Map<ProduitDTO>(produit);

            return CreatedAtAction(nameof(GetById), new { id = produit.Id }, result);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProduitCreateUpdateDTO model)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Met à jour l'entité existante
            _mapper.Map(model, existing);

            await _repo.UpdateAsync(existing);

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<bool> DeleteAsync(int id)
        {
            var produit = await _context.Produits
                              .Include(p => p.LigneCommandes)
                              .FirstOrDefaultAsync(p => p.Id == id);

            if (produit == null)
                return false;

            if (produit.LigneCommandes.Any())
                throw new InvalidOperationException("Impossible de supprimer un produit déjà utilisé dans une commande.");

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
            return true;
        }
     
    }
}
