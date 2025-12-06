using GestionParfumerie.Domain.Entities;


namespace GestionParfumerie.Application.Interfaces
{
    public interface IProduitRepository
    {
        Task<IEnumerable<Produit>> GetAllWithRelationsAsync();
        Task<Produit?> GetByIdWithRelationsAsync(int id);
        Task<Produit> AddAsync(Produit produit);
        Task<Produit?> GetByIdAsync(int id);
        Task UpdateAsync(Produit produit);
        Task DeleteAsync(Produit produit);
        Task<bool> HasOrdersAsync(int produitId);

        // Méthodes pour gérer les familles olfactives
        Task AddFamillesAsync(int produitId, IEnumerable<int> familleIds);
        Task RemoveAllFamillesAsync(int produitId);
        Task<int> GetTotalCountAsync();
    }
}