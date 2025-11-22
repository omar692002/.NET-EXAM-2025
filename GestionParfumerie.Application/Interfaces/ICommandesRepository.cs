using GestionParfumerie.Domain.Entities;

namespace GestionParfumerie.Application.Interfaces
{
    public interface ICommandesRepository
    {
        Task<IEnumerable<Commande>> GetAllAsync();
        Task<Commande> GetByIdAsync(int id);
        Task AddAsync(Commande commande);
        Task UpdateAsync(Commande commande);
        Task DeleteAsync(int id);
        Task<IEnumerable<Commande>> GetAllByUtilisateurIdAsync(string utilisateurId);
    }
}