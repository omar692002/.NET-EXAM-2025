using GestionParfumerie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionParfumerie.Infrastructure.Repositories
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Categorie>> GetAllAsync();
        Task<Categorie> GetByIdAsync(int id);
        Task AddAsync(Categorie categorie);
        Task UpdateAsync(Categorie categorie);
        Task DeleteAsync(int id);
    }
}
