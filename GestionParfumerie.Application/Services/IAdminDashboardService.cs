using GestionParfumerie.Application.DTOs;

namespace GestionParfumerie.Application.Services
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardStatsDTO> GetDashboardStatsAsync();
    }
}