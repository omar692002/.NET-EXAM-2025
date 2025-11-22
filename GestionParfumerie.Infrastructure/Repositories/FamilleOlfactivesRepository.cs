using GestionParfumerie.Domain.Entities;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GestionParfumerie.Application.Interfaces;

namespace GestionParfumerie.Infrastructure.Repositories
{
    public class FamilleOlfactivesRepository : IFamilleOlfactivesRepository
    {
        private readonly ApplicationDbContext _context;
        public FamilleOlfactivesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FamilleOlfactive>> GetAllAsync()
        {
            return await _context.FamillesOlfactives.ToListAsync();
        }

        public async Task<FamilleOlfactive> GetByIdAsync(int id)
        {
            return await _context.FamillesOlfactives.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(FamilleOlfactive famille)
        {
            _context.FamillesOlfactives.Add(famille);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FamilleOlfactive famille)
        {
            _context.FamillesOlfactives.Update(famille);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var famille = await GetByIdAsync(id);
            if (famille != null)
            {
                _context.FamillesOlfactives.Remove(famille);
                await _context.SaveChangesAsync();
            }
        }
    }
}
