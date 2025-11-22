using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GestionParfumerie.Application.Interfaces;

namespace GestionParfumerie.Infrastructure.Repositories
{
    public class LigneCommandesRepository : ILigneCommandesRepository
    {
        private readonly ApplicationDbContext _context;
        public LigneCommandesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LigneCommande>> GetAllAsync()
        {
            return await _context.LigneCommandes.Include(lc=>lc.ProduitId).Include(lc=>lc.CommandeId).ToListAsync();
        }

        public async Task<LigneCommande> GetByIdAsync(int id)
        {
            return await _context.LigneCommandes.Include(p=>p.ProduitId).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(LigneCommande ligne)
        {
            _context.LigneCommandes.Add(ligne);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LigneCommande ligneCommande)
        {
            _context.LigneCommandes.Update(ligneCommande);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ligneCommande = await GetByIdAsync(id);
            if (ligneCommande != null)
            {
                _context.LigneCommandes.Remove(ligneCommande);
                await _context.SaveChangesAsync();
            }
        }
    }
}
