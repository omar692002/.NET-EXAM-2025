using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GestionParfumerie.Application.Interfaces;


namespace GestionParfumerie.Infrastructure.Repositories
{
    public class FournisseursRepository : IFournisseursRepository
    {
        private readonly ApplicationDbContext _context;

        public FournisseursRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Get all suppliers ---
        public async Task<IEnumerable<Fournisseur>> GetAllAsync()
        {
            return await _context.Fournisseurs
                .Include(f => f.Produits) 
                .ToListAsync();
        }

        // --- Get supplier by Id ---
        public async Task<Fournisseur?> GetByIdAsync(int id)
        {
            return await _context.Fournisseurs
                .Include(f => f.Produits)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        // --- Add a new supplier ---
        public async Task AddAsync(Fournisseur fournisseur)
        {
            _context.Fournisseurs.Add(fournisseur);
            await _context.SaveChangesAsync();
        }

        // --- Update an existing supplier ---
        public async Task UpdateAsync(Fournisseur fournisseur)
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
