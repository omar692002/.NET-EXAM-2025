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
                .Include(c => c.Utilisateur)
                .Include(c => c.LigneCommandes)
                    .ThenInclude(lc => lc.Produit)
                        .ThenInclude(p => p.Categorie) // On va jusqu'à la catégorie
                .Include(c => c.LigneCommandes)
                    .ThenInclude(lc => lc.Produit)
                        .ThenInclude(p => p.Brand) // On va jusqu'au brand
                .Include(c => c.LigneCommandes)
                    .ThenInclude(lc => lc.Produit)
                        .ThenInclude(p => p.ProduitFamilles) // CORRECTION : utiliser ProduitFamilles
                            .ThenInclude(pf => pf.FamilleOlfactive) // puis FamilleOlfactive
                .ToListAsync();
        }

        public async Task<Commande?> GetByIdAsync(int id)
        {
            return await _context.Commandes
                .Include(c => c.Utilisateur)
                .Include(c => c.LigneCommandes)
                    .ThenInclude(lc => lc.Produit)
                        .ThenInclude(p => p.Categorie)
                .Include(c => c.LigneCommandes)
                    .ThenInclude(lc => lc.Produit)
                        .ThenInclude(p => p.Brand)
                .Include(c => c.LigneCommandes)
                    .ThenInclude(lc => lc.Produit)
                        .ThenInclude(p => p.ProduitFamilles)
                            .ThenInclude(pf => pf.FamilleOlfactive)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
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
