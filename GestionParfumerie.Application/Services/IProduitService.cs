using GestionParfumerie.Application.DTOs;

namespace GestionParfumerie.Application.Services
{
    public interface IProduitService
    {
        Task<IEnumerable<ProduitDTO>> GetAllAsync();
        Task<ProduitDTO?> GetByIdAsync(int id);
        Task<ProduitDTO> CreateAsync(ProduitCreateUpdateDTO dto);
        Task<bool> UpdateAsync(int id, ProduitCreateUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}