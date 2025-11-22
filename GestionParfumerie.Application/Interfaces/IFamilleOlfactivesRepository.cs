using GestionParfumerie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionParfumerie.Application.Interfaces
{
    public interface IFamilleOlfactivesRepository
    {
        Task<IEnumerable<FamilleOlfactive>> GetAllAsync();
        Task<FamilleOlfactive> GetByIdAsync(int id);
        Task AddAsync(FamilleOlfactive famille);
        Task UpdateAsync(FamilleOlfactive familleOlfactive);
        Task DeleteAsync(int id);
    }
}
