using GestionParfumerie.Application.DTOs;

namespace GestionParfumerie.Application.Services
{
    public interface ICommandeService
    {
        Task<IEnumerable<CommandeDTO>> GetAllAsync();
        Task<CommandeDTO> GetByIdAsync(int id);
        Task<CommandeDTO> CreateAsync(string utilisateurId, CommandeCreateDTO dto);
        Task<bool> UpdateStatutAsync(int id, string statut);
        Task<IEnumerable<CommandeDTO>> GetAllByUtilisateurIdAsync(string utilisateurId);
    }
}
