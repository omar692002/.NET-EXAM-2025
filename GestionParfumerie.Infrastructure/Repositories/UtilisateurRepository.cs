using GestionParfumerie.Application.Interfaces;
using GestionParfumerie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionParfumerie.Infrastructure.Repositories
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly ApplicationDbContext _context;

        public UtilisateurRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalClientsCountAsync()
        {
            // Adapter selon votre structure (si vous avez une table Roles séparée)
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetNewClientsCountSinceDateAsync(DateTime date)
        {
            return await _context.Users
                .Where(u => u.CreatedAt >= date)
                .CountAsync();
        }
    }
}