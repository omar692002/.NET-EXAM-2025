namespace GestionParfumerie.Application.Interfaces
{
    public interface IUtilisateurRepository
    {
        Task<int> GetTotalClientsCountAsync();
        Task<int> GetNewClientsCountSinceDateAsync(DateTime date);
    }
}