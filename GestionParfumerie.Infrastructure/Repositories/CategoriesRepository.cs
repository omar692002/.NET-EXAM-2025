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
            return await _context.Categories.ToListAsync();
        }

        public async Task<Categorie> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Categorie categorie)
        {
            _context.Categories.Add(categorie);
            await _context.SaveChangesAsync();
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
    }
}
