using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GestionParfumerie.Application.Interfaces;
namespace GestionParfumerie.Infrastructure.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categorie>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.Produits)
                    .ThenInclude(p => p.Brand)  
                .Include(c => c.Produits)
                    .ThenInclude(p => p.ProduitFamilles)
                        .ThenInclude(pf => pf.FamilleOlfactive)
                .ToListAsync();
        }

        public async Task<Categorie?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Produits)
                    .ThenInclude(p => p.Brand)
                .Include(c => c.Produits)
                    .ThenInclude(p => p.ProduitFamilles)
                        .ThenInclude(pf => pf.FamilleOlfactive)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Categorie> AddAsync(Categorie categorie)
        {
            _context.Categories.Add(categorie);
            await _context.SaveChangesAsync();
            return categorie;
        }

        public async Task UpdateAsync(Categorie categorie)
        {
            _context.Categories.Update(categorie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var categorie = await GetByIdAsync(id);
            if (categorie != null)
            {
                _context.Categories.Remove(categorie);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> HasProductsAsync(int categorieId)
        {
            return await _context.Produits.AnyAsync(p => p.CategorieId == categorieId);
        }
    }
}
