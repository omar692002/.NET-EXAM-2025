using GestionParfumerie.Domain.Entities;


namespace GestionParfumerie.Infrastructure.Repositories
{
    public interface IProduitRepository
    {
        Task<IEnumerable<Produit>> GetAllAsync();
        Task<Produit> GetByIdAsync(int id);
        Task AddAsync(Produit produit);
        Task UpdateAsync(Produit produit);
        Task DeleteAsync(int id);
    }
}