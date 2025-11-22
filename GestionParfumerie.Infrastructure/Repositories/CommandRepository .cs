using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GestionParfumerie.Application.Interfaces;

namespace GestionParfumerie.Infrastructure.Repositories
{
    public class CommandesRepository : ICommandesRepository
    {
        private readonly ApplicationDbContext _context;

        public CommandesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Commande>> GetAllAsync()
        {
            return await _context.Commandes
                .Include(c => c.LigneCommandes)
                    .ThenInclude(l => l.Produit)
                        .ThenInclude(p => p.Categorie)
                .Include(c => c.LigneCommandes)
                    .ThenInclude(l => l.Produit)
                        .ThenInclude(p => p.Fournisseur)
                .Include(c => c.LigneCommandes)
                    .ThenInclude(l => l.Produit)
                        .ThenInclude(p => p.FamilleOlfactive)
                .Include(c => c.Utilisateur)
                .ToListAsync();
        }

        public async Task<Commande> GetByIdAsync(int id) =>
            await _context.Commandes
                .Include(c => c.LigneCommandes)
                    .ThenInclude(l => l.Produit)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Commande commande)
        {
            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Commande commande)
        {
            _context.Commandes.Update(commande);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var commande = await GetByIdAsync(id);
            if (commande != null)
            {
                _context.Commandes.Remove(commande);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Commande>> GetAllByUtilisateurIdAsync(string utilisateurId)
        {
            return await _context.Commandes
                .Where(c => c.UtilisateurId == utilisateurId)
                .Include(c => c.LigneCommandes)
                    .ThenInclude(lc => lc.Produit)
                .ToListAsync();
        }
    }
}
