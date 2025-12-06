using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GestionParfumerie.Application.Interfaces;


namespace GestionParfumerie.Infrastructure.Repositories
{
    public class BrandsRepository : IBrandsRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Get all suppliers ---
        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Fournisseurs  // ou Brands selon votre DbSet
                .Include(b => b.Produits)
                    .ThenInclude(p => p.Categorie)
                .Include(b => b.Produits)
                    .ThenInclude(p => p.ProduitFamilles)
                        .ThenInclude(pf => pf.FamilleOlfactive)
                .ToListAsync();
        }
        // --- Get supplier by Id ---
        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Fournisseurs
                .Include(f => f.Produits)
                    .ThenInclude(p => p.ProduitFamilles)
                        .ThenInclude(pf => pf.FamilleOlfactive)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        // --- Add a new supplier ---
        public async Task AddAsync(Brand fournisseur)
        {
            _context.Fournisseurs.Add(fournisseur);
            await _context.SaveChangesAsync();
        }

        // --- Update an existing supplier ---
        public async Task UpdateAsync(Brand fournisseur)
        {
            _context.Fournisseurs.Update(fournisseur);
            await _context.SaveChangesAsync();
        }

        // --- Delete supplier ---
        public async Task DeleteAsync(int id)
        {
            var fournisseur = await GetByIdAsync(id);
            if (fournisseur != null)
            {
                _context.Fournisseurs.Remove(fournisseur);
                await _context.SaveChangesAsync();
            }
        }
    }
}
