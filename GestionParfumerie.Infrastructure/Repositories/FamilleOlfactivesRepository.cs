using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using GestionParfumerie.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionParfumerie.Infrastructure.Repositories
{
    public class FamilleOlfactivesRepository : IFamilleOlfactivesRepository
    {
        private readonly ApplicationDbContext _context;

        public FamilleOlfactivesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FamilleOlfactive>> GetAllAsync()
        {
            return await _context.FamillesOlfactives
                .Include(f => f.ProduitFamilles)
                    .ThenInclude(pf => pf.Produit)
                        .ThenInclude(p => p.Brand)  // ✅ Charger le Brand
                .Include(f => f.ProduitFamilles)
                    .ThenInclude(pf => pf.Produit)
                        .ThenInclude(p => p.Categorie)  // ✅ Charger la Categorie
                .Include(f => f.ProduitFamilles)
                    .ThenInclude(pf => pf.Produit)
                        .ThenInclude(p => p.ProduitFamilles)  // ✅ Charger les autres familles
                            .ThenInclude(pf2 => pf2.FamilleOlfactive)
                .ToListAsync();
        }

        public async Task<FamilleOlfactive?> GetByIdAsync(int id)
        {
            return await _context.FamillesOlfactives
                .Include(f => f.ProduitFamilles)
                    .ThenInclude(pf => pf.Produit)
                        .ThenInclude(p => p.Brand)
                .Include(f => f.ProduitFamilles)
                    .ThenInclude(pf => pf.Produit)
                        .ThenInclude(p => p.Categorie)
                .Include(f => f.ProduitFamilles)
                    .ThenInclude(pf => pf.Produit)
                        .ThenInclude(p => p.ProduitFamilles)
                            .ThenInclude(pf2 => pf2.FamilleOlfactive)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AddAsync(FamilleOlfactive famille)
        {
            _context.FamillesOlfactives.Add(famille);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FamilleOlfactive famille)
        {
            _context.FamillesOlfactives.Update(famille);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var famille = await GetByIdAsync(id);
            if (famille != null)
            {
                _context.FamillesOlfactives.Remove(famille);
                await _context.SaveChangesAsync();
            }
        }
    }
}
