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

        public async Task<IEnumerable<Produit>> GetAllAsync()
        {
            return await _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Fournisseur)
                .Include(p => p.FamilleOlfactive)
                .ToListAsync();
        }

        public async Task<Produit> GetByIdAsync(int id)
        {
            return await _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Fournisseur)
                .Include(p => p.FamilleOlfactive)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Produit produit)
        {
            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Produit produit)
        {
            _context.Produits.Update(produit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var produit = await GetByIdAsync(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
                await _context.SaveChangesAsync();
            }
        }
    }
}
