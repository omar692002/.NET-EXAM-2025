using GestionParfumerie.Domain.Entities;


namespace GestionParfumerie.Application.Interfaces
{
    public interface IFournisseursRepository
    {
        Task<IEnumerable<Fournisseur>> GetAllAsync();
        Task<Fournisseur> GetByIdAsync(int id);
        Task AddAsync(Fournisseur fournisseur);
        Task UpdateAsync(Fournisseur fournisseur);
        Task DeleteAsync(int id);
    }
}