using GestionParfumerie.Application.Interfaces;
using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionParfumerie.Infrastructure.Repositories
{
    public class ProduitRepository : IProduitRepository
    {
        private readonly ApplicationDbContext _context;

        public ProduitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produit>> GetAllWithRelationsAsync()
        {
            return await _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Brand)
                .Include(p => p.ProduitFamilles)
                    .ThenInclude(pf => pf.FamilleOlfactive)
                .ToListAsync();
        }

        public async Task<Produit?> GetByIdWithRelationsAsync(int id)
        {
            return await _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Brand)
                .Include(p => p.ProduitFamilles)
                    .ThenInclude(pf => pf.FamilleOlfactive)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produit?> GetByIdAsync(int id)
        {
            return await _context.Produits
                .Include(p => p.ProduitFamilles)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produit> AddAsync(Produit produit)
        {
            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();
            return produit;
        }

        public async Task UpdateAsync(Produit produit)
        {
            _context.Produits.Update(produit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Produit produit)
        {
            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasOrdersAsync(int produitId)
        {
            return await _context.LigneCommandes
                .AnyAsync(lc => lc.ProduitId == produitId);
        }

        public async Task AddFamillesAsync(int produitId, IEnumerable<int> familleIds)
        {
            foreach (var familleId in familleIds)
            {
                var produitFamille = new ProduitFamilleOlfactive
                {
                    ProduitId = produitId,
                    FamilleOlfactiveId = familleId
                };
                _context.ProduitFamilleOlfactives.Add(produitFamille);
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAllFamillesAsync(int produitId)
        {
            var familles = await _context.ProduitFamilleOlfactives
                .Where(pf => pf.ProduitId == produitId)
                .ToListAsync();

            _context.ProduitFamilleOlfactives.RemoveRange(familles);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Produits.CountAsync();
        }
    }
}