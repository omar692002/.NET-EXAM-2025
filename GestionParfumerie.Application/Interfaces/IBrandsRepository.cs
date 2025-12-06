using GestionParfumerie.Domain.Entities;


namespace GestionParfumerie.Application.Interfaces
{
    public interface IBrandsRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(int id);
        Task AddAsync(Brand fournisseur);
        Task UpdateAsync(Brand fournisseur);
        Task DeleteAsync(int id);
    }
}