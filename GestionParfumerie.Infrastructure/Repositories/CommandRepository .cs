using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionParfumerie.Infrastructure.Repositories
{
    public class CommandesRepository : ICommandesRepository
    {
        private readonly ApplicationDbContext _context;

        public CommandesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Get all commandes with related data ---
        public async Task<IEnumerable<Commande>> GetAllAsync()
        {
            return await _context.Commandes
                .Include(c => c.Utilisateur)
                .Include(c => c.LigneCommandes)
                    .ThenInclude(lc => lc.Produit)
                .ToListAsync();
        }

        // --- Get one commande by Id ---
        public async Task<Commande?> GetByIdAsync(int id)
        {
            return await _context.Commandes
                .Include(c => c.Utilisateur)
                .Include(c => c.LigneCommandes)
                    .ThenInclude(lc => lc.Produit)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // --- Add a new commande ---
        public async Task AddAsync(Commande commande)
        {
            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();
        }

        // --- Update an existing commande ---
        public async Task UpdateAsync(Commande commande)
        {
            _context.Commandes.Update(commande);
            await _context.SaveChangesAsync();
        }

        // --- Delete a commande ---
        public async Task DeleteAsync(int id)
        {
            var commande = await GetByIdAsync(id);
            if (commande != null)
            {
                _context.Commandes.Remove(commande);
                await _context.SaveChangesAsync();
            }
        }
    }
}
