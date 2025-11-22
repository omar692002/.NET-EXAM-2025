using GestionParfumerie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionParfumerie.Application.Interfaces
{
    public interface ILigneCommandesRepository
    {
        Task<IEnumerable<LigneCommande>> GetAllAsync();
        Task<LigneCommande> GetByIdAsync(int id);
        Task AddAsync(LigneCommande ligne);
        Task UpdateAsync(LigneCommande ligne);
        Task DeleteAsync(int id);
    }
}
